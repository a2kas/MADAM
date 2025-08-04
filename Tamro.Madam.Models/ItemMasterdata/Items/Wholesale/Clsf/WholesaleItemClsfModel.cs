namespace Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;

public class WholesaleItemClsfModel
{
    public string ItemNo { get; set; }
    public string Name { get; set; }
    public string DisplayName { get { return $"{ItemNo} - {Name}"; } }
}
