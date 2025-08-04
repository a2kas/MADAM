using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class ImportSafetyStocksCommand : IRequest<Result<IEnumerable<SafetyStockImportResultModel>>>, IDefaultErrorMessage
{
    public ImportSafetyStocksCommand(IEnumerable<SafetyStockGridDataModel> safetyStocks, BalticCountry country)
    {
        SafetyStocks = safetyStocks;
        Country = country;
    }

    public IEnumerable<SafetyStockGridDataModel> SafetyStocks { get; set; }
    public BalticCountry Country { get; set; }
    public string ErrorMessage { get; set; } = "Failed to import safety stock";
}
