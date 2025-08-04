using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tamro.Madam.Ui.Components.Dialogs;

public partial class LogoutConfirmation
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public string? ContentText { get; set; }

    [Parameter]
    public Color Color { get; set; }

    private Task Submit()
    {
        MudDialog.Close(DialogResult.Ok(true));
        return Task.CompletedTask;
    }

    private void Cancel() => MudDialog.Cancel();
}
