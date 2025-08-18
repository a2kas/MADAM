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

        // Always initialize the list
        _model.SelectedShipToAddresses = new List<int>();

        if (!_isEditMode)
        {
            _model.ExclusionType = ExclusionLevel.EntireLegalEntity;
        }

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
        if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations && _model.Customer != null)
        {
            await LoadCustomerLocations();
        }
        else
        {
            _customerLocations.Clear();
            _model.SelectedShipToAddresses.Clear();
        }

        StateHasChanged();
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

                // DEBUG: Log what we're loading
                Console.WriteLine($"Loaded {_customerLocations.Count} locations");
                Console.WriteLine($"Model has {_model.SelectedShipToAddresses?.Count ?? 0} selected addresses: {string.Join(", ", _model.SelectedShipToAddresses ?? new List<int>())}");

                // FIX: Set selection state based on model's SelectedShipToAddresses, not IsExcluded
                foreach (var location in _customerLocations)
                {
                    location.IsSelected = _model.SelectedShipToAddresses?.Contains(location.E1ShipTo) ?? false;
                    Console.WriteLine($"Location {location.E1ShipTo}: IsSelected = {location.IsSelected}, IsExcluded = {location.IsExcluded}, In SelectedShipToAddresses = {_model.SelectedShipToAddresses?.Contains(location.E1ShipTo)}");
                }
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
                _customerLocations.Clear();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading locations: {ex.Message}");
            Snackbar.Add("Failed to load customer locations", Severity.Error);
            _customerLocations.Clear();
        }
        finally
        {
            _loadingLocations = false;
            StateHasChanged();
        }
    }

    private void OnLocationSelectionChanged(bool isSelected, CustomerLocationModel location)
    {
        Console.WriteLine($"Selection changed for {location.E1ShipTo}: {isSelected}");

        location.IsSelected = isSelected;

        if (isSelected)
        {
            if (!_model.SelectedShipToAddresses.Contains(location.E1ShipTo))
            {
                _model.SelectedShipToAddresses.Add(location.E1ShipTo);
                Console.WriteLine($"Added {location.E1ShipTo} to selected addresses");
            }
        }
        else
        {
            _model.SelectedShipToAddresses.Remove(location.E1ShipTo);
            Console.WriteLine($"Removed {location.E1ShipTo} from selected addresses");
        }

        Console.WriteLine($"Selected addresses now: {string.Join(", ", _model.SelectedShipToAddresses)}");
        StateHasChanged();
    }

    private bool IsLocationExcluded(CustomerLocationModel location)
    {
        bool isExcluded = location.IsSelected;
        Console.WriteLine($"IsLocationExcluded called for {location.E1ShipTo}: returning {isExcluded}");
        return isExcluded;
    }

    private void SelectAll()
    {
        Console.WriteLine("Select All clicked");
        Console.WriteLine($"Locations count: {_customerLocations.Count}");

        foreach (var location in _customerLocations)
        {
            location.IsSelected = true;
            if (!_model.SelectedShipToAddresses.Contains(location.E1ShipTo))
            {
                _model.SelectedShipToAddresses.Add(location.E1ShipTo);
            }
        }

        Console.WriteLine($"After Select All - Selected addresses: {string.Join(", ", _model.SelectedShipToAddresses)}");
        StateHasChanged();
    }

    private void ClearAll()
    {
        Console.WriteLine("Clear All clicked");

        foreach (var location in _customerLocations)
        {
            location.IsSelected = false;
        }

        _model.SelectedShipToAddresses.Clear();
        Console.WriteLine("After Clear All - Selected addresses cleared");
        StateHasChanged();
    }

    private string GetSelectionSummary()
    {
        var selectedCount = _customerLocations.Count(x => x.IsSelected);
        var totalCount = _customerLocations.Count;

        return $"{selectedCount} of {totalCount} locations selected";
    }

    private bool IsFormValid()
    {
        if (_model.Customer == null)
            return false;

        if (!_isEditMode)
            return true;

        if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
        {
            return _model.SelectedShipToAddresses.Any();
        }

        return true;
    }

    private async Task OnCustomerChanged()
    {
        Console.WriteLine("Customer changed");
        _customerLocations.Clear();
        _model.SelectedShipToAddresses.Clear();

        if (_model.Customer != null && _model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
        {
            await LoadCustomerLocations();
        }

        StateHasChanged();
    }

    public async Task InitializeForEdit(int excludedCustomerId)
    {
        Console.WriteLine($"Initializing for edit: {excludedCustomerId}");
        _isEditMode = true;

        try
        {
            var query = new GetExcludedCustomerForEditQuery(excludedCustomerId);
            var result = await _mediator.Send(query);

            if (result.Succeeded)
            {
                _model = result.Data;

                // Ensure list is never null
                _model.SelectedShipToAddresses ??= new List<int>();

                Console.WriteLine($"Loaded model with exclusion type: {_model.ExclusionType}");
                Console.WriteLine($"Selected addresses from model: {string.Join(", ", _model.SelectedShipToAddresses)}");

                StateHasChanged();

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
            Console.WriteLine($"Error initializing for edit: {ex.Message}");
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
                if (_isEditMode && _model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations && !_model.SelectedShipToAddresses.Any())
                {
                    Snackbar.Add("Please select at least one location to exclude", Severity.Warning);
                }
                return;
            }

            _model.Country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LV;

            if (!_isEditMode)
            {
                _model.ExclusionType = ExclusionLevel.EntireLegalEntity;
                _model.SelectedShipToAddresses.Clear();
            }
            else if (_model.ExclusionType == ExclusionLevel.OneOrMorePhysicalLocations)
            {
                // Ensure we're using the current UI state
                _model.SelectedShipToAddresses = _customerLocations
                    .Where(x => x.IsSelected)
                    .Select(x => x.E1ShipTo)
                    .ToList();

                Console.WriteLine($"Submitting with selected addresses: {string.Join(", ", _model.SelectedShipToAddresses)}");
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
            Console.WriteLine($"Error during submit: {ex.Message}");
            Snackbar.Add("An error occurred while saving", Severity.Error);
        }
        finally
        {
            _saving = false;
        }
    }

    private void Cancel() => _mudDialog.Cancel();
}