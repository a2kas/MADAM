using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class InitializeParallelItemCommandHandler : IRequestHandler<InitializeParallelItemCommand, Result<ItemModel>>
{
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    public InitializeParallelItemCommandHandler(IItemRepository itemRepository, IMapper mapper)
    {
        _itemRepository = itemRepository;
        _mapper = mapper;
    }

    public async Task<Result<ItemModel>> Handle(InitializeParallelItemCommand request, CancellationToken cancellationToken)
    {
        var includes = new List<IncludeOperation<Item>>
        {
            new IncludeOperation<Item>(q => q.Include(i => i.Producer)),
            new IncludeOperation<Item>(q => q.Include(i => i.Brand)),
            new IncludeOperation<Item>(q => q.Include(i => i.Form)),
            new IncludeOperation<Item>(q => q.Include(i => i.Atc)),
            new IncludeOperation<Item>(q => q.Include(i => i.SupplierNick)),
            new IncludeOperation<Item>(q => q.Include(i => i.MeasurementUnit)),
            new IncludeOperation<Item>(q => q.Include(i => i.Requestor)),
        };

        var item = await _itemRepository.Get(request.Id, includes);

        var itemModel = _mapper.Map<ItemModel>(item);
        itemModel.Id = default;
        itemModel.EditedAt = default;
        itemModel.EditedBy = string.Empty;
        itemModel.ParallelParentItemId = request.Id;
        return Result<ItemModel>.Success(itemModel);
    }
}

