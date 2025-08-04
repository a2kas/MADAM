using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class GetSafetyStockItemInfoCommand : IRequest<Result<WholesaleSafetyStockItemModel>>, IDefaultErrorMessage
{
    public GetSafetyStockItemInfoCommand(string itemNo, BalticCountry country)
    {
        ItemNo = itemNo;
        Country = country;
    }

    public string ItemNo { get; set; }
    public BalticCountry Country { get; set; }

    public string ErrorMessage { get; set; } = "Failed to retrieve item info";
}
