using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;

public class GetItemBarcodesCommand : IRequest<Result<IEnumerable<BarcodeModel>>>, IDefaultErrorMessage
{
    public GetItemBarcodesCommand(int itemId)
    {
        ItemId = itemId;
    }

    public int ItemId { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve barcodes";
}
