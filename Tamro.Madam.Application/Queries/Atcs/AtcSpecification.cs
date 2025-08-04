using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Repository.Entities.Atcs;

namespace Tamro.Madam.Application.Queries.Atcs;

public class AtcSpecification : Specification<Atc>
{
    public AtcSpecification(AtcFilter filter)
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
            if (appliedFilter.Column.PropertyName.Equals(nameof(AtcModel.Value)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Value);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(AtcModel.Name)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Name);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Value.Contains(filter.SearchTerm) || x.Name.Contains(filter.SearchTerm));
        }
    }
}
