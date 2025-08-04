using System.Collections.Immutable;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tamro.Madam.Ui.Components.DataGrid.Filtering;

public partial class AutoDictionaryFilterTemplate<DataT, QueryT>
{

    [Parameter]
    public required QueryT AffectedQuery { get; set; }

    [Parameter]
    public required MudDataGrid<DataT> AffectedGrid { get; set; }

    [Parameter]
    public required ImmutableDictionary<string, string> SelectableOptions { get; set; }

    [Parameter]
    public required string FilteredFieldName { get; set; }

    [Parameter]
    public required Expression<Func<QueryT, HashSet<string>?>> QueryProperty { get; set; }


    /// <summary>
    /// optional, yet necessary if You need external control over the selected options,
    /// e.g. to create a Reset button which would empty the SelectedOptions array.
    /// Also allows setting a custom Icon
    /// </summary>
    [Parameter]
    public DictionaryFilterOptions FilterOptions { get; set; } = new();

    [Parameter]
    public bool MultiSelect { get; set; } = true;



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
            FilterOptions.SelectedOptions = SelectableOptions.ToHashSet();
        }
        else
        {
            FilterOptions.SelectedOptions = new();
        }
    }

    private void SelectCheckChanged(bool value, KeyValuePair<string, string> item)
    {
        if (MultiSelect)
        {
            if (value)
            {
                FilterOptions.SelectedOptions.Add(item);
            }
            else
            {
                FilterOptions.SelectedOptions.Remove(item);
            }

            FilterOptions.SelectAllChecked = FilterOptions.SelectedOptions.Count == SelectableOptions.Count;
        }
        else
        {
            FilterOptions.SelectedOptions = new()
            {
                item,
            };
        }
    }

    private async Task CloseGroupFilter()
    {
        FilterOptions.IsOpen = false;
    }

    private async Task ApplyGroupFilter()
    {
        var propertyInfo = ((MemberExpression)QueryProperty.Body).Member as System.Reflection.PropertyInfo;

        if (propertyInfo != null)
        {
            var selectedValues = FilterOptions.SelectedOptions
                .Select(pair => pair.Value)
                .ToHashSet();

            propertyInfo.SetValue(AffectedQuery, selectedValues);
        }

        FilterOptions.IsOpen = false;
        await AffectedGrid.ReloadServerData();
    }
}
