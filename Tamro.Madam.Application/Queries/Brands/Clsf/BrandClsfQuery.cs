using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Brands.Clsf;

public class BrandClsfQuery : BrandClsfFilter, IRequest<PaginatedData<BrandClsfModel>>
{
    public BrandClsfSpecification Specification => new(this);
}
