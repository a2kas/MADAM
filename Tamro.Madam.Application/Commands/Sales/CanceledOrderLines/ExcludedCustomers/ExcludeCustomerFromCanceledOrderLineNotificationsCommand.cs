using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;

public class ExcludeCustomerFromCanceledOrderLineNotificationsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public ExcludeCustomerFromCanceledOrderLineNotificationsCommand(ExcludedCustomerDetailsModel model)
    {
        Model = model;
    }

    public ExcludedCustomerDetailsModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to exclude customer";
}
