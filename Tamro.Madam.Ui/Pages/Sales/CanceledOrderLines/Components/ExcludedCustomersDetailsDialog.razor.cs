using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Customers.Wholesale;
using Tamro.Madam.Models.Customers.Wholesale.Clsf;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.Sales.CanceledOrderLines.Components;

public partial class ExcludedCustomersDetailsDialog
{
    private ExcludedCustomerDetailsModel _model { get; set; } = new();
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

    private async Task<IEnumerable<WholesaleCustomerClsfModel>> SearchCustomers(string value, CancellationToken token)
    {
        try
        {
            var query = new WholesaleCustomerClsfQuery()
            {
                SearchTerm = value,
                Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV,
                CustomerType = WholesaleCustomerType.LegalEntity,
            };

            var result = await _mediator.Send(query, token);

            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load customers", Severity.Error);
            return new List<WholesaleCustomerClsfModel>();
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

            _model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV;
            var result = await _mediator.Send(new ExcludeCustomerFromCanceledOrderLineNotificationsCommand(_model));

            if (result.Succeeded)
            {
                _mudDialog.Close(DialogResult.Ok(_model));
                Snackbar.Add("Customer excluded from receiving canceled order notifications", Severity.Success);
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

    private void Cancel() => _mudDialog.Cancel();
}
