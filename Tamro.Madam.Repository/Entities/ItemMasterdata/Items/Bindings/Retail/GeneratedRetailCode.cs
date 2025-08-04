using System.ComponentModel.DataAnnotations;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;

public class GeneratedRetailCode : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public BalticCountry Country { get; set; }
    public string CodePrefix { get; set; }
    public long Code { get; set; }
    public string GeneratedBy { get; set; }
}
