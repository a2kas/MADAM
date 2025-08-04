using AutoMapper;
using MediatR;
using System.Linq.Dynamic.Core;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Administration.Configuration.Ubl;
using Tamro.Madam.Models.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Handlers.Adminitration.Configuration.Ubl;
[RequiresPermission(Permissions.CanManageUnifiedPost)]
public class UblApiKeyQueryHandler : IRequestHandler<UblApiKeyQuery, PaginatedData<UblApiKeyModel>>
{
    private readonly IE1GatewayDbContext _dbContext;
    private readonly IMapper _mapper;

    public UblApiKeyQueryHandler(IE1GatewayDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<UblApiKeyModel>> Handle(UblApiKeyQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.UblApiKeys
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<UblApiKey, UblApiKeyModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
