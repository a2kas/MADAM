using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Queries.Administration.Configuration.Ubl;
public class UblApiKeySpecification : Specification<UblApiKey>
{
    public UblApiKeySpecification(UblApiKeyFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.CustomerName.Contains(filter.SearchTerm) || x.E1SoldTo.ToString().Contains(filter.SearchTerm));
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

            if (appliedFilter.Column.PropertyName.Equals(nameof(UblApiKey.CustomerName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.CustomerName);
            }

            if (appliedFilter.Column.PropertyName.Equals(nameof(UblApiKey.E1SoldTo)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.E1SoldTo);
            }
        }
    }
}
