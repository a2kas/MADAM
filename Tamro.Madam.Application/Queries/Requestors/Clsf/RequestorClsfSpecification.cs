using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;

namespace Tamro.Madam.Application.Queries.Requestors.Clsf;

public class RequestorClsfSpecification : Specification<Requestor>
{
    public RequestorClsfSpecification(RequestorClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
