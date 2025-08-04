namespace Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

public class CanceledLinesCustomerStatisticsGridModel : CanceledLinesCustomerGridModel
{
    public IEnumerable<CanceledLinesItemGridModel> Items { get; set; }
}
