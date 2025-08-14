using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.Repositories.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler : IRequestHandler<ExcludeCustomerFromCanceledOrderLineNotificationsCommand, Result<int>>
{
    private readonly ICustomerLegalEntityRepository _customerLegalEntityRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IMadamUnitOfWork _uow;

    public ExcludeCustomerFromCanceledOrderLineNotificationsCommandHandler(
        ICustomerLegalEntityRepository customerLegalEntityRepository,
        ICustomerRepository customerRepository,
        IMadamUnitOfWork uow)
    {
        _customerLegalEntityRepository = customerLegalEntityRepository;
        _customerRepository = customerRepository;
        _uow = uow;
    }

    public async Task<Result<int>> Handle(ExcludeCustomerFromCanceledOrderLineNotificationsCommand request, CancellationToken cancellationToken)
    {
        var model = request.Model;
        var includes = new List<IncludeOperation<CustomerLegalEntity>>
        {
            new(q => q.Include(c => c.NotificationSettings)),
            new(q => q.Include(c => c.Customers).ThenInclude(c => c.CustomerNotification))
        };

        var customerLegalEntity = await _customerLegalEntityRepository
            .Get(x => x.E1SoldTo == model.Customer.AddressNumber && x.Country == model.Country,
                includes,
                true,
                cancellationToken);

        if (customerLegalEntity == null)
        {
            customerLegalEntity = new CustomerLegalEntity()
            {
                E1SoldTo = model.Customer.AddressNumber,
                Country = model.Country,
                IsActive = true,
            };
        }

        if (model.ExclusionType == ExclusionLevel.EntireLegalEntity)
        {
            if (customerLegalEntity.NotificationSettings == null)
            {
                customerLegalEntity.NotificationSettings = new CustomerLegalEntityNotification
                {
                    CustomerLegalEntityId = customerLegalEntity.Id,
                    SendCanceledOrderNotification = false
                };
            }
            else
            {
                customerLegalEntity.NotificationSettings.SendCanceledOrderNotification = false;
            }

            if (customerLegalEntity.Customers?.Any() == true)
            {
                foreach (var customer in customerLegalEntity.Customers)
                {
                    if (customer.CustomerNotification != null)
                    {
                        customer.CustomerNotification.SendCanceledOrderNotification = true;
                    }
                }
            }
        }
        else if (model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
        {
            if (customerLegalEntity.NotificationSettings != null)
            {
                customerLegalEntity.NotificationSettings.SendCanceledOrderNotification = true;
            }

            await HandleLocationExclusionsSimple(customerLegalEntity, model.SelectedShipToAddresses, cancellationToken);
        }

        var result = await _customerLegalEntityRepository.Upsert(customerLegalEntity, cancellationToken);
        return Result<int>.Success(result.Id);
    }

    private async Task HandleLocationExclusionsSimple(CustomerLegalEntity customerLegalEntity, List<int> selectedShipToAddresses, CancellationToken cancellationToken)
    {
        if (customerLegalEntity.Id == 0)
        {
            await _customerLegalEntityRepository.Upsert(customerLegalEntity, cancellationToken);
        }

        var existingCustomers = await _customerRepository.GetMany(
            x => x.CustomerLegalEntityId == customerLegalEntity.Id,
            new List<IncludeOperation<Customer>>
            {
                new(q => q.Include(c => c.CustomerNotification))
            },
            true,
            cancellationToken);

        foreach (var shipToAddress in selectedShipToAddresses)
        {
            var existingCustomer = existingCustomers.FirstOrDefault(c => c.E1ShipTo == shipToAddress);

            if (existingCustomer == null)
            {
                var newCustomer = new Customer
                {
                    CustomerLegalEntityId = customerLegalEntity.Id,
                    E1ShipTo = shipToAddress,
                    IsActive = true
                };

                await _customerRepository.Upsert(newCustomer, cancellationToken);

                var customerNotificationRepo = _uow.GetRepository<CustomerNotification>();
                var notification = new CustomerNotification
                {
                    CustomerId = newCustomer.Id,
                    SendCanceledOrderNotification = false
                };
                customerNotificationRepo.Create(notification);
            }
            else
            {
                if (existingCustomer.CustomerNotification == null)
                {
                    var customerNotificationRepo = _uow.GetRepository<CustomerNotification>();
                    var notification = new CustomerNotification
                    {
                        CustomerId = existingCustomer.Id,
                        SendCanceledOrderNotification = false
                    };
                    customerNotificationRepo.Create(notification);
                }
                else
                {
                    existingCustomer.CustomerNotification.SendCanceledOrderNotification = false;
                }
            }
        }

        foreach (var customer in existingCustomers.Where(c => !selectedShipToAddresses.Contains(c.E1ShipTo)))
        {
            if (customer.CustomerNotification != null)
            {
                customer.CustomerNotification.SendCanceledOrderNotification = true;
            }
        }

        await _uow.SaveChangesAsync(cancellationToken);
    }
}