using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.EFCore.Models;
using Z.EntityFramework.Plus;
namespace Tamro.Madam.Application.Jobs;

public class ItemLogDataMigrationJob : IOneTimeJob
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMadamDbContext _madamDbContext;

    public ItemLogDataMigrationJob()
    {
    }

    public ItemLogDataMigrationJob(IMadamUnitOfWork uow, IMadamDbContext madamDbContext)
    {
        _uow = uow;
        _madamDbContext = madamDbContext;
    }

    public string Name => "Item log data migration";

    public string Description => "Migrate item log to audit tables";

    public bool Processing { get; set; }

    public async Task<Result<int>> Execute()
    {
        var deletableAuditEntries = _madamDbContext.AuditEntry
            .Where(x => x.EntityTypeName == nameof(Item))
            .ToList();

        _madamDbContext.AuditEntry.RemoveRange(deletableAuditEntries);
        await _madamDbContext.SaveChangesAsync(CancellationToken.None);

        var itemLogs = _uow.GetRepository<ItemLog>()
            .AsReadOnlyQueryable()
            .ToList();
        var itemHistoriesByItem = itemLogs.GroupBy(x => x.Id);

        foreach (var itemHistory in itemHistoriesByItem)
        {
            var orderedItemHistory = itemHistory.OrderBy(x => x.EditedAt).ToList();
            var audit = new List<DbAuditEntry>();
            var state = AuditEntryState.EntityAdded;
            foreach (var historyEntry in orderedItemHistory)
            {
                var auditEntry = new DbAuditEntry()
                {
                    EntitySetName = "Items",
                    EntityTypeName = nameof(Item),
                    State = state,
                    StateName = state.ToString(),
                    CreatedBy = GetFormattedPersonName(historyEntry.EditedBy),
                    CreatedDate = historyEntry.EditedAt ?? DateTime.Now,
                    EntityID = historyEntry.Id.ToString(),
                };

                if (state == AuditEntryState.EntityAdded)
                {
                    auditEntry.Properties = GetNewEntryAuditEntryProperties(historyEntry);
                }
                if (state == AuditEntryState.EntityModified)
                {
                    auditEntry.Properties = GetModifiedEntryAuditEntryProperties(historyEntry, audit);
                }

                if (auditEntry.Properties.Any(x => x.PropertyName != nameof(Item.EditedAt) && x.PropertyName != nameof(Item.EditedBy)))
                {
                    audit.Add(auditEntry);
                }
                state = AuditEntryState.EntityModified;
            }

            _madamDbContext.AuditEntry.AddRange(audit);
            await _madamDbContext.SaveChangesAsync(CancellationToken.None);
        }

        return Result<int>.Success(0);
    }

    private static string GetFormattedPersonName(string createdBy)
    {
        var words = createdBy.Split('.');

        for (int i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }

        return string.Join(" ", words);
    }

    private static List<DbAuditEntryProperty> GetNewEntryAuditEntryProperties(ItemLog historyEntry)
    {
        return [
                        new()
                        {
                            PropertyName = nameof(Item.Id),
                            NewValue = historyEntry.Id.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.Active),
                            NewValue = historyEntry.Active.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.ActiveSubstance),
                            NewValue = historyEntry.ActiveSubstance,
                        },
                        new()
                        {
                            PropertyName = nameof(Item.AtcId),
                            NewValue = historyEntry.AtcId?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.BrandId),
                            NewValue = historyEntry.BrandId.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.Description),
                            NewValue = historyEntry.Description,
                        },
                        new()
                        {
                            PropertyName = nameof(Item.EditedAt),
                            NewValue = null,
                        },
                        new()
                        {
                            PropertyName = nameof(Item.EditedBy),
                            NewValue = GetFormattedPersonName(historyEntry.EditedBy),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.FormId),
                            NewValue = historyEntry.FormId?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.ItemName),
                            NewValue = historyEntry.ItemName,
                        },
                        new()
                        {
                            PropertyName = nameof(Item.Measure),
                            NewValue = historyEntry.Measure?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.MeasurementUnitId),
                            NewValue = historyEntry.UomId?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.Numero),
                            NewValue = historyEntry.Numero.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.ParallelParentItemId),
                            NewValue = historyEntry.ParallelParentItemId?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.ProducerId),
                            NewValue = historyEntry.ProducerId.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.RequestorId),
                            NewValue = historyEntry.RequestorId?.ToString(),
                        },
                        new()
                        {
                            PropertyName = nameof(Item.Strength),
                            NewValue = historyEntry.Strength,
                        },
                        new()
                        {
                            PropertyName = nameof(Item.SupplierNickId),
                            NewValue = historyEntry.SupplierNickId?.ToString(),
                        },
        ];
    }

    private static List<DbAuditEntryProperty> GetModifiedEntryAuditEntryProperties(ItemLog historyEntry, List<DbAuditEntry> audit)
    {
        var result = new List<DbAuditEntryProperty>();

        var idPreviousValue = GetPreviousValue(audit, nameof(Item.Id));
        if (idPreviousValue != historyEntry.Id.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Id),
                NewValue = historyEntry.Id.ToString(),
                OldValue = idPreviousValue,
            });
        }

        var activePreviousValue = GetPreviousValue(audit, nameof(Item.Active));
        if (activePreviousValue != historyEntry.Active.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Active),
                NewValue = historyEntry.Active.ToString(),
                OldValue = activePreviousValue,
            });
        }

        var activeSubstancePreviousValue = GetPreviousValue(audit, nameof(Item.ActiveSubstance));
        if (activeSubstancePreviousValue != historyEntry.ActiveSubstance)
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.ActiveSubstance),
                NewValue = historyEntry.ActiveSubstance,
                OldValue = activeSubstancePreviousValue,
            });
        }

        var atcId = GetPreviousValue(audit, nameof(Item.AtcId));
        if (atcId != historyEntry.AtcId?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.AtcId),
                NewValue = historyEntry.AtcId?.ToString(),
                OldValue = atcId,
            });
        }

        var brandId = GetPreviousValue(audit, nameof(Item.BrandId));
        if (brandId != historyEntry.BrandId.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.BrandId),
                NewValue = historyEntry.BrandId.ToString(),
                OldValue = brandId,
            });
        }

        var descriptionPreviousValue = GetPreviousValue(audit, nameof(Item.Description));
        if (descriptionPreviousValue != historyEntry.Description)
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Description),
                NewValue = historyEntry.Description,
                OldValue = descriptionPreviousValue,
            });
        }

        var formIdPreviousValue = GetPreviousValue(audit, nameof(Item.FormId));
        if (formIdPreviousValue != historyEntry.FormId?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.FormId),
                NewValue = historyEntry.FormId?.ToString(),
                OldValue = formIdPreviousValue,
            });
        }

        var itemNamePreviousValue = GetPreviousValue(audit, nameof(Item.ItemName));
        if (itemNamePreviousValue != historyEntry.ItemName?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.ItemName),
                NewValue = historyEntry.ItemName?.ToString(),
                OldValue = itemNamePreviousValue,
            });
        }

        var measurePreviousValue = GetPreviousValue(audit, nameof(Item.Measure));
        if (measurePreviousValue != historyEntry.Measure?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Measure),
                NewValue = historyEntry.Measure?.ToString(),
                OldValue = measurePreviousValue,
            });
        }

        var measurementUnitIdPreviousValue = GetPreviousValue(audit, nameof(Item.MeasurementUnitId));
        if (measurementUnitIdPreviousValue != historyEntry.UomId?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.MeasurementUnitId),
                NewValue = historyEntry.UomId?.ToString(),
                OldValue = measurementUnitIdPreviousValue,
            });
        }

        var numeroPreviousValue = GetPreviousValue(audit, nameof(Item.Numero));
        if (numeroPreviousValue != historyEntry.Numero.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Numero),
                NewValue = historyEntry.Numero.ToString(),
                OldValue = numeroPreviousValue,
            });
        }


        var parallelParentItemIdPreviousValue = GetPreviousValue(audit, nameof(Item.ParallelParentItemId));
        if (parallelParentItemIdPreviousValue != historyEntry.ParallelParentItemId.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.ParallelParentItemId),
                NewValue = historyEntry.ParallelParentItemId.ToString(),
                OldValue = parallelParentItemIdPreviousValue,
            });
        }

        var producerIdPreviousValue = GetPreviousValue(audit, nameof(Item.ProducerId));
        if (producerIdPreviousValue != historyEntry.ProducerId.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.ProducerId),
                NewValue = historyEntry.ProducerId.ToString(),
                OldValue = producerIdPreviousValue,
            });
        }

        var requestorIdPreviousValue = GetPreviousValue(audit, nameof(Item.RequestorId));
        if (requestorIdPreviousValue != historyEntry.RequestorId?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.RequestorId),
                NewValue = historyEntry.RequestorId?.ToString(),
                OldValue = requestorIdPreviousValue,
            });
        }

        var strengthPreviousValue = GetPreviousValue(audit, nameof(Item.Strength));
        if (strengthPreviousValue != historyEntry.Strength?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.Strength),
                NewValue = historyEntry.Strength?.ToString(),
                OldValue = strengthPreviousValue,
            });
        }

        var supplierNickIdPreviousValue = GetPreviousValue(audit, nameof(Item.SupplierNickId));
        if (supplierNickIdPreviousValue != historyEntry.SupplierNickId?.ToString())
        {
            result.Add(new DbAuditEntryProperty()
            {
                PropertyName = nameof(Item.SupplierNickId),
                NewValue = historyEntry.SupplierNickId?.ToString(),
                OldValue = supplierNickIdPreviousValue,
            });
        }

        return result;
    }

    private static string GetPreviousValue(List<DbAuditEntry> auditEntries, string propertyName)
    {
        var auditEntriesByDateDescending = auditEntries.OrderByDescending(x => x.CreatedDate);
        foreach (var auditEntry in auditEntriesByDateDescending)
        {
            var property = auditEntry.Properties.SingleOrDefault(x => x.PropertyName == propertyName);
            if (property == null)
            {
                continue;
            }
            return property.NewValue;
        }

        return null;
    }
}
