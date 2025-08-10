using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Customers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.SafetyStocks;

[RequiresPermission(Permissions.CanManageCanceledOrderLineNotificationReceivers)]
public class ExcludedCustomersQueryHandler : IRequestHandler<ExcludedCustomersQuery, PaginatedData<ExcludedCustomerGridModel>>
{
    private readonly IWholesaleCustomerRepositoryFactory _wholesaleCustomerRepositoryFactory;
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ExcludedCustomersQueryHandler(IWholesaleCustomerRepositoryFactory wholesaleCustomerRepositoryFactory, IMadamUnitOfWork uow, IMapper mapper)
    {
        _wholesaleCustomerRepositoryFactory = wholesaleCustomerRepositoryFactory;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ExcludedCustomerGridModel>> Handle(ExcludedCustomersQuery request, CancellationToken cancellationToken)
    {
        var customerLegalEntities = await _uow.GetRepository<CustomerLegalEntity>()
            .AsReadOnlyQueryable()
            .Include(x => x.NotificationSettings)
            .Include(x => x.Customer)
            .Include(x => x.Customer.CustomerNotification)
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<CustomerLegalEntity, ExcludedCustomerGridModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

        if (customerLegalEntities.Items.Any())
        {
            var e1SoldToNumbers = customerLegalEntities.Items.Select(x => x.E1SoldTo).Distinct();
            var customers = (
                    await _wholesaleCustomerRepositoryFactory.Get(request.Country ?? BalticCountry.LV)
                        .GetClsf(addressNumbers: e1SoldToNumbers, WholesaleCustomerType.LegalEntity, 1, int.MaxValue)
                ).Items;

            var customerDictionary = customers.ToDictionary(c => c.AddressNumber, c => c.Name);

            foreach (var excludedCustomer in customerLegalEntities.Items)
            {
                if (customerDictionary.TryGetValue(excludedCustomer.E1SoldTo, out var customerName))
                {
                    excludedCustomer.Name = customerName;
                }
            }
        }

        return customerLegalEntities;
    }
}
