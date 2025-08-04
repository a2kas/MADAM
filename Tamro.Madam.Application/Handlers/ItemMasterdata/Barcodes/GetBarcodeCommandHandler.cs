using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Barcodes;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetBarcodeCommandHandler : IRequestHandler<GetBarcodeCommand, Result<BarcodeModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetBarcodeCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<BarcodeModel>> Handle(GetBarcodeCommand request, CancellationToken cancellationToken)
    {
        var barcode = await _uow.GetRepository<Barcode>()
            .AsReadOnlyQueryable()
            .Include(x => x.Item)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id);

        return Result<BarcodeModel>.Success(_mapper.Map<BarcodeModel>(barcode));
    }
}
