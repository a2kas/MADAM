using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tamro.Madam.Application.Services.Authentication;
using Tamro.Madam.Models.General;
using Tamro.Madam.Ui.Services.Storage;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Shared;

public partial class MainLayout
{
    private bool _drawerOpen = true;
    private BalticCountry? _selectedCountry = null;
    private List<BalticCountry> _countries = [];

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private ILocalStorageService _localStorageService { get; set; }
    [Inject]
    private NavigationManager _navigationManager { get; set; }
    [Inject]
    private IPermissionService _permissionService { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnSettingClick { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _selectedCountry = _userProfileState.Value.UserProfile.Country;

        _countries = _permissionService.GetAvailableCountries(_userProfileState.Value.UserProfile.Permissions);
    }

    private void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private async Task CountryChanged(BalticCountry? country)
    {
        _selectedCountry = country;
        await _localStorageService.SetCountry(_selectedCountry.ToString());

        _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
    }
}