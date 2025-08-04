using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Entities.Brands;

namespace Tamro.Madam.Application.Queries.Brands;

public class BrandSpecification : Specification<Brand>
{
    public BrandSpecification(BrandFilter filter)
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
            if (appliedFilter.Column.PropertyName.Equals(nameof(BrandModel.Name)))
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
