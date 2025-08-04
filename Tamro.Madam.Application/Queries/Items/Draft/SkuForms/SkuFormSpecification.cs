using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms;

public class SkuFormSpecification : Specification<SkuForm>
{
    public SkuFormSpecification(SkuFormFilter filter)
    {
        if (filter.Countries != null && filter.Countries.Any())
        {
            Query.Where(e => filter.Countries.Contains(e.Country));
        }

        if (filter.Types?.Any() ?? false)
        {
            Query.Where(e => filter.Types.Contains(e.Type));
        }
    }
}
