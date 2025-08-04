using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.Brands;

namespace Tamro.Madam.Application.Queries.Brands.Clsf;

public class BrandClsfSpecification : Specification<Brand>
{
    public BrandClsfSpecification(BrandClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
