using MudBlazor;

namespace Tamro.Madam.Ui.Components.DataGrid.Filtering;

public class DictionaryFilterOptions : CustomFilterOptions
{
    public bool SelectAllChecked { get; set; }
    public HashSet<KeyValuePair<string, string>> SelectedOptions { get; set; } = [];
    public string Icon 
    { 
        get
        {
            if (SelectedOptions.Count != 0)
            {
                return Icons.Material.Filled.FilterAlt;
            }
            return Icons.Material.Outlined.FilterAlt;
        } 
    }

    public void Reset()
    {
        IsOpen = false;
        SelectAllChecked = false;
        SelectedOptions = [];
    }
}
