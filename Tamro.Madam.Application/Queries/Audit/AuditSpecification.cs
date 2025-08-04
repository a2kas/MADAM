using Ardalis.Specification;
using System.Collections.Immutable;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Audit;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Application.Queries.Audit;

public class AuditSpecification : Specification<DbAuditEntry>
{
    public AuditSpecification(AuditFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Properties.Any(x => x.NewValue.Contains(filter.SearchTerm)) || x.Properties.Any(x => x.OldValue.Contains(filter.SearchTerm)));
        }
        if (!string.IsNullOrEmpty(filter.EntityId))
        {
            Query.Where(x => x.EntityID == filter.EntityId);
        }
        if (!string.IsNullOrEmpty(filter.EntityTypeName))
        {
            Query.Where(x => x.EntityTypeName == filter.EntityTypeName);
        }
            
        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }

            if (appliedFilter.Column.PropertyName.Equals(nameof(AuditGridModel.EntityId)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.EntityID);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(AuditGridModel.EntityTypeName)))
            {
                var propertyValues = ((HashSet<KeyValuePair<string, string>>)appliedFilter.Value).Select(x => x.Key);
                if (propertyValues.Any())
                {
                    Query.Where(x => propertyValues.Contains(x.EntityTypeName));
                }
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(AuditGridModel.StateName)))
            {
                var propertyValues = ((HashSet<KeyValuePair<string, string>>)appliedFilter.Value).Select(x => x.Key);
                if (propertyValues.Any())
                {
                    Query.Where(x => propertyValues.Contains(x.StateName));
                }
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(AuditGridModel.CreatedBy)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.CreatedBy);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(AuditGridModel.CreatedDate)))
            {
                var date = (DateTime?)appliedFilter.Value;
                if (date.HasValue)
                {
                    date = DateTime.SpecifyKind(date.Value, DateTimeKind.Local);
                }
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.CreatedDate);
            }
        }
    }
}
