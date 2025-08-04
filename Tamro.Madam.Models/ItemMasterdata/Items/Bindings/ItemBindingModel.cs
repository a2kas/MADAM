using System.ComponentModel;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Bindings;

public class ItemBindingModel
{
    public int Id { get; set; }
    [DisplayName("Company")]
    public Company Company { get; set; }
    public ItemClsfModel Item { get; set; }
    [DisplayName("Local code")]
    public string LocalId { get; set; }
    [DisplayName("Languages")]
    public IEnumerable<ItemBindingLanguageModel> Languages { get; set; }
    public DateTime RowVer { get; set; }
    public byte[]? RowVerDeprecated { get; set; }
    public string EditedBy { get; set; }
}
