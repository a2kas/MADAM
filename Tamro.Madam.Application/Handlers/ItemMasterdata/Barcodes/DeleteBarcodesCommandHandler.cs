using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Barcodes;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class DeleteBarcodesCommandHandler : IRequestHandler<DeleteBarcodesCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeleteBarcodesCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteBarcodesCommand command, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<Barcode>();

        var barcodesToDelete = await repo
            .AsReadOnlyQueryable()
            .Where(x => command.Id.Contains(x.Id))
            .ToListAsync(cancellationToken);

        repo.DeleteMany(barcodesToDelete);

        var result = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(result);
    }
}
