using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Repositories.Sales;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.Statistics;
[RequiresPermission(Permissions.CanViewCanceledOrderLines)]
public class GetCanceledLinesStatisticsCommandHandler : IRequestHandler<GetCanceledLinesStatisticsCommand, Result<IEnumerable<CanceledLineStatisticModel>>>
{
    private readonly ISalesOrderCustomerDecorator _canceledOrderCustomerDecorator;
    private readonly ICanceledOrderLineItemDecorator _canceledOrderLineItemDecorator;
    private readonly ICanceledLineStatisticRepository _canceledLineStatisticRepository;

    public GetCanceledLinesStatisticsCommandHandler(ISalesOrderCustomerDecorator canceledOrderCustomerDecorator, ICanceledOrderLineItemDecorator canceledOrderLineItemDecorator, ICanceledLineStatisticRepository canceledLineStatisticRepository)
    {
        _canceledOrderCustomerDecorator = canceledOrderCustomerDecorator;
        _canceledOrderLineItemDecorator = canceledOrderLineItemDecorator;
        _canceledLineStatisticRepository = canceledLineStatisticRepository;
    }

    public async Task<Result<IEnumerable<CanceledLineStatisticModel>>> Handle(GetCanceledLinesStatisticsCommand request, CancellationToken cancellationToken)
    {
        var canceledLineStatistics = await _canceledLineStatisticRepository.GetStatistics(request.Country, request.DateFrom, request.DateTill);

        if (canceledLineStatistics.Any())
        {
            await _canceledOrderLineItemDecorator.Decorate(canceledLineStatistics, request.Country);
            await _canceledOrderCustomerDecorator.Decorate(canceledLineStatistics, request.Country);
        }

        return Result<IEnumerable<CanceledLineStatisticModel>>.Success(canceledLineStatistics);
    }
}
