using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;

namespace Tamro.Madam.Application.Queries.Nicks.Clsf;

public class NickClsfSpecification : Specification<Nick>
{
    public NickClsfSpecification(NickClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
