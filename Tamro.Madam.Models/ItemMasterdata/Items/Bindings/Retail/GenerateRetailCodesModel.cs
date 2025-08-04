using System.ComponentModel;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

public class GenerateRetailCodesModel
{
    public CountryModel Country { get; set; }
    [DisplayName("Code prefix")]
    public string CodePrefix { get; set; }
    [DisplayName("Amount to generate")]
    public int AmountToGenerate { get; set; }
}
