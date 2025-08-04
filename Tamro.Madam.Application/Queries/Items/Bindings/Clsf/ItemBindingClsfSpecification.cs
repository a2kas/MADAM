using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Clsf;

public class ItemBindingClsfSpecification : Specification<ItemBinding>
{
    public ItemBindingClsfSpecification(ItemBindingClsfFilter filter)
    {
        Query.Where(x => x.Item.ItemName.Contains(filter.SearchTerm) || x.LocalId.Contains(filter.SearchTerm));
        Query.Where(x => x.Item.Active);

        if (filter.Companies.Count != 0)
        {
            Query.Where(x => filter.Companies.Contains(x.Company));
        }
    }
}
