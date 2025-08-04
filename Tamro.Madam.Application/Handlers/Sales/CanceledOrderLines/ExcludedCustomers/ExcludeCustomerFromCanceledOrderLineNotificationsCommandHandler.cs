using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.Repositories.Customers;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler : IRequestHandler<ExcludeCustomerFromCanceledOrderLineNotificationsCommand, Result<int>>
{
    private readonly ICustomerLegalEntityRepository _customerLegalEntityRepository;

    public ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler(ICustomerLegalEntityRepository customerLegalEntityRepository)
    {
        _customerLegalEntityRepository = customerLegalEntityRepository;
    }

    public async Task<Result<int>> Handle(ExcludeCustomerFromCanceledOrderLineNotificationsCommand request, CancellationToken cancellationToken)
    {
        var includes = new List<IncludeOperation<CustomerLegalEntity>>
        {
            new(q => q.Include(c => c.NotificationSettings)),
        };
        var customer = await _customerLegalEntityRepository
            .Get(x => x.E1SoldTo == request.Model.Customer.AddressNumber && x.Country == request.Model.Country,
                includes,
                true,
                cancellationToken);

        if (customer == null)
        {
            customer = new CustomerLegalEntity()
            {
                E1SoldTo = request.Model.Customer.AddressNumber,
                Country = request.Model.Country,
                IsActive = true,
            };
        }
        if (customer.NotificationSettings == null)
        {
            customer.NotificationSettings = new CustomerLegalEntityNotification();
        }

        customer.NotificationSettings.SendCanceledOrderNotification = false;

        var result = await _customerLegalEntityRepository.Upsert(customer, cancellationToken);

        return Result<int>.Success(result.Id);
    }
}
