using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.QualityCheck;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.QualityCheck;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class ItemQualityCheckQueryCommandHandler : IRequestHandler<ItemQualityCheckQuery, PaginatedData<ItemQualityCheckGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ItemQualityCheckQueryCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ItemQualityCheckGridModel>> Handle(ItemQualityCheckQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<ItemQualityCheck>().AsReadOnlyQueryable();

        query = query.Include(x => x.Item)
            .Include(x => x.ItemQualityCheckIssues)
            .ThenInclude(y => y.ItemBinding)
            .AsSplitQuery();

        if (request.OrderBy == nameof(ItemQualityCheckGridModel.ItemName))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Item.ItemName, request.SortDirection);
        }
        else if (request.OrderBy == nameof(ItemQualityCheckGridModel.ScanDate))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.RowVer, request.SortDirection);
        }

        return await query.ProjectToPaginatedDataAsync<ItemQualityCheck, ItemQualityCheckGridModel>
            (request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);

    }
}
