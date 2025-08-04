using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Barcodes;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Barcodes;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class BarcodeQueryHandler : IRequestHandler<BarcodeQuery, PaginatedData<BarcodeGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public BarcodeQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<BarcodeGridModel>> Handle(BarcodeQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<Barcode>()
            .AsReadOnlyQueryable()
            .Include(x => x.Item)
            .AsQueryable();

        if (request.OrderBy == nameof(BarcodeGridModel.ItemName))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Item.ItemName, request.SortDirection);
        }
        else if (request.OrderBy == nameof(BarcodeGridModel.ItemId))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Item.Id, request.SortDirection);
        }
        else
        {
            query = query.OrderBy($"{request.OrderBy} {request.SortDirection}");
        }

        query = query.Include(x => x.Item);

        return await query.ProjectToPaginatedDataAsync<Barcode, BarcodeGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
