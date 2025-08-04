using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Forms;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Forms;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class DeleteFormsCommandHandler : IRequestHandler<DeleteFormsCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeleteFormsCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteFormsCommand command, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<Form>();

        var entities = await repo
            .AsReadOnlyQueryable()
            .Where(e => command.Id.Contains(e.Id))
            .ToListAsync(cancellationToken);

        repo.DeleteMany(entities);

        var deletedCount = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}
