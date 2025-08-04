using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Vlk;

public class VlkBindingSpecification : Specification<VlkBinding>
{
    public VlkBindingSpecification(VlkBindingFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(VlkBindingGridModel.ItemName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemBinding.Item.ItemName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(VlkBindingGridModel.ItemNo2)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemBinding.LocalId);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(VlkBindingGridModel.NpakId7)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.NpakId7);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.ItemBinding.Item.ItemName!.Contains(filter.SearchTerm) || x.ItemBinding.LocalId!.Contains(filter.SearchTerm) || (x.NpakId7.ToString() ?? "").Contains(filter.SearchTerm));
        }
    }
}
