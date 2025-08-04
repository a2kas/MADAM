using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Application.Queries.Items;

public class CategoryManagerSpecification : Specification<CategoryManager>
{
    public CategoryManagerSpecification(CategoryManagerFilter filter)
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

            if (appliedFilter.Column.Equals(nameof(CategoryManagerModel.Id)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Id);
            }
            if (appliedFilter.Column.Equals(nameof(CategoryManagerModel.EmailAddress)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, appliedFilter.Value.ToString()!, x => x.EmailAddress);
            }
            if (appliedFilter.Column.Equals(nameof(CategoryManagerModel.FirstName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, appliedFilter.Value.ToString()!, x => x.FirstName);
            }
            if (appliedFilter.Column.Equals(nameof(CategoryManagerModel.LastName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, appliedFilter.Value.ToString()!, x => x.LastName);
            }
        }

        if (filter.Countries != null && filter.Countries.Any())
        {
            var selectedCountries = filter.Countries
                    .Select(Enum.Parse<BalticCountry>)
                    .ToArray();
            Query.Where(e => selectedCountries.Contains(e.Country));
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x =>
                 (x.Id.ToString() ?? "").Contains(filter.SearchTerm)
                || x.EmailAddress.Contains(filter.SearchTerm)
                || x.FirstName.Contains(filter.SearchTerm)
                || x.LastName.Contains(filter.SearchTerm));
        }
    }
}
