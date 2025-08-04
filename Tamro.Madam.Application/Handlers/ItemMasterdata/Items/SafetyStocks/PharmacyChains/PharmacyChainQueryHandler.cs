using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

[RequiresPermission(Permissions.CanViewSafetyStock)]
public class PharmacyChainQueryHandler : IRequestHandler<PharmacyChainQuery, PaginatedData<PharmacyChainModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public PharmacyChainQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<PharmacyChainModel>> Handle(PharmacyChainQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<SafetyStockPharmacyChain>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<SafetyStockPharmacyChain, PharmacyChainModel>(
                request.Specification, request.PageNumber, request.PageSize,
                _mapper.ConfigurationProvider, cancellationToken);
    }
}
