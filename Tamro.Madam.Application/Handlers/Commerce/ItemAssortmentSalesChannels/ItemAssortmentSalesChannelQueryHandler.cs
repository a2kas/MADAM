using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanViewItemAssortmentSalesChannels)]
public class ItemAssortmentSalesChannelQueryHandler : IRequestHandler<ItemAssortmentSalesChannelQuery, PaginatedData<ItemAssortmentSalesChannelGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemAssortmentSalesChannelQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public Task<PaginatedData<ItemAssortmentSalesChannelGridModel>> Handle(ItemAssortmentSalesChannelQuery request, CancellationToken cancellationToken)
    {
        return _uow.GetRepository<ItemAssortmentSalesChannel>()
            .AsReadOnlyQueryable()
            .Include(x => x.ItemAssortmentBindingMaps)
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<ItemAssortmentSalesChannel, ItemAssortmentSalesChannelGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
