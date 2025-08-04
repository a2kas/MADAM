using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Sales.Sabis;
using Tamro.Madam.Application.Queries.Customers.E1.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.Sales.Sabis.Components;

public partial class SabisDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public SksContractModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _saving;
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
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new UpsertSksContractCommand(Model, _userProfileState.Value.UserProfile));
            Model.Id = result.Data;

            if (result.Succeeded)
            {
                Model.Id = result.Data;
                _mudDialog.Close(DialogResult.Ok(Model));
                Snackbar.Add(GetSuccessMessage(), Severity.Success);
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        finally
        {
            _saving = false;
        }
    }

    private async Task<IEnumerable<WholesaleCustomerClsfModel>> SearchCustomers(string value, CancellationToken token)
    {
        try
        {
            var query = new E1CustomerClsfQuery()
            {
                SearchTerm = value,
            };

            var result = await _mediator.Send(query);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load customers", Severity.Error);
            return new List<WholesaleCustomerClsfModel>();
        }
    }

    private async Task OnCustomerChanged(WholesaleCustomerClsfModel clsf)
    {
        if (clsf != null)
        {
            var result = await _mediator.Send(new GetSabisCustomerCompanyIdCommand(clsf.AddressNumber));
            Model.CompanyId = result.Data;
            Model.Customer = clsf;
        }
        else
        {
            Model.CompanyId = string.Empty;
            Model.Customer = null;
        }
    }

    private string GetTitle()
    {
        return State switch
        {
            DialogState.Create => $"Create new contract mapping",
            DialogState.View => $"Contract mapping",
            DialogState.Copy => $"Copy contract mapping",
            _ => "",
        };
    }

    private string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Contract mapping '{Model.ContractTamro}' created",
            DialogState.View => $"Contract mapping '{Model.ContractTamro}' updated",
            _ => "",
        };
    }

    private void Cancel() => _mudDialog.Cancel();
}
