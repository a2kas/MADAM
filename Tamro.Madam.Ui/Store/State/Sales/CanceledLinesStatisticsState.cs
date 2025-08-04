using Fluxor;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Ui.Store.State.Sales;

[FeatureState]
public class CanceledLinesStatisticsState
{
    public List<CanceledLineStatisticModel> Data { get; set; } = new List<CanceledLineStatisticModel>();
    public CanceledLinesStatisticsFilterState Filter { get; set; } = new CanceledLinesStatisticsFilterState();
}
