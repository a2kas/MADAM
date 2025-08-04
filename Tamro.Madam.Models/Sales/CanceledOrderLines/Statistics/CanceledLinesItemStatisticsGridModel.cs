namespace Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

public class CanceledLinesItemStatisticsGridModel : CanceledLinesItemGridModel
{
    public IEnumerable<CanceledLinesCustomerGridModel> Customers { get; set; }
}
