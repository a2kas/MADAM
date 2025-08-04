using Microsoft.AspNetCore.Components;
using System.Collections.Immutable;

namespace Tamro.Madam.Ui.Components.DataGrid.Filtering;

partial class CountryFilterTemplate
{
    [Parameter]
    public ImmutableDictionary<string, string> Options { get; set; }
    [Parameter]
    public DictionaryFilterOptions FilterOptions { get; set; }
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

    private void SelectAllCheckedChanged()
    {
        FilterOptions.SelectAllChecked = !FilterOptions.SelectAllChecked;

        if (FilterOptions.SelectAllChecked)
        {
            FilterOptions.SelectedOptions = Options.ToHashSet();
        }
        else
        {
            FilterOptions.SelectedOptions = new();
        }
    }

    private void SelectCheckChanged(bool value, KeyValuePair<string, string> item)
    {
        if (value)
        {
            FilterOptions.SelectedOptions.Add(item);
        }
        else
        {
            FilterOptions.SelectedOptions.Remove(item);
        }

        FilterOptions.SelectAllChecked = FilterOptions.SelectedOptions.Count == Options.Count;
    }
}
