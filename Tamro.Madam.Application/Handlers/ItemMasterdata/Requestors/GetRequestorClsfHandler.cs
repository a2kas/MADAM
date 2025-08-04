using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Requestors.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Requestors;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetRequestorClsfHandler : IRequestHandler<RequestorClsfQuery, PaginatedData<RequestorClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetRequestorClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<RequestorClsfModel>> Handle(RequestorClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Requestor>().AsReadOnlyQueryable().OrderBy($"{nameof(Requestor.Name)} asc")
                    .ProjectToPaginatedDataAsync<Requestor, RequestorClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
