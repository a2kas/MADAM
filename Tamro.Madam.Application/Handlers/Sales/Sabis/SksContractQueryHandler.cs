using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Sales.Sabis;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Application.Handlers.Sales.Sabis;

[RequiresPermission(Permissions.CanViewSabisContracts)]
public class SksContractQueryHandler : IRequestHandler<SksContractQuery, PaginatedData<SksContractGridModel>>
{
    private readonly IE1DbContext _dbContext;
    private readonly IMapper _mapper;

    public SksContractQueryHandler(IE1DbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<SksContractGridModel>> Handle(SksContractQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.SksContractMappings
            .AsQueryable().AsNoTracking();

        if (request.OrderBy == nameof(SksContractGridModel.CustomerName))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.Customer.MailingName, request.SortDirection);
        }
        else if (request.OrderBy == nameof(SksContractGridModel.CompanyId))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.AdditionalTaxId, request.SortDirection);
        }
        else if (request.OrderBy == nameof(SksContractGridModel.EditedAt))
        {
            query = QueryableExtensions.ApplySorting(query, x => x.RowVer, request.SortDirection);
        }
        else
        {
            query = query.OrderBy($"{request.OrderBy} {request.SortDirection}");
        }

        query = query.Include(x => x.Customer);

        return await query
            .ProjectToPaginatedDataAsync<SksContractMapping, SksContractGridModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }

}
