using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings.Vlk;

[RequiresPermission(Permissions.CanManageVlkBindings)]
public class DeleteVlkBindingsCommandHandler : IRequestHandler<DeleteVlkBindingsCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeleteVlkBindingsCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteVlkBindingsCommand command, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<VlkBinding>();

        var entities = await repo
            .AsReadOnlyQueryable()
            .Where(e => command.Id.Contains(e.Id))
            .ToListAsync(cancellationToken);

        repo.DeleteMany(entities);

        var deletedCount = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}
