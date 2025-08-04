using System.ComponentModel;
using Tamro.Madam.Models.Common;

namespace Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;

public class MeasurementUnitModel : BaseDataGridModel<MeasurementUnitModel>
{
    public int Id { get; set; }
    [DisplayName("Measurement unit name")]
    public string Name { get; set; }
}
