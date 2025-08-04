using AutoMapper;
using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Application.Commands.User.UserSettings;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Models.State.General.Settings;
using Tamro.Madam.Models.User.UserSettings;
using Tamro.Madam.Ui.Store.Actions;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Pages.Settings;

public partial class Settings
{
    [EditorRequired]
    [Parameter]
    public UserSettingsModel Model { get; set; }

    [Inject]
    private IValidationService _validator { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private UserSettingsUtils _userSettingsUtils { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }
    [Inject]
    private IDispatcher _dispatcher { get; set; }
    [Inject]
    private IMapper _mapper { get; set; }

    private bool _saving;
    private MudForm? _form;

    private List<Item> _items;
    private bool showWarning;
    private string warningMessage;

    protected override void OnInitialized()
    {
        Model = new UserSettingsModel
        {
            Usability = new UserUsabilitySettingsModel()
        };
        Model.Usability.RowsPerPage = _userSettingsUtils.GetRowsPerPageSetting();

        _items = new List<Item>
        {
            new Item { Description = "Set default rows per page amount", Options = new List<SelectOption>
                {
                    new SelectOption { Value = 10, Text = "10" },
                    new SelectOption { Value = 15, Text = "15" },
                    new SelectOption { Value = 30, Text = "30" },
                    new SelectOption { Value = 50, Text = "50" },
                    new SelectOption { Value = 100, Text = "100" },
                    new SelectOption { Value = 200, Text = "200" },
                },
            },
        };
    }

    private void OnSelectRowsPerPage(int? value)
    {
        Model.Usability.RowsPerPage = value;

        if (value == 100 || value == 200)
        {
            showWarning = true;
            warningMessage = $"Setting page size to {value} may slow down the system, especially on poor network quality.";
        }
        else
        {
            showWarning = false;
            warningMessage = "";
        }
    }

    private class Item
    {
        public string Description { get; set; }
        public List<SelectOption> Options { get; set; }
    }

    private class SelectOption
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }

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

            var result = await _mediator.Send(new UpsertUserSettingsCommand(Model));

            if (result.Succeeded)
            {
                Model = result.Data;
                Snackbar.Add("User settings successfully updated", Severity.Success);
                _dispatcher.Dispatch(
                 new UpdateUserProfileAction()
                 {
                     UserProfile = new UserProfileStateModel()
                     {
                         DisplayName = _userProfileState.Value.UserProfile.DisplayName,
                         Permissions = _userProfileState.Value.UserProfile.Permissions,
                         UserName = _userProfileState.Value.UserProfile.UserName,
                         Country = _userProfileState.Value.UserProfile.Country,
                         Settings = _mapper.Map<SettingsModel>(Model),
                     }
                 });
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
}
