using System.Collections.Immutable;
using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExtensions;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Data.Sales.CanceledOrderLines;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.Sales;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines;
public partial class Statistics
{
    #region IoC
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IState<CanceledLinesStatisticsState> _statisticsState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    #endregion

    #region Properties
    private int _defaultPageSize;
    private bool _loading;
    private MudForm? _form = new();
    private MudDataGrid<CanceledLinesItemStatisticsGridModel> _table = new();
    private string[] XAxisLabels = [];
    private List<CanceledLineStatisticModel> _filteredItems;
    private IEnumerable<CanceledLinesItemStatisticsGridModel> _itemsGridDataSource = new List<CanceledLinesItemStatisticsGridModel>() {
    new CanceledLinesItemStatisticsGridModel() { }, };
    private IEnumerable<CanceledLinesCustomerStatisticsGridModel> _customersGridDataSource = new List<CanceledLinesCustomerStatisticsGridModel>() {
    new CanceledLinesCustomerStatisticsGridModel() { }, };
    private List<DateTime> _datesRange;
    private string _staticticElementsSelectLabel = string.Empty;
    private List<ChartSeries> Series = [];
    private List<ChartSeries> _filteredSeries = [];
    private IEnumerable<string>? _selectedValues;
    private List<MudComboBoxItem<string>> _items = [];
    private readonly Dictionary<string, string> _cancelationReasonsTexts = CanceledOrderHeaderData.CancellationReasons.ToDictionary(x => x.Value, x => x.Key);

    private readonly ChartOptions Options = new()
    {
        LineStrokeWidth = 1
    };

    private StatisticsViewType ViewType
    {
        get => _statisticsState.Value.Filter.ViewType;
        set
        {
            if (_statisticsState.Value.Filter.ViewType != value)
            {
                _statisticsState.Value.Filter.ViewType = value;
                UpdateSeries();
            }
        }
    }

    private IEnumerable<string>? SelectedValues
    {
        get => _selectedValues;
        set
        {
            if (!Equals(_selectedValues, value))
            {
                _selectedValues = value;
                OnSelectedValuesChanged();
            }
        }
    }

    private IEnumerable<string> CancelationReasonsTexts
    {
        get => _statisticsState.Value.Filter.CancelationReasons
            .Select(reason => CanceledOrderHeaderData.CancellationReasons[reason.ToString()]);
        set
        {
            _statisticsState.Value.Filter.CancelationReasons = value
                .Select(x => _cancelationReasonsTexts.TryGetValue(x, out var key) && Enum.TryParse<CancelationReason>(key, out var value) ? value : (CancelationReason?)null)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToList();

            UpdateSeries();
            UpdateGrids();
        }
    }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
        await OnDataFetch();
    }

    private async Task OnReset()
    {
        _statisticsState.Value.Filter.FilterDateRange = new DateRange(DateTime.UtcNow.AddDays(-31), DateTime.UtcNow);
        _statisticsState.Value.Filter.ViewType = StatisticsViewType.Items;
        await OnDataFetch();
    }

    private async Task OnDataFetch()
    {
        _loading = true;

        var cmd = new GetCanceledLinesStatisticsCommand
        (
            _userProfileState.Value.UserProfile.Country ?? BalticCountry.LT,
            _statisticsState.Value.Filter.FilterDatesModel.DateFrom,
            _statisticsState.Value.Filter.FilterDatesModel.DateTill
        );

        var statisticItems = await _mediator.Send(cmd);
        _statisticsState.Value.Data = [.. statisticItems.Data];


        var filter = _statisticsState.Value.Filter;

        _filteredItems = _statisticsState.Value.Data;

        XAxisLabels = Enumerable.Range(0, (int)(filter.FilterDatesModel.DateTill - filter.FilterDatesModel.DateFrom).TotalDays + 1)
            .Select(i => filter.FilterDatesModel.DateFrom.AddDays(i).ToString("dd MMM"))
            .ToArray();

        _datesRange = Enumerable.Range(0, (int)(filter.FilterDatesModel.DateTill - filter.FilterDatesModel.DateFrom).TotalDays + 1)
                          .Select(i => filter.FilterDatesModel.DateFrom.AddDays(i))
                          .ToList();

        UpdateSeries();
        UpdateGrids();

        StateHasChanged();

        _loading = false;
    }

    private void UpdateSeries()
    {
        List<IGrouping<string, CanceledLineStatisticModel>> groupedItems = new List<IGrouping<string, CanceledLineStatisticModel>>();

        if (_statisticsState.Value.Filter.CancelationReasons != null && _statisticsState.Value.Filter.CancelationReasons.Count() != 0 && _statisticsState.Value.Filter.CancelationReasons.Count() != CanceledOrderHeaderData.CancellationReasons.Count)
        {
            _filteredItems = [.. _statisticsState.Value.Data.Where(x => _statisticsState.Value.Filter.CancelationReasons.Contains(x.CancelationReason))];
        }
        else
        {
            _filteredItems = _statisticsState.Value.Data;
        }

        if (_statisticsState.Value.Filter.ViewType == StatisticsViewType.Items)
        {
            groupedItems = _filteredItems
                .GroupBy(i => i.ItemNo)
                .ToList();
        }

        if (_statisticsState.Value.Filter.ViewType == StatisticsViewType.Customers)
        {
            groupedItems = _filteredItems
                .GroupBy(i => i.E1ShipTo.ToString())
                .ToList();
        }

        Series = groupedItems.Select(group =>
        {
            var data = _datesRange.Select(date =>
            {
                var dayData = group.Where(i => i.CreatedDate.Date == date.Date)
                                   .Sum(i => i.CanceledQuantity);
                return (double)dayData;
            }).ToArray();

            var name = GetSeriesName(_statisticsState.Value.Filter.ViewType, group);

            return new ChartSeries
            {
                Name = name,
                Data = data
            };
        })
        .OrderByDescending(series => series.Data.Sum()).ToList();

        _filteredSeries = Series;
        InitializeStatisticSelectElements(_statisticsState.Value.Filter.ViewType);
    }

    private void UpdateGrids()
    {
        _itemsGridDataSource = _filteredItems.GroupBy(x => x.ItemNo)
            .Select(g => new CanceledLinesItemStatisticsGridModel
            {
                ItemNo = g.Key,
                ItemName = g.First().ItemName,
                CanceledQuantity = g.Sum(x => x.CanceledQuantity),
                Customers = g.GroupBy(c => new { c.CustomerName, c.E1ShipTo, c.CancelationReason })
                     .Select(cg => new CanceledLinesCustomerGridModel
                     {
                         CustomerName = cg.Key.CustomerName,
                         E1ShipTo = cg.Key.E1ShipTo.ToString(),
                         CanceledQuantity = cg.Sum(c => c.CanceledQuantity),
                         CancelationReason = cg.Key.CancelationReason
                     }).OrderByDescending(c => c.CanceledQuantity).ToList()
            }).OrderByDescending(c => c.CanceledQuantity).ToList();

        _customersGridDataSource = _filteredItems.GroupBy(x => x.E1ShipTo)
            .Select(g => new CanceledLinesCustomerStatisticsGridModel
            {
                E1ShipTo = g.Key.ToString(),
                CustomerName = g.First().CustomerName,
                CanceledQuantity = g.Sum(x => x.CanceledQuantity),
                Items = g.GroupBy(i => new { i.ItemNo, i.ItemName, i.CancelationReason })
                            .Select(ig => new CanceledLinesItemGridModel
                            {
                                ItemNo = ig.Key.ItemNo,
                                ItemName = ig.Key.ItemName,
                                CanceledQuantity = ig.Sum(i => i.CanceledQuantity),
                                CancelationReason = ig.Key.CancelationReason
                            }).OrderByDescending(c => c.CanceledQuantity).ToList()
            }).OrderByDescending(c => c.CanceledQuantity).ToList();
    }

    private void InitializeStatisticSelectElements(StatisticsViewType viewType)
    {
        _staticticElementsSelectLabel = GetStaticticElementsSelectLabel(viewType);
        _items.Clear();
        SelectedValues = new List<string>();

        foreach (var item in _filteredSeries)
        {
            var newStatisticElement = new MudComboBoxItem<string> { Value = item.Name, Text = item.Name };
            _items.Add(newStatisticElement);
        }
    }

    private void OnSelectedValuesChanged()
    {
        if (_selectedValues != null && _selectedValues.Any())
        {
            _filteredSeries = Series.Where(x => _selectedValues.Contains(x.Name)).ToList();
        }
        else
        {
            _filteredSeries = Series;
        }

        StateHasChanged();
    }

    private static string GetStaticticElementsSelectLabel(StatisticsViewType viewType) => viewType == StatisticsViewType.Items ? "Select Items" : "Select Customers";

    private static string GetSeriesName(StatisticsViewType viewType, IGrouping<string, CanceledLineStatisticModel> group)
    {
        var firstItem = group.First();
        int totalSum = group.Sum(i => i.CanceledQuantity);

        return viewType == StatisticsViewType.Items
            ? $"{firstItem.ItemName} - {firstItem.ItemNo} ({totalSum})"
            : $"{firstItem.CustomerName} - {firstItem.E1ShipTo} ({totalSum})";
    }

    private async Task OnExportStatistics()
    {
        var mappers = new Dictionary<string, Func<CanceledLineStatisticModel, object?>>
        {
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.CustomerName)), i => i.CustomerName },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.E1ShipTo)), i => i.E1ShipTo },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.ItemName)), i => i.ItemName },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.ItemNo)), i => i.ItemNo },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.CanceledQuantity)), i => i.CanceledQuantity },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.CreatedDate)), i => i.CreatedDate },
            { DisplayNameHelper.Get(typeof(CanceledLineStatisticModel), nameof(CanceledLineStatisticModel.CancelationReason)), i => CanceledOrderHeaderData.CancellationReasons[i.CancelationReason.ToString()]},
        };

        var startDate = _statisticsState.Value.Filter.FilterDateRange.Start.Value.ToString("ddMMyyyy");
        var endDate = _statisticsState.Value.Filter.FilterDateRange.End.Value.ToString("ddMMyyyy");
        var fileName = $"CanceledLines_{startDate}_{endDate}.xlsx";

        var fileContent = await _excelService.ExportAsync(_statisticsState.Value.Data, mappers, $"canceled_lines");
        await _blazorDownloadFileService.DownloadFile(fileName, fileContent, "application/octet-stream");
        Snackbar.Add("Canceled lines exported successfully", Severity.Success);
    }
}