using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.Nicks;

public class NickModel : BaseDataGridModel<NickModel>
{
    public int Id { get; set; }
    [DisplayName("Baltic nick")]
    public string Name { get; set; }
}
