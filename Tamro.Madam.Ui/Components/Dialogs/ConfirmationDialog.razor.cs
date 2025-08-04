using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Ui.Components.Dialogs;

public partial class ConfirmationDialog
{
    [EditorRequired]
    [Parameter]
    public string? Content { get; set; }

    [EditorRequired]
    [Parameter]
    public string? Title { get; set; }

    [EditorRequired]
    [Parameter]
    public string? SuccessMessage { get; set; }

    [EditorRequired]
    [Parameter]
    public string? SubmitIcon { get; set; } = Icons.Material.Filled.Delete;

    [EditorRequired]
    [Parameter]
    public string? SubmitText { get; set; } = "Delete";

    [EditorRequired]
    [Parameter]
    public Color SubmitColor { get; set; } = Color.Error;

    [EditorRequired]
    [Parameter]
    public Variant SubmitVariant { get; set; } = Variant.Filled;

    [EditorRequired]
    [Parameter]
    public IRequest<Result<int>> Command { get; set; } = default!;

    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    [Inject]
    private IMediator _mediator { get; set; }

    private async Task Submit()
    {
        var result = await _mediator.Send(Command);
        if (result.Succeeded)
        {
            Snackbar.Add(SuccessMessage, Severity.Success);
            _mudDialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private void Cancel() => _mudDialog.Cancel();
}
