using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tamro.Madam.Ui.Components.Dialogs;

public partial class ExportDateRangeDialog
{
    [Parameter]
    public DateRange? DateRange { get; set; }

    [Parameter]
    public TimeSpan? MaximumAllowedRange { get; set; }

    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _isValid { get; set; } = true;
    private string _validationMessage { get; set; }
    private MudForm _form { get; set; }

    private async Task Submit()
    {
        await _form.Validate();

        _isValid = true;
        if (_form.IsValid)
        {
            if (DateRange.Start == null || DateRange.End == null || DateRange.Start.Value == DateRange.End.Value)
            {
                _validationMessage = "Date range is not selected";
                _isValid = false;
            }
            if (MaximumAllowedRange != null)
            {
                var period = DateRange.End - DateRange.Start;
                if (period > MaximumAllowedRange.Value)
                {
                    _validationMessage = $"Maximum allowed export period is {MaximumAllowedRange.Value.TotalDays} days";
                    _isValid = false;
                }
            }
        }

        if (_isValid)
        {
            _mudDialog.Close(DialogResult.Ok(DateRange));
        }
    }

    private void Cancel() => _mudDialog.Cancel();
}
