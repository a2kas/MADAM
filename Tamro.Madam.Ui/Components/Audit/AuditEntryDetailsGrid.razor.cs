using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.Audit;

namespace Tamro.Madam.Ui.Components.Audit;

public partial class AuditEntryDetailsGrid
{
    [Parameter]
    public IEnumerable<AuditPropertyModel> Items { get; set; }

    [Parameter]
    public string StateName { get; set; }

    [Parameter]
    public AuditPropertyModel? EntityHistoryGlobalFilter { get; set; }

    [Parameter]
    public bool? Filterable { get; set; }
    public int? Id { get; set; }
    public bool? IsExpanded { get; set; }

    protected override void OnParametersSet()
    {
        if (EntityHistoryGlobalFilter == null)
        {
            return;
        }

        Items = Items.Where(x =>
            (EntityHistoryGlobalFilter.NewValue == null || (x.NewValue ?? string.Empty).Contains(EntityHistoryGlobalFilter.NewValue, StringComparison.CurrentCultureIgnoreCase)) &&
            (EntityHistoryGlobalFilter.PropertyName == null || (x.PropertyName ?? string.Empty).Contains(EntityHistoryGlobalFilter.PropertyName, StringComparison.CurrentCultureIgnoreCase)) &&
            (EntityHistoryGlobalFilter.OldValue == null || (x.OldValue ?? string.Empty).Contains(EntityHistoryGlobalFilter.OldValue, StringComparison.CurrentCultureIgnoreCase))
        ).ToList();
    }
}