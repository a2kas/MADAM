using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Sales.CanceledOrderLines.ExcludedCustomers;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class GetCustomerLocationsQueryHandler : IRequestHandler<GetCustomerLocationsQuery, Result<IEnumerable<CustomerLocationModel>>>
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;
    private readonly IMadamUnitOfWork _uow;

    public GetCustomerLocationsQueryHandler(
        IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory,
        IMadamUnitOfWork uow)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory;
        _uow = uow;
    }

    public async Task<Result<IEnumerable<CustomerLocationModel>>> Handle(GetCustomerLocationsQuery request, CancellationToken cancellationToken)
    {
        var allCustomers = await _wholesaleCustomerRepositoryFactory.Get(request.Country)
            .GetClsf(string.Empty, WholesaleCustomerType.All, 1, int.MaxValue);

        if (!allCustomers.Items.Any())
        {
            return Result<IEnumerable<CustomerLocationModel>>.Success([]);
        }

        var legalEntity = await _uow.GetRepository<CustomerLegalEntity>()
            .AsReadOnlyQueryable()
            .Include(x => x.Customers)
            .ThenInclude(x => x.CustomerNotification)
            .FirstOrDefaultAsync(x => x.E1SoldTo == request.E1SoldTo && x.Country == request.Country, cancellationToken);

        // DEBUG: Add logging
        Console.WriteLine($"GetCustomerLocationsQuery for E1SoldTo: {request.E1SoldTo}");
        Console.WriteLine($"Found legal entity: {legalEntity != null}");

        HashSet<int> existingExclusions = [];
        if (legalEntity?.Customers?.Any() == true)
        {
            existingExclusions = legalEntity.Customers
                .Where(c => c.CustomerNotification?.SendCanceledOrderNotification == false)
                .Select(c => c.E1ShipTo)
                .ToHashSet();

            Console.WriteLine($"Existing exclusions: {string.Join(", ", existingExclusions)}");
        }

        var physicalLocations = allCustomers.Items
            .Where(x =>
                x.AddressNumber2 == request.E1SoldTo &&
                x.AddressNumber != x.AddressNumber2
            )
            .Select(x => new CustomerLocationModel
            {
                E1ShipTo = x.AddressNumber,
                Name = x.Name,
                IsExcluded = existingExclusions.Contains(x.AddressNumber),
                IsSelected = false // FIX: Always start with false, let the UI set this based on model data
            })
            .OrderBy(x => x.Name)
            .ToList();

        Console.WriteLine($"Physical locations found: {physicalLocations.Count}");
        foreach (var loc in physicalLocations)
        {
            Console.WriteLine($"  Location {loc.E1ShipTo}: IsExcluded={loc.IsExcluded}, IsSelected={loc.IsSelected}");
        }

        return Result<IEnumerable<CustomerLocationModel>>.Success(physicalLocations);
    }
}