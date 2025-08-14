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
    private readonly ICustomerNotificationRepository _customerNotificationRepository;

    public RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommandHandler(ICustomerLegalEntityNotificationRepository customerLegalEntityNotificationRepository, ICustomerNotificationRepository customerNotificationRepository)
    {
        _customerNotificationRepository = customerNotificationRepository;
        _customerLegalEntityNotificationRepository = customerLegalEntityNotificationRepository;
    }

    public async Task<Result<int>> Handle(RemoveCustomerFromExcludedCanceledOrderLineNotificationsCommand request, CancellationToken cancellationToken)
    {
        await _customerLegalEntityNotificationRepository.MarkSendCanceledOrderNotification(request.Id.ToList(), true, cancellationToken);
        await _customerNotificationRepository.MarkSendCanceledOrderNotification(request.Id.ToList(), true, cancellationToken);
        return Result<int>.Success(request.Id.Length);
    }
}
