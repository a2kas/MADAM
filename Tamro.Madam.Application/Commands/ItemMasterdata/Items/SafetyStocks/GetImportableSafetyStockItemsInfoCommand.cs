using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class GetImportableSafetyStockItemsInfoCommand : IRequest<Result<IEnumerable<SafetyStockGridDataModel>>>, IDefaultErrorMessage
{
    public GetImportableSafetyStockItemsInfoCommand(string[] itemNumbers, BalticCountry country)
    {
        ItemNumbers = itemNumbers;
        Country = country;
    }

    public string[] ItemNumbers { get; set; }
    public BalticCountry Country { get; set; }

    public string ErrorMessage { get; set; } = "Failed to retrieve item infos";
}
