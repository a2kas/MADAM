using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Brands.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Brands;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetBrandClsfHandler : IRequestHandler<BrandClsfQuery, PaginatedData<BrandClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetBrandClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<BrandClsfModel>> Handle(BrandClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Brand>().AsReadOnlyQueryable().OrderBy($"{nameof(Brand.Name)} asc")
                    .ProjectToPaginatedDataAsync<Brand, BrandClsfModel>(request.Specification, 1, 50, _mapper.ConfigurationProvider, cancellationToken);
    }
}
