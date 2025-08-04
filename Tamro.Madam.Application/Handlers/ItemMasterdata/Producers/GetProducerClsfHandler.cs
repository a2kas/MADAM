using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Producers.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Producers;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetProducerClsfHandler : IRequestHandler<ProducerClsfQuery, PaginatedData<ProducerClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetProducerClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ProducerClsfModel>> Handle(ProducerClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Producer>().AsReadOnlyQueryable().OrderBy($"{nameof(Producer.Name)} asc")
                    .ProjectToPaginatedDataAsync<Producer, ProducerClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
