using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
public class CanceledOrderLineItemDecorator : ICanceledOrderLineItemDecorator
{
    private readonly IWholesaleItemRepositoryFactory _wholesaleItemRepositoryFactory;

    public CanceledOrderLineItemDecorator(IWholesaleItemRepositoryFactory wholesaleItemRepositoryFactory)
    {
        _wholesaleItemRepositoryFactory = wholesaleItemRepositoryFactory;
    }

    public async Task Decorate(IEnumerable<IItemDetailsModel> lines, BalticCountry country)
    {
        var itemNo2s = lines
            .Select(line => line.ItemNo)
            .Distinct()
            .ToList();

        var searchModel = new WholesaleItemSearchModel
        {
            ItemNo2s = itemNo2s
        };

        var items = (await _wholesaleItemRepositoryFactory.Get(country).GetClsf(itemNo2s, 1, int.MaxValue)).Items;

        var itemDictionary = items.ToDictionary(item => item.ItemNo, item => item.Name);

        foreach (var line in lines)
        {
            if (itemDictionary.TryGetValue(line.ItemNo, out var itemDescription))
            {
                line.ItemName = itemDescription;
            }
        }
    }
}
