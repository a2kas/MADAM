using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Brands;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Brands;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class BrandQueryHandler : IRequestHandler<BrandQuery, PaginatedData<BrandModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public BrandQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<BrandModel>> Handle(BrandQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Brand>().AsReadOnlyQueryable().OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectToPaginatedDataAsync<Brand, BrandModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
