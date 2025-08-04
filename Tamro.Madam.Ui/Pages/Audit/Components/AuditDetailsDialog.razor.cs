using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Models.Audit;

namespace Tamro.Madam.Ui.Pages.Audit.Components;

public partial class AuditDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public AuditDetailsModel Model { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _dialog { get; set; } = default!;

    private void Close() => _dialog.Close();
}
