using Fluxor;
using MudBlazor;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;


namespace Tamro.Madam.Ui.Store.State.Sales;

[FeatureState]
public class CanceledLinesStatisticsFilterState
{
    private static readonly int _maxDays = -31;

    private DateRange _filterDateRange { get; set; } = new DateRange(DateTime.UtcNow.AddDays(_maxDays), DateTime.UtcNow);
    public DateRange FilterDateRange
    {
        get { return _filterDateRange; }
        set
        {
            _filterDateRange = value;
            if (_filterDateRange.Start.HasValue)
            {
                FilterDatesModel.DateFrom = _filterDateRange.Start.Value;
            }
            if (_filterDateRange.End.HasValue)
            {
                FilterDatesModel.DateTill = _filterDateRange.End.Value;
            }
        }
    }

    public CanceledILinesFilterDatesModel FilterDatesModel { get; } = new CanceledILinesFilterDatesModel { DateFrom = DateTime.UtcNow.AddDays(_maxDays), DateTill = DateTime.UtcNow };
    public StatisticsViewType ViewType { get; set; } = StatisticsViewType.Items;
    public IEnumerable<CancelationReason> CancelationReasons { get; set; } = [];
}
