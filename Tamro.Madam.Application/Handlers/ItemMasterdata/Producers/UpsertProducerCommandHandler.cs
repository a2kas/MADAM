using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Producers;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Producers;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertProducerCommandHandler : IRequestHandler<UpsertProducerCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertProducerCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertProducerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Producer>(request.Model);
        var trackedEntity = await _uow.GetRepository<Producer>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
