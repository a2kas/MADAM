using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.Sales.CanceledOrderLines;

public class CanceledOrderHeaderModel : ISalesOrderHeader
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public DateTime OrderDate { get; set; }
    public int E1ShipTo { get; set; }
    public string CustomerName { get; set; }
    public string? CustomerOrderNo { get; set; }
    public string DocumentNo { get; set; }
    public int? SoldTo { get; set; }
    public bool SendCanceledOrderNotification { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public IEnumerable<CanceledOrderLineModel> Lines { get; set; }
}
