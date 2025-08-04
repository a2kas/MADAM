using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Atcs.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Atcs;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Atcs;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetAtcClsfHandler : IRequestHandler<AtcClsfQuery, PaginatedData<AtcClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetAtcClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<AtcClsfModel>> Handle(AtcClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Atc>()
            .AsReadOnlyQueryable()
            .OrderBy($"{nameof(Atc.Name)} asc")
            .ProjectToPaginatedDataAsync<Atc, AtcClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
