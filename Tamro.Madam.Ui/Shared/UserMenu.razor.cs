using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Ui.Components.Dialogs;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Shared;

public partial class UserMenu
{
    [Parameter]
    public EventCallback<MouseEventArgs> OnSettingClick { get; set; }

    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private AuthenticationStateProvider _authenticationStateProvider { get; set; }

    private bool IsLoading => _userProfileState.Value.IsLoading;
    private UserProfileStateModel UserProfile => _userProfileState.Value.UserProfile;

    private async Task OnLogout()
    {
        var parameters = new DialogParameters<LogoutConfirmation>
        {
            { x=> x.ContentText, "Are you sure you want to log out"},
            { x=> x.Color,  Color.Error}
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall, FullWidth = true };
        var dialog = DialogService.Show<LogoutConfirmation>("Log out", parameters, options);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            _navigationManager.NavigateTo("/logout", true);
        }
    }

    private static string GetInitials(string str)
    {
        string[] words = str.Split(' ');
        string initials = "";

        foreach (var word in words)
        {
            if (!string.IsNullOrEmpty(word))
            {
                initials += word[0];
            }
        }

        return initials.ToUpper();
    }
}
