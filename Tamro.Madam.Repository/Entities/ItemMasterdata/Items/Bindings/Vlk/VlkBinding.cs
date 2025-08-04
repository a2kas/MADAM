using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

[Table("VlkBinding")]
public class VlkBinding : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public int NpakId7 { get; set; }

    public int ItemBindingId { get; set; }
    public ItemBinding ItemBinding { get; set; }
}
