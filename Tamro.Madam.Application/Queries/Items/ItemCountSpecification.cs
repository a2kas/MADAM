using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Queries.Items;

public class ItemCountSpecification : Specification<Item>
{
    public ItemCountSpecification()
    {
        Query.Where(x => x.Active);
    }
}
