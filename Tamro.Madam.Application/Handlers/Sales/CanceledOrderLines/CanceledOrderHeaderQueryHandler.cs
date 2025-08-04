using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanViewCanceledOrderLines)]
public class CanceledOrderHeaderQueryHandler : IRequestHandler<CanceledOrderHeaderQuery, PaginatedData<CanceledOrderHeaderGridModel>>
{
    private readonly IE1GatewayDbContext _dbContext;
    private readonly ISalesOrderCustomerDecorator _salesOrderCustomerDecorator;
    private readonly IMapper _mapper;

    public CanceledOrderHeaderQueryHandler(IE1GatewayDbContext dbContext, ISalesOrderCustomerDecorator salesOrderCustomerDecorator, IMapper mapper)
    {
        _dbContext = dbContext;
        _salesOrderCustomerDecorator = salesOrderCustomerDecorator;
        _mapper = mapper;
    }

    public async Task<PaginatedData<CanceledOrderHeaderGridModel>> Handle(CanceledOrderHeaderQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.E1CanceledOrderHeaders
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<E1CanceledOrderHeader, CanceledOrderHeaderGridModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

        if (result.Items.Any())
        {
            await _salesOrderCustomerDecorator.Decorate(result.Items, request.Country ?? BalticCountry.LV);
        }

        return result;
    }

}
