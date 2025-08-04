using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Forms;

public class FormModel : BaseDataGridModel<FormModel>
{
    public int Id { get; set; }
    [DisplayName("Form name")]
    public string Name { get; set; }
}
