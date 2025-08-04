using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertItemBindingCommandHandler : IRequestHandler<UpsertItemBindingCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertItemBindingCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertItemBindingCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ItemBinding>(request.Model);
        entity.EditedAt = DateTime.UtcNow;
        var trackedEntity = await _uow.GetRepository<ItemBinding>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
