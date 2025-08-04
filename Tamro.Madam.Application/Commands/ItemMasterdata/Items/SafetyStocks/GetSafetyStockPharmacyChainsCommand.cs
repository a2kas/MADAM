using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class GetSafetyStockPharmacyChainsCommand : IRequest<Result<List<PharmacyChainModel>>>, IDefaultErrorMessage
{
    public GetSafetyStockPharmacyChainsCommand(BalticCountry? country = null, bool? isActive = null)
    {
        BalticCountry = country;
        IsActive = isActive;
    }

    public BalticCountry? BalticCountry { get; set; }
    public bool? IsActive { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve safety stock pharmacy chains";
}
