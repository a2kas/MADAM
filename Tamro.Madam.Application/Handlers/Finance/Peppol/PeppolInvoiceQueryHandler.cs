using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Finance.Peppol;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Finance.Peppol;

namespace Tamro.Madam.Application.Handlers.Finance.Peppol;

[RequiresPermission(Permissions.CanViewPeppol)]
public class PeppolInvoiceQueryHandler : IRequestHandler<PeppolInvoiceQuery, PaginatedData<PeppolInvoiceGridModel>>
{
    private readonly IE1GatewayDbContext _dbContext;
    private readonly IMapper _mapper;

    public PeppolInvoiceQueryHandler(IE1GatewayDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedData<PeppolInvoiceGridModel>> Handle(PeppolInvoiceQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.PeppolInvoices
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<PeppolInvoice, PeppolInvoiceGridModel>(
                request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }

}
