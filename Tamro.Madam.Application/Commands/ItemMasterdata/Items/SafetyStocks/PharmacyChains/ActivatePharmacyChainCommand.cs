using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

public class ActivatePharmacyChainCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public ActivatePharmacyChainCommand(int[] id)
    {
        Id = id;
    }

    public string ErrorMessage { get; set; } = "Failed to activate pharmacy chains";

    public int[] Id { get; }
}
