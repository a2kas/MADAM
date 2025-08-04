using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Retail;

public class GeneratedRetailCodeSpecification : Specification<GeneratedRetailCode>
{
    public GeneratedRetailCodeSpecification(GeneratedRetailCodeFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => (x.CodePrefix + x.Code).Contains(filter.SearchTerm));
        }

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
            if (appliedFilter.Column.PropertyName.Equals(nameof(GeneratedRetailCodeModel.Code)))
            {
                Query.ApplyLongFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Code);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(GeneratedRetailCodeModel.FullCode)))
            {
                Query.Where(x => (x.CodePrefix + x.Code).Contains((string)appliedFilter.Value));
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(GeneratedRetailCodeModel.GeneratedBy)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.GeneratedBy);
            }
        }
    }
}
