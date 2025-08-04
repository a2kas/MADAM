using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class ActivateItemsCommandHandler : IRequestHandler<ActivateItemsCommand, Result<int>>
{
    private readonly IItemRepository _itemRepository;

    public ActivateItemsCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<Result<int>> Handle(ActivateItemsCommand command, CancellationToken cancellationToken)
    {
        var itemsToActivate = await _itemRepository.GetList(x => command.Id.Contains(x.Id), cancellationToken: cancellationToken);

        itemsToActivate.ToList().ForEach(x =>
        {
            x.Active = true;
            x.EditedAt = DateTime.Now;
            x.EditedBy = command.DisplayName;
        });

        var result = await _itemRepository.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(result);
    }
}
