using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Assortment;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

[Table("ItemBinding")]
public class ItemBinding : IMadamEntity<int>, IAuditable, IBaseEntity
{
    [Key]
    public int Id { get; set; }
    public string? Company { get; set; }
    public int ItemId { get; set; }
    public string LocalId { get; set; }
    public DateTime? EditedAt { get; set; }
    public string? EditedBy { get; set; }
    [Timestamp]
    public byte[]? RowVerDeprecated { get; set; }

    public Item Item { get; set; }
    public ItemBindingInfo? ItemBindingInfo { get; set; }
    public ICollection<ItemBindingLanguage> Languages { get; set; }
    public ICollection<ItemAssortmentBindingMap> ItemAssortmentBindingMaps { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }
}
