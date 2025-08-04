using System.Collections.Immutable;
using Microsoft.AspNetCore.Components;

namespace Tamro.Madam.Ui.Components.DataGrid.Selecting;

public partial class AutoDictionarySelectTemplate
{
    [Parameter]
    public string Value { get; set; }

    [Parameter]
    public required ImmutableDictionary<string, string> SelectableOptions { get; set; }

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private async Task OnValueChanged(string newValue)
    {
        Value = newValue;
        await ValueChanged.InvokeAsync(newValue);
    }
}
