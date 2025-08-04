using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Components.Security;

public partial class Permission
{
    [Parameter]
    public string RequiredPermission { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set;}
    [Inject]
    private NavigationManager _navigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var hasPermission = _userProfileState.Value.UserProfile.Permissions.Contains(RequiredPermission);

        if (!hasPermission)
        {
            _navigationManager.NavigateTo("/no-access");
        }
    }
}
