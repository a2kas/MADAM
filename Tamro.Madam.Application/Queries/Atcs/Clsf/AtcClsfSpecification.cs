using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.Atcs;

namespace Tamro.Madam.Application.Queries.Atcs.Clsf;

public class AtcClsfSpecification : Specification<Atc>
{
    public AtcClsfSpecification(AtcClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm) || x.Value!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
