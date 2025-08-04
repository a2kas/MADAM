using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;

public class ItemDescriptionResolver : ICanceledOrderLinesResolver
{
    public int Priority => 3;

    private readonly IWholesaleItemRepositoryFactory _wholesaleItemRepositoryFactory;

    public ItemDescriptionResolver(IWholesaleItemRepositoryFactory wholesaleItemRepositoryFactory)
    {
        _wholesaleItemRepositoryFactory = wholesaleItemRepositoryFactory ?? throw new ArgumentNullException(nameof(wholesaleItemRepositoryFactory));
    }

    public async Task Resolve(IEnumerable<CanceledOrderHeaderModel> orders, BalticCountry country)
    {
        var itemNo2s = orders
            .Where(order => order.SendCanceledOrderNotification)
            .SelectMany(order => order.Lines)
            .Where(line => line.EmailStatus != CanceledOrderLineEmailStatus.FailureSending)
            .Select(line => line.ItemNo)
            .Distinct()
            .ToList();

        var searchModel = new WholesaleItemSearchModel
        {
            ItemNo2s = itemNo2s
        };

        var items = await _wholesaleItemRepositoryFactory.Get(country).GetMany(searchModel);

        var itemDictionary = items.ToDictionary(item => item.ItemNo, item => item.ItemDescription);

        foreach (var order in orders)
        {
            foreach (var line in order.Lines)
            {
                if (itemDictionary.TryGetValue(line.ItemNo, out var itemDescription))
                {
                    line.ItemName = itemDescription;
                }
            }
        }
    }
}
