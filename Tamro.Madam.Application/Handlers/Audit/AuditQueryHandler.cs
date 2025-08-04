using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Audit;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Application.Handlers.Audit;

[RequiresPermission(Permissions.CanViewAudit)]
public class AuditQueryHandler : IRequestHandler<AuditQuery, Result<PaginatedData<AuditGridModel>>>
{
    private readonly IMadamDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AuditQueryHandler> _logger;

    public AuditQueryHandler(IMadamDbContext dbContext, IMapper mapper, ILogger<AuditQueryHandler> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<PaginatedData<AuditGridModel>>> Handle(AuditQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _dbContext.AuditEntry
                .AsNoTracking()
                .AsQueryable();

            query = query.OrderBy($"{MapOrderBy(request.OrderBy)} {request.SortDirection}");

            var data = await query.ProjectToPaginatedDataAsync<DbAuditEntry, AuditGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
            return Result<PaginatedData<AuditGridModel>>.Success(data);
        }
        catch (Exception ex)
        {
            const string errorMessage = "Failed to load audit";
            _logger.LogError(ex, errorMessage);
            return Result<PaginatedData<AuditGridModel>>.Failure(errorMessage);
        }
    }

    private static string MapOrderBy(string gridOrderBy)
    {
        return gridOrderBy switch
        {
            nameof(AuditGridModel.Id) => nameof(DbAuditEntry.EntityID),
            _ => gridOrderBy,
        };
    }
}
