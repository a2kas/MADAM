using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class GetExcludedCustomerForEditQueryHandler : IRequestHandler<GetExcludedCustomerForEditQuery, Result<ExcludedCustomerDetailsModel>>
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;
    private readonly IMadamUnitOfWork _uow;
    private readonly IMediator _mediator;

    public GetExcludedCustomerForEditQueryHandler(
        IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory,
        IMadamUnitOfWork uow,
        IMediator mediator)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory;
        _uow = uow;
        _mediator = mediator;
    }

    public async Task<Result<ExcludedCustomerDetailsModel>> Handle(GetExcludedCustomerForEditQuery request, CancellationToken cancellationToken)
    {
        var legalEntity = await _uow.GetRepository<CustomerLegalEntity>()
            .AsReadOnlyQueryable()
            .Include(x => x.NotificationSettings)
            .Include(x => x.Customers)
            .ThenInclude(x => x.CustomerNotification)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (legalEntity == null)
        {
            return Result<ExcludedCustomerDetailsModel>.Failure("Excluded customer not found");
        }

        var customers = await _wholesaleCustomerRepositoryFactory.Get(legalEntity.Country)
            .GetClsf(addressNumbers: new[] { legalEntity.E1SoldTo }, WholesaleCustomerType.LegalEntity, 1, 1);

        var customer = customers.Items.FirstOrDefault();
        if (customer == null)
        {
            return Result<ExcludedCustomerDetailsModel>.Failure("Customer not found in wholesale system");
        }

        var exclusionType = ExclusionLevel.None;
        var selectedShipToAddresses = new List<int>();

        if (legalEntity.NotificationSettings?.SendCanceledOrderNotification == false)
        {
            exclusionType = ExclusionLevel.EntireLegalEntity;
        }
        else if (legalEntity.Customers?.Any(c => c.CustomerNotification?.SendCanceledOrderNotification == false) == true)
        {
            exclusionType = ExclusionLevel.OneOrMorePhysicalLocations;
            selectedShipToAddresses = legalEntity.Customers
                .Where(c => c.CustomerNotification?.SendCanceledOrderNotification == false)
                .Select(c => c.E1ShipTo)
                .ToList();
        }

        var allCustomerData = await _wholesaleCustomerRepositoryFactory.Get(legalEntity.Country)
            .GetClsf(addressNumbers: [legalEntity.E1SoldTo], WholesaleCustomerType.All, 1, int.MaxValue);

        var availableLocations = allCustomerData.Items
            .Where(x => x.AddressNumber != legalEntity.E1SoldTo && x.AddressNumber2 == legalEntity.E1SoldTo)
            .Select(x => new CustomerLocationModel
            {
                E1ShipTo = x.AddressNumber,
                Name = x.Name,
                IsExcluded = selectedShipToAddresses.Contains(x.AddressNumber),
                IsSelected = selectedShipToAddresses.Contains(x.AddressNumber)
            })
            .OrderBy(x => x.Name)
            .ToList();

        var model = new ExcludedCustomerDetailsModel
        {
            Id = legalEntity.Id,
            Customer = new WholesaleCustomerClsfModel
            {
                AddressNumber = customer.AddressNumber,
                Name = customer.Name
            },
            Country = legalEntity.Country,
            ExclusionType = exclusionType,
            SelectedShipToAddresses = selectedShipToAddresses,
            AvailableLocations = availableLocations
        };

        return Result<ExcludedCustomerDetailsModel>.Success(model);
    }
}