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
public class CopyItemCommandHandler : IRequestHandler<CopyItemCommand, Result<ItemModel>>
{
    private readonly IItemRepository _repository;
    private readonly IMapper _mapper;

    public CopyItemCommandHandler(IItemRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ItemModel>> Handle(CopyItemCommand request, CancellationToken cancellationToken)
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

        var item = await _repository.Get(request.Id, includes);
        item.Id = default;

        return Result<ItemModel>.Success(_mapper.Map<ItemModel>(item));
    }
}
