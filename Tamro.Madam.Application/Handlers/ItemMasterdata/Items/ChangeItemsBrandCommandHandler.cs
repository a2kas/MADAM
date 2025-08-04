using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class ChangeItemsBrandCommandHandler : IRequestHandler<ChangeItemsBrandCommand, Result<IEnumerable<ItemModel>>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public ChangeItemsBrandCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ItemModel>>> Handle(ChangeItemsBrandCommand command, CancellationToken cancellationToken)
    {  
        var itemIds = command.Items.Select(x => x.Id);
        var itemsToChangeBrand = await _itemRepository.GetList(x => itemIds.Contains(x.Id), cancellationToken: cancellationToken);
        itemsToChangeBrand.ForEach(x => x.BrandId = command.NewBrand.Id ?? 0);

        var result = await _itemRepository.UpdateMany(itemsToChangeBrand, cancellationToken);
        return Result<IEnumerable<ItemModel>>.Success(_mapper.Map<IEnumerable<ItemModel>>(result));
    }
}
