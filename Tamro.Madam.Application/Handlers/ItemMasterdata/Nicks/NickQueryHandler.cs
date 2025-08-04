using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Nicks;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Nicks;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class NickQueryHandler : IRequestHandler<NickQuery, PaginatedData<NickModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public NickQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<NickModel>> Handle(NickQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Nick>().AsReadOnlyQueryable().OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectToPaginatedDataAsync<Nick, NickModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
