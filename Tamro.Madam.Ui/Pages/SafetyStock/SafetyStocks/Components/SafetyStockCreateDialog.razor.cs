using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Application.Queries.Items.Wholesale.Clsf;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.SafetyStock.SafetyStocks.Components;

public partial class SafetyStockCreateDialog
{
    [EditorRequired]
    [Parameter]
    public SafetyStockItemUpsertFormModel Model { get; set; }
    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [CascadingParameter]
    private IMudDialogInstance _mudDialog { get; set; } = default!;

    private bool _isItemInfoLoading;
    private bool _isItemInfoPopoverOpen;
    private bool _isSaving;
    private MudForm? _form;
    private MudSelect<SafetyStockRestrictionLevel>? _restrictionLevel;
    private List<PharmacyGroup> _pharmacyGroups = Enum.GetValues(typeof(PharmacyGroup)).Cast<PharmacyGroup>().Where(x => x != PharmacyGroup.All).ToList();
    private List<PharmacyChainModel> _pharmacyChains = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadPharmacyChains();
        Model.User = _userProfileState.Value.UserProfile;
    }

    private async Task Submit()
    {
        try
        {
            _isSaving = true;
            Model.IsSaveAttempted = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new UpsertSafetyStockItemCommand(Model));

            if (result.Succeeded)
            {
                _mudDialog.Close(DialogResult.Ok(true));
                Snackbar.Add($"Safety stock condition record for itemNo '{Model.Item.ItemNo}' created", Severity.Success);
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        finally
        {
            _isSaving = false;
        }
    }

    private async Task LoadPharmacyChains()
    {
        var result = await _mediator.Send(new GetSafetyStockPharmacyChainsCommand(Model.Country, isActive: true));

        if (result.Succeeded)
        {
            _pharmacyChains = [.. result.Data.OrderBy(x => x.DisplayName)];
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task<IEnumerable<WholesaleItemClsfModel>> SearchItems(string value, CancellationToken cancellationToken)
    {
        try
        {
            var query = new WholesaleItemClsfQuery()
            {
                SearchTerm = value,
                Country = Model.Country,
            };
            var result = await _mediator.Send(query, cancellationToken);
            
            return result.Items;
        }
        catch (Exception)
        {
            Snackbar.Add("Failed to load wholesale items", Severity.Error);
            return [];
        }
    }

    private async void OnItemChanged(WholesaleItemClsfModel wholesaleItem)
    {
        Model.Item = wholesaleItem;
        Model.ItemInfo = new();

        if (wholesaleItem == null)
        {
            return;
        }
        
        _isItemInfoLoading = true;
        var cmd = new GetSafetyStockItemInfoCommand(wholesaleItem.ItemNo, Model.Country);
        var result = await _mediator.Send(cmd);

        if (result.Succeeded)
        {
            Model.ItemInfo = result.Data ?? new();
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }

        _isItemInfoLoading = false;
    }

    private bool IsCheckedPharmacyGroup(PharmacyGroup group)
    {
        return Model.PharmacyGroups.Contains(group);
    }

    private void OnPharmacyGroupCheckChanged(bool isChecked, PharmacyGroup group)
    {
        if (isChecked)
        {
            if (!Model.PharmacyGroups.Contains(group))
            {
                Model.PharmacyGroups.Add(group);
            }
        }
        else
        {
            if (Model.PharmacyGroups.Contains(group))
            {
                Model.PharmacyGroups.Remove(group);
            }
        }
        _restrictionLevel.Validate();
    }

    private bool IsCheckedPharmacyChain(PharmacyChainModel pharmacyChain) => Model.PharmacyChains.Exists(x => x.Id == pharmacyChain.Id);

    private void OnPharmacyChainCheckChanged(bool isChecked, PharmacyChainModel pharmacyChain)
    {
        if (isChecked)
        {
            if (!Model.PharmacyChains.Exists(x => x.Id == pharmacyChain.Id))
            {
                Model.PharmacyChains.Add(pharmacyChain);
            }
        }
        else
        {
            var removablePharmacyChain = Model.PharmacyChains.FirstOrDefault(x => x.Id == pharmacyChain.Id);
            if (removablePharmacyChain != null)
            {
                Model.PharmacyChains.Remove(removablePharmacyChain);
            }
        }
        _restrictionLevel.Validate();
    }

    private void ToggleItemInfoPopover()
    {
        _isItemInfoPopoverOpen = !_isItemInfoPopoverOpen;
    }

    private void Cancel() => _mudDialog.Cancel();
}
