using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Queries.Producers.Clsf;

public class ProducerClsfSpecification : Specification<Producer>
{
    public ProducerClsfSpecification(ProducerClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
