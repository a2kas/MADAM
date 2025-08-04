using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetItemCommandHandler : IRequestHandler<GetItemCommand, Result<ItemModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetItemCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<ItemModel>> Handle(GetItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _uow.GetRepository<Item>()
            .AsReadOnlyQueryable()
            .Include(x => x.Producer)
            .Include(x => x.Brand)
            .Include(x => x.Form)
            .Include(x => x.Atc)
            .Include(x => x.SupplierNick)
            .Include(x => x.MeasurementUnit)
            .Include(x => x.Barcodes)
            .Include(x => x.Bindings)
                .ThenInclude(x => x.Languages)
            .Include(x => x.Requestor)
            .AsSplitQuery()              
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        return Result<ItemModel>.Success(_mapper.Map<ItemModel>(item));
    }
}
