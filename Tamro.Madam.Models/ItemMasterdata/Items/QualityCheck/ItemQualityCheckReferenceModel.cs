namespace Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;

public class ItemQualityCheckReferenceModel
{
    public int Id { get; set; }
    public string ItemName { get; set; }
    public string Strength { get; set; }
    public string Atc { get; set; }
    public string Text { get; set; }
    public List<ItemBindingQualityCheckReferenceModel> Bindings { get; set; } = new();
}
