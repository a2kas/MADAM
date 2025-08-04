using MudBlazor;

namespace Tamro.Madam.Ui.Components.DataGrid.Filtering;

public class BooleanFilterOptions : CustomFilterOptions
{
    public bool YesChecked { get; set; }
    public bool NoChecked { get; set; }
    public string Icon
    {
        get
        {
            if (YesChecked != NoChecked)
            {
                return Icons.Material.Filled.FilterAlt;
            }
            return Icons.Material.Outlined.FilterAlt;
        }
    }
}
