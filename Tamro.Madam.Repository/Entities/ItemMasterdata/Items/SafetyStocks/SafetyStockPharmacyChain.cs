using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

[Table("SafetyStockPharmacyChain")]
public class SafetyStockPharmacyChain : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public string DisplayName { get; set; }
    public int E1SoldTo { get; set; }
    public PharmacyGroup Group { get; set; }
    public bool IsActive { get; set; }
}
