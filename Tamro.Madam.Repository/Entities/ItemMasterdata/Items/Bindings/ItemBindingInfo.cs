using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

public class ItemBindingInfo : IMadamEntity, IBaseEntity
{
    public int ItemBindingId { get; set; }
    public string? ShortDescription { get; set; }
    public string? DescriptionReference { get; set; }
    public string? Usage { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public ItemBinding ItemBinding { get; set; }
}
