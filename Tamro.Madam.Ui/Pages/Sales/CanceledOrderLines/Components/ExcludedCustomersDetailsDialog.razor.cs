using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Queries.Customers.Wholesale.Clsf;
using Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models;
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

    // Additional fields for enhanced functionality
    private bool _saving;
    private bool _loadingLocations = false;
    private bool _isEditMode = false;
    private List<CustomerLocationModel> _customerLocations = new();
    private MudForm? _form;
    [Parameter] public int? ExcludedCustomerId { get; set; }
    [Parameter] public bool IsEditMode { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _isEditMode = IsEditMode;

        if (_isEditMode && ExcludedCustomerId.HasValue)
        {
            await InitializeForEdit(ExcludedCustomerId.Value);
        }

        await base.OnInitializedAsync();
    }

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

    private async Task OnExclusionTypeChanged()
    {
        //try
        //{
        //    Console.WriteLine($"OnExclusionTypeChanged called with: {_model.ExclusionType}");

        //    if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations && _model.Customer != null)
        //    {
        //        await LoadCustomerLocations();
        //    }
        //    else
        //    {
        //        _customerLocations.Clear();
        //        _model.SelectedShipToAddresses.Clear();
        //    }

        //    StateHasChanged();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine($"Error in OnExclusionTypeChanged: {ex.Message}");
        //    // Handle the exception appropriately
        //}
    }

    private async Task LoadCustomerLocations()
    {
        if (_model.Customer == null) return;

        _loadingLocations = true;
        StateHasChanged();

        try
        {
            var query = new GetCustomerLocationsQuery(_model.Customer.AddressNumber, _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV);
            var result = await _mediator.Send(query);

            if (result.Succeeded)
            {
                _customerLocations = result.Data.ToList();
                if (_isEditMode)
                {
                    foreach (var location in _customerLocations)
                    {
                        location.IsSelected = _model.SelectedShipToAddresses.Contains(location.E1ShipTo);
                    }
                }
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
                _customerLocations.Clear();
            }
        }
        catch
        {
            Snackbar.Add("Failed to load customer locations", Severity.Error);
            _customerLocations.Clear();
        }
        finally
        {
            _loadingLocations = false;
            StateHasChanged();
        }
    }

    private void OnLocationSelectionChanged(CustomerLocationModel location, bool isSelected)
    {
        location.IsSelected = isSelected;

        if (isSelected)
        {
            if (!_model.SelectedShipToAddresses.Contains(location.E1ShipTo))
            {
                _model.SelectedShipToAddresses.Add(location.E1ShipTo);
            }
        }
        else
        {
            _model.SelectedShipToAddresses.Remove(location.E1ShipTo);
        }

        StateHasChanged();
    }

    private void SelectAll()
    {
        foreach (var location in _customerLocations)
        {
            location.IsSelected = true;
        }

        _model.SelectedShipToAddresses = _customerLocations.Select(x => x.E1ShipTo).ToList();
        StateHasChanged();
    }
    private void ClearAll()
    {
        foreach (var location in _customerLocations)
        {
            location.IsSelected = false;
        }

        _model.SelectedShipToAddresses.Clear();
        StateHasChanged();
    }

    private string GetSelectionSummary()
    {
        var selectedCount = _customerLocations.Count(x => x.IsSelected);
        var totalCount = _customerLocations.Count;

        return $"{selectedCount} of {totalCount} locations selected";
    }

    private string GetRowClass(CustomerLocationModel item, int index)
    {
        if (item.IsSelected)
            return "mud-error-text";
        if (item.IsExcluded)
            return "mud-warning-text";

        return string.Empty;
    }

    private bool IsFormValid()
    {
        if (_model.Customer == null)
            return false;

        if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
        {
            return _model.SelectedShipToAddresses.Any();
        }

        return true;
    }

    private EventCallback<ExclusionLevel> ExclusionTypeChangedCallback =>
    EventCallback.Factory.Create<ExclusionLevel>(this, OnExclusionTypeChanged);

    private async Task OnCustomerChanged()
    {
        if (_model.Customer != null && _model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
        {
            await LoadCustomerLocations();
        }
        else
        {
            _customerLocations.Clear();
            _model.SelectedShipToAddresses.Clear();
        }
    }

    public async Task InitializeForEdit(int excludedCustomerId)
    {
        _isEditMode = true;

        try
        {
            var query = new GetExcludedCustomerForEditQuery(excludedCustomerId);
            var result = await _mediator.Send(query);

            if (result.Succeeded)
            {
                _model = result.Data;

                if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
                {
                    await LoadCustomerLocations();
                }
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add("Failed to load customer details for editing", Severity.Error);
        }

        StateHasChanged();
    }

    private async Task Submit()
    {
        try
        {
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid || !IsFormValid())
            {
                if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations && !_model.SelectedShipToAddresses.Any())
                {
                    Snackbar.Add("Please select at least one location to exclude", Severity.Warning);
                }
                return;
            }

            _model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV;
            if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
            {
                _model.SelectedShipToAddresses = _customerLocations
                    .Where(x => x.IsSelected)
                    .Select(x => x.E1ShipTo)
                    .ToList();
            }
            else
            {
                _model.SelectedShipToAddresses.Clear();
            }

            var command = new ExcludeCustomerFromCanceledOrderLineNotificationsCommand(_model);
            var result = await _mediator.Send(command);

            if (result.Succeeded)
            {
                _mudDialog.Close(DialogResult.Ok(_model));
                var message = _isEditMode ? "Customer exclusion updated successfully" : "Customer excluded from receiving canceled order notifications";
                Snackbar.Add(message, Severity.Success);
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add("An error occurred while saving", Severity.Error);
        }
        finally
        {
            _saving = false;
        }
    }

    private void Cancel() => _mudDialog.Cancel();
}