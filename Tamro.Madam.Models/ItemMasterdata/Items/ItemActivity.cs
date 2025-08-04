using System.ComponentModel;

namespace Tamro.Madam.Models.ItemMasterdata.Items;

public enum ItemActivity
{
    [Description("All")] All,
    [Description("Active")] Active,
    [Description("Inactive")] Inactive,
}
