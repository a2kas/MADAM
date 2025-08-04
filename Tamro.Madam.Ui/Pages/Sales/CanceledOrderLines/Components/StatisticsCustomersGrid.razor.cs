using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines.Components;

public partial class StatisticsCustomersGrid
{
    [Parameter]
    public IEnumerable<CanceledLinesCustomerStatisticsGridModel> Items { get; set; }
    [Parameter]
    public int RowsPerPage { get; set; }
    [Parameter]
    public bool Loading { get; set; }
}
