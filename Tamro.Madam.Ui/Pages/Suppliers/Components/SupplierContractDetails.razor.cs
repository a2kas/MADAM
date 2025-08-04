using AutoMapper;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.Suppliers;

namespace Tamro.Madam.Ui.Pages.Suppliers.Components;

public partial class SupplierContractDetails
{
    private SupplierContractModel _initialSupplierContract;

    [CascadingParameter] IMudDialogInstance MudDialog { get; set; }
    [Parameter]
    public SupplierContractModel SupplierContract { get; set; }

    [Inject]
    private IMapper _mapper { get; set; }
    [Inject]
    private IState<SupplierState> _state { get; set; }
    [Inject]
    private IValidationService _validator { get; set; }

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    [Inject]
    private IMediator _mediator { get; set; }

    private IBrowserFile _selectedFile;
    private MudForm? _form;
    private bool _isExtracting;

    private async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Save();
        }
    }

    protected override void OnInitialized()
    {
        _initialSupplierContract = _mapper.Map<SupplierContractModel>(SupplierContract);
    }

    private void OnReset()
    {
        SupplierContract = _mapper.Map<SupplierContractModel>(_initialSupplierContract);
        _selectedFile = null;
        StateHasChanged();
    }

    private async Task OnFileSelected(InputFileChangeEventArgs e)
    {
        _selectedFile = e.File;
        await UploadFile();
    }

    private async Task OnAiGenerate(InputFileChangeEventArgs e)
    {
        _isExtracting = true;
        _selectedFile = e.File;
        var uploadTask = UploadFile();
        var extractTask = AiExtractFile();

        await Task.WhenAll(uploadTask, extractTask);
        _isExtracting = false;
    }

    private async Task AiExtractFile()
    {
        await using var stream = _selectedFile.OpenReadStream();
        var result = await _mediator.Send(new ExtractContractFileCommand(SupplierContract, stream, _selectedFile.Name));

        if (result.Succeeded)
        {
            SupplierContract = result.Data.Contract;
            if (string.IsNullOrEmpty(_state.Value.Model.RegistrationNumber))
            {
                _state.Value.Model.RegistrationNumber = result.Data.RegistrationNumber;
            }
            if (string.IsNullOrEmpty(_state.Value.Model.Name))
            {
                _state.Value.Model.Name = result.Data.SupplierName;
            }
            Snackbar.Add("Contract extracted succesfully", Severity.Success);
        }
        else
        {
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task UploadFile()
    {
        if (_selectedFile == null)
        {
            return;
        }

        await using var fileStream = _selectedFile.OpenReadStream();

        var country = _userProfileState.Value.UserProfile.Country ?? BalticCountry.LT;
        var registrationNumber = _state.Value.Model.RegistrationNumber;
        var fileName = _selectedFile.Name;
        var fileExtension = Path.GetExtension(fileName).TrimStart('.');

        var result = await _mediator.Send(new UploadContractFileCommand(
            country,
            registrationNumber,
            fileStream,
            fileName,
            fileExtension
        ));

        if (result.Succeeded)
        {
            SupplierContract.DocumentReference = result.Data;
        }
        else
        {
            SupplierContract.DocumentReference = null;
            Snackbar.Add(result.ErrorMessage, Severity.Error);
        }
    }

    private async Task Save()
    {
        await _form!.Validate();

        if (!_form.IsValid)
        {
            return;
        }

        MudDialog.Close(SupplierContract);
    }

    private async Task ValidateForm()
    {
        await _form!.Validate();
    }

    private void Close() => MudDialog.Close();
}