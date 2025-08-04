using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;

namespace Tamro.Madam.Application.Queries.Commerce.ItemAssortmentSalesChannels;

public class ItemAssortmentSalesChannelSpecification : Specification<ItemAssortmentSalesChannel>
{
    public ItemAssortmentSalesChannelSpecification(ItemAssortmentSalesChannelFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }
        if (filter.Country != null)
        {
            Query.Where(x => x.Country == filter.Country);
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemAssortmentSalesChannelGridModel.Name)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Name);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Name.Contains(filter.SearchTerm));
        }
    }
}