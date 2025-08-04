using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Brands;

public class BrandModel : BaseDataGridModel<BrandModel>
{
    public int Id { get; set; }
    [DisplayName("Brand name")]
    public string Name { get; set; }
}
