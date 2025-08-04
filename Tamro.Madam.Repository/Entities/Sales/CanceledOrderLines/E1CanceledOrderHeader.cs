using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.General;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

public class E1CanceledOrderHeader : IBaseEntity
{
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    [Column(TypeName = "date")]
    public DateTime OrderDate { get; set; }
    public int E1ShipTo { get; set; }
    [MaxLength(25)]
    public string? CustomerOrderNo { get; set; }
    [MaxLength(15)]
    public string? DocumentNo { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public IEnumerable<E1CanceledOrderLine> Lines { get; set; }
}
