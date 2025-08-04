using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanViewSafetyStock)]
public class SafetyStockQueryHandler : IRequestHandler<SafetyStockQuery, PaginatedData<SafetyStockGridDataModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SafetyStockQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<SafetyStockGridDataModel>> Handle(SafetyStockQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<SafetyStockGridData>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<SafetyStockGridData, SafetyStockGridDataModel>(
                request.Specification, request.PageNumber, request.PageSize,
                _mapper.ConfigurationProvider, cancellationToken);
    }
}
