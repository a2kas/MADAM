using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;

public class RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete excluded customer(s)";
}
