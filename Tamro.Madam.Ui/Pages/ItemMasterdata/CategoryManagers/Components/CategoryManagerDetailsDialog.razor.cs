using AutoMapper;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.Common.Dialog;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.CategoryManagers.Components;

public partial class CategoryManagerDetailsDialog
{
    [EditorRequired]
    [Parameter]
    public CategoryManagerModel Model { get; set; } = new();

    [EditorRequired]
    [Parameter]
    public DialogState State { get; set; }

    [Inject]
    protected IState<UserProfileState> _userProfileState { get; set; }

    [Inject]
    protected IValidationService _validator { get; set; }

    [Inject]
    protected IMediator _mediator { get; set; }

    [Inject]
    protected IMapper _mapper { get; set; }

    [CascadingParameter]
    protected IMudDialogInstance MudDialog { get; set; } = default!;

    protected bool _saving;
    protected MudForm? _form;

    protected async Task OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await Submit();
        }
    }

    protected async Task Submit()
    {
        try
        {
            _saving = true;
            await _form!.Validate();

            if (!_form.IsValid)
            {
                return;
            }

            var result = await _mediator.Send(
                _mapper.Map<UpsertCategoryManagerCommand>(Model));

            if (result.Succeeded)
            {
                MudDialog.Close(DialogResult.Ok(true));
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

    protected string GetTitle()
    {
        return State switch
        {
            DialogState.Create => "Create new category manager",
            DialogState.View => "Category manager",
            DialogState.Copy => "Copy category manager",
            _ => "Edit category manager",
        };
    }

    protected string GetSuccessMessage()
    {
        return State switch
        {
            DialogState.Create or DialogState.Copy => $"Category manager {Model.EmailAddress} created",
            DialogState.View => $"Category manager {Model.EmailAddress} updated",
            _ => "",
        };
    }

    protected void Cancel() => MudDialog.Cancel();
}