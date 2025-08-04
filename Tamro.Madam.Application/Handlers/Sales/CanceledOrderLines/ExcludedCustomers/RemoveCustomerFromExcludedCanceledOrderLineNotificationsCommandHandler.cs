using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Repositories.Customers.Notifications;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler : IRequestHandler<RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand, Result<int>>
{
    private readonly ICustomerLegalEntityNotificationRepository _customerLegalEntityNotificationRepository;

    public RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler(ICustomerLegalEntityNotificationRepository customerLegalEntityNotificationRepository)
    {
        _customerLegalEntityNotificationRepository = customerLegalEntityNotificationRepository;
    }

    public async Task<Result<int>> Handle(RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand request, CancellationToken cancellationToken)
    {
        await _customerLegalEntityNotificationRepository.MarkSendCanceledOrderNotification(request.Id.ToList(), true, cancellationToken);

        return Result<int>.Success(request.Id.Length);
    }
}
