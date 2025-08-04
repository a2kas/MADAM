using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

public class CopyPharmacyChainCommand : IRequest<Result<ItemModel>>, IDefaultErrorMessage
{
    public CopyPharmacyChainCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to copy pharmacy chain";
}
