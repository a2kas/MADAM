using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Constants;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Ui.Pages.Tools.Components;

public partial class RetailCodeGeneratorDialog
{
    public GenerateRetailCodesModel Model { get; set; } = new GenerateRetailCodesModel()
    {
        Country = new CountryModel()
        {
            Value = BalticCountry.LV,
            Name = "LV",
        },
        CodePrefix = CommonConstants.LvRetailPrefix,
        AmountToGenerate = 1,
    };

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IJSRuntime _jsRuntime { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _generating;
    private List<GeneratedRetailCodeModel> _generatedCodes = new();
    private MudForm? _form;

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") 
        {
            await Submit();
        }
    }

    private async Task Submit()
    {
        try
        {
            _generating = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new GenerateRetailCodesCommand(Model));
            if (result.Succeeded)
            {
                _generatedCodes = result.Data;
                Snackbar.Add($"{Model.AmountToGenerate} retail codes generated successfully", Severity.Success);
            }
            else
            {
                _generatedCodes = new();
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        finally
        {
            _generating = false;
        }
    }

    private async Task CopyToClipboard()
    {
        var textToCopy = string.Join(Environment.NewLine, _generatedCodes.Select(x => x.FullCode));
        await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", textToCopy);
    }

    private void Cancel() => _mudDialog.Cancel();
}
