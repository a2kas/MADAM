using Microsoft.AspNetCore.Components;

namespace Tamro.Madam.Ui.Components.DataGrid.Filtering;

public partial class BooleanFilterTemplate
{
    [Parameter]
    public BooleanFilterOptions FilterOptions { get; set; }
    [Parameter]
    public Func<Task> CancelClick { get; set; }
    [Parameter]
    public Func<Task> ApplyClick { get; set; }

    private void Open()
    {
        FilterOptions.IsOpen = true;
    }

    private void Close()
    {
        FilterOptions.IsOpen = false;
    }

    private void YesCheckChanged(bool value)
    {
        FilterOptions.YesChecked = value;
    }

    private void NoCheckChanged(bool value)
    {
        FilterOptions.NoChecked = value;
    }
}
