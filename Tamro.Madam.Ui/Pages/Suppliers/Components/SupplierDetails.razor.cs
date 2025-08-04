using BlazorDownloadFile;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Application.Commands.Suppliers;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Utils;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Ui.ComponentExtensions;
using Tamro.Madam.Ui.Store.Actions.Suppliers;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.Suppliers;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Suppliers.Components;

public partial class SupplierDetails
{
    [Inject]
    private UserSettingsUtils _userSettings { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IExcelService _excelService { get; set; }
    [Inject]
    private IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IState<SupplierState> _state { get; set; }

    private bool _saving;
    private MudForm? _form;
    private int _defaultPageSize;
    private HashSet<SupplierContractModel> _selectedSupplierContracts = new();
    private MudDataGrid<SupplierContractModel> _supplierContractGrid = new();

    protected override async Task OnInitializedAsync()
    {
        _defaultPageSize = _userSettings.GetRowsPerPageSetting();
    }

    private async Task OnExportContracts()
    {
        var mappers = new Dictionary<string, Func<SupplierContractModel, object?>>
        {
            { DisplayNameHelper.Get(typeof(SupplierContractModel), nameof(SupplierContractModel.AgreementDate)), i => i.AgreementDate },
            { DisplayNameHelper.Get(typeof(SupplierContractModel), nameof(SupplierContractModel.AgreementValidFrom)), i => i.AgreementValidFrom },
            { DisplayNameHelper.Get(typeof(SupplierContractModel), nameof(SupplierContractModel.AgreementValidTo)), i => i.AgreementValidTo },
            { DisplayNameHelper.Get(typeof(SupplierContractModel), nameof(SupplierContractModel.PaymentTermInDays)), i => i.PaymentTermInDays },
        };

        var fileContent = await _excelService.ExportAsync(_state.Value.Model.Contracts, mappers, $"supplier_contracts");
        await _blazorDownloadFileService.DownloadFile($"Supplier_{_state.Value.Model.Name}_contracts.xlsx", fileContent, "application/octet-stream");
        Snackbar.Add("Supplier contracts exported successfully", Severity.Success);
    }

    private async Task OnResetContractsGrid()
    {
        await MudDataGridExtensions<SupplierContractModel>.ResetGridState(_supplierContractGrid);
    }

    private async Task OnNewContract()
    {
        await ShowContractDialog("New Supplier Contract", new SupplierContractModel(), true);
    }

    private async Task OnOpenSingleContract(SupplierContractModel supplierContract)
    {
        await ShowContractDialog("Supplier Contract Details", supplierContract, false);
    }

    private async Task ShowContractDialog(string title, SupplierContractModel contract, bool isNew)
    {
        var parameters = new DialogParameters<SupplierContractDetails>
        {
            { x => x.SupplierContract, contract },
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            Position = DialogPosition.TopCenter,
        };
        var dialog = DialogService.Show<SupplierContractDetails>(title, parameters, options);
        var result = await dialog.Result;

        if ((result?.Canceled == false) && result.Data is SupplierContractModel updatedContract)
        {
            if (isNew)
            {
                _state.Value.Model.Contracts.Add(updatedContract);
            }
            else
            {
                var index = _state.Value.Model.Contracts.FindIndex(x => x.Guid == updatedContract.Guid);
                if (index != -1)
                {
                    _state.Value.Model.Contracts[index] = updatedContract;
                }
            }
        }
    }

    private async Task OnDeleteSingleContract(SupplierContractModel supplierContract)
    {
        await OnDelete([supplierContract]);
    }

    private async Task OnDeleteContracts()
    {
        await OnDelete(_selectedSupplierContracts);
    }

    private async Task OnDelete(HashSet<SupplierContractModel> supplierContracts)
    {
        foreach (var contract in supplierContracts)
        {
            _state.Value.Model.Contracts.Remove(contract);
        }
    }

    private async Task Submit()
    {
        try
        {
            _saving = true; 
            _state.Value.Model.Country = _userProfileState.Value.UserProfile.Country ?? default;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(new UpsertSupplierCommand(_state.Value.Model));

            if (result.Succeeded)
            {
                Snackbar.Add("Supplier " + (_state.Value.Model.Id == default ? "created" : "updated"), Severity.Success);
                _dispatcher.Dispatch(new RefreshGridAction());
                OnClose();
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

    private void OnClose()
    {
        _dispatcher.Dispatch(new SetActiveFormAction(SupplierForm.Grid));
    }

    private async Task OnContractDownload(string documentReference)
    {
        try
        {
            var cmd = new DownloadContractFileCommand(documentReference);
            var result = await _mediator.Send(cmd);

            if (result.Succeeded)
            {
                string fileName = Path.GetFileName(documentReference);
                await _blazorDownloadFileService.DownloadFile(fileName, result.Data, "application/octet-stream");
            }
            else
            {
                Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }
        catch
        {
            Snackbar.Add("Failed to download contract", Severity.Error);
        }
    }
}
