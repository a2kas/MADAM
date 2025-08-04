using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.Commerce.Assortment;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Commerce.ItemAssortmentSalesChannels;

[RequiresPermission(Permissions.CanManageItemAssortmentSalesChannels)]
public class DeleteItemAssortmentSalesChannelCommandHandler : IRequestHandler<DeleteItemAssortmentSalesChannelCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public DeleteItemAssortmentSalesChannelCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(DeleteItemAssortmentSalesChannelCommand command, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<ItemAssortmentSalesChannel>();

        var entities = await repo
            .AsReadOnlyQueryable()
            .Where(e => command.Id.Contains(e.Id))
            .ToListAsync(cancellationToken);

        repo.DeleteMany(entities);

        var deletedCount = await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(deletedCount);
    }
}
