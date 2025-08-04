using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

public class DeactivatePharmacyChainCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeactivatePharmacyChainCommand(int[] id)
    {
        Id = id;
    }

    public string ErrorMessage { get; set; } = "Failed to deactivate pharmacy chains";

    public int[] Id { get; }
}
