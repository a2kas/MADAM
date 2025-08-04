using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;

namespace Tamro.Madam.Application.Handlers.Sales.HeldOrders;
[RequiresPermission(Permissions.CanViewHeldOrders)]
public class HeldOrderQueryHandler : IRequestHandler<HeldOrderQuery, PaginatedData<HeldOrderGridModel>>
{
    private readonly IE1GatewayDbContext _dbContext;
    private readonly ISalesOrderCustomerDecorator _salesOrderCustomerDecorator;
    private readonly IMapper _mapper;

    public HeldOrderQueryHandler(IE1GatewayDbContext dbContext, ISalesOrderCustomerDecorator salesOrderCustomerDecorator, IMapper mapper)
    {
        _dbContext = dbContext;
        _salesOrderCustomerDecorator = salesOrderCustomerDecorator;
        _mapper = mapper;
    }

    public async Task<PaginatedData<HeldOrderGridModel>> Handle(HeldOrderQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.E1HeldOrder
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<E1HeldOrder, HeldOrderGridModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

        if (result.Items.Any())
        {
            await _salesOrderCustomerDecorator.Decorate(result.Items, request.Country ?? BalticCountry.LV);
        }

        return result;
    }
}
