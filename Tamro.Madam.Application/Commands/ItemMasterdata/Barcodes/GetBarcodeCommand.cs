using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;

public class GetBarcodeCommand : IRequest<Result<BarcodeModel>>, IDefaultErrorMessage
{
    public GetBarcodeCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve barcode";
}
