using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Barcodes;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetItemBarcodesCommandHandler : IRequestHandler<GetItemBarcodesCommand, Result<IEnumerable<BarcodeModel>>>
{
    private readonly IBarcodeRepository _repository;
    private readonly IMapper _mapper;

    public GetItemBarcodesCommandHandler(IBarcodeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<BarcodeModel>>> Handle(GetItemBarcodesCommand request, CancellationToken cancellationToken)
    {
        var barcodes = await _repository.GetList(x => x.ItemId == request.ItemId, track: false, cancellationToken: cancellationToken);
        return Result<IEnumerable<BarcodeModel>>.Success(_mapper.Map<IEnumerable<BarcodeModel>>(barcodes));
    }
}
