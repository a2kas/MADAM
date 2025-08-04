using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;

[Table("SupplierNick")]
public class Nick : IMadamEntity<int>, IAuditable
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Item> Items { get; set; }
}
