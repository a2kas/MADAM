using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Delete;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class DeleteCategoryManagersCommandHandler : IRequestHandler<DeleteCategoryManagersCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    public DeleteCategoryManagersCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteCategoryManagersCommand request, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<CategoryManager>();

        var entities = await repo
            .AsReadOnlyQueryable()
            .Where(e => request.Ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (!entities.Any())
        {
            throw new ArgumentException($@"No category manager found by ids [{string.Join(',', request.Ids)}]");
        }

        repo.DeleteMany(entities);

        var deletedCount = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}
