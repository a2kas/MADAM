using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class ItemQueryHandler : IRequestHandler<ItemQuery, PaginatedData<ItemGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ItemGridModel>> Handle(ItemQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<Item>().AsReadOnlyQueryable();

        if (request.OrderBy == nameof(ItemGridModel.Producer))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Producer.Name, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.Brand))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Brand.Name, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.MeasurementUnit))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.MeasurementUnit.Name, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.AtcName))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Atc.Name, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.AtcCode))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Atc.Value, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.Form))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Form.Name, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemGridModel.SupplierNick))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.SupplierNick.Name, request.SortDirection);
        }
        else
        {
            query = query.OrderBy($"{request.OrderBy} {request.SortDirection}");
        }

        query = query.Include(x => x.Producer)
                   .Include(x => x.Brand)
                   .Include(x => x.Form)
                   .Include(x => x.Atc)
                   .Include(x => x.SupplierNick)
                   .Include(x => x.MeasurementUnit)
                   .AsSplitQuery();

        return await query.ProjectToPaginatedDataAsync<Item, ItemGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
