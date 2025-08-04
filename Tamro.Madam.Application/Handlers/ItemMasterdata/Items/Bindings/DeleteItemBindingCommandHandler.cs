using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class DeleteItemBindingCommandHandler : IRequestHandler<DeleteItemBindingCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeleteItemBindingCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteItemBindingCommand command, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<ItemBinding>();

        var entity = await repo
            .AsReadOnlyQueryable()
            .SingleOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        repo.Delete(entity);

        var deletedCount = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}
