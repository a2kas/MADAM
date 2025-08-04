using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Queries.Producers;

public class ProducerSpecification : Specification<Producer>
{
    public ProducerSpecification(ProducerFilter filter)
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
            if (appliedFilter.Column.PropertyName.Equals(nameof(ProducerModel.Name)))
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
