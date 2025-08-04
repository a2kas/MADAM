using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Brands;

public class BrandQuery : BrandFilter, IRequest<PaginatedData<BrandModel>>
{
    public BrandSpecification Specification => new(this);
}
