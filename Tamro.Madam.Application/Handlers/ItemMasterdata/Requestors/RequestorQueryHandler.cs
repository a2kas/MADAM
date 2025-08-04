using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Requestors;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Requestors;
[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class RequestorQueryHandler : IRequestHandler<RequestorQuery, PaginatedData<RequestorModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public RequestorQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<RequestorModel>> Handle(RequestorQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Requestor>().AsReadOnlyQueryable().OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectToPaginatedDataAsync<Requestor, RequestorModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
