using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Atcs;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Atcs;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Atcs;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class AtcQueryHandler : IRequestHandler<AtcQuery, PaginatedData<AtcModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AtcQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<AtcModel>> Handle(AtcQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Atc>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<Atc, AtcModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
