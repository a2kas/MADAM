using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Entities.Suppliers;

namespace Tamro.Madam.Application.Queries.Suppliers;

public class SupplierSpecification : Specification<Supplier>
{
    public SupplierSpecification(SupplierFilter filter)
    {
        Query.Where(x => x.Country == filter.Country);

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
            if (appliedFilter.Column.PropertyName.Equals(nameof(SupplierGridModel.Name)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SupplierGridModel.RegistrationNumber)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.RegistrationNumber);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SupplierGridModel.CreatedDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.CreatedDate);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Name.Contains(filter.SearchTerm) || x.RegistrationNumber.Contains(filter.SearchTerm));
        }
    }
}
