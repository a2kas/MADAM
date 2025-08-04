using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Table("SafetyStock")]
public class SafetyStock : IMadamEntity<int>, IAuditable
{
    [Key]
    [Column("SafetyStockItemId")]
    public int Id { get; set; }
    public int? WholesaleQuantity { get; set; }
    public decimal? RetailQuantity { get; set; }
}
