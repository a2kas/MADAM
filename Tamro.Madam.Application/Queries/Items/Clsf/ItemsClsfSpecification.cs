using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Queries.Items.Clsf;

public class ItemsClsfSpecification : Specification<Item>
{
    public ItemsClsfSpecification(ItemsClsfFilter filter)
    {
        Query.Where(x => x.ItemName!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));

        Query.Where(x => x.Active);
    }
}
