using System.Security.Claims;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Services.Settings;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Models.State.General.Settings;
using Tamro.Madam.Repository.Audit;
using Tamro.Madam.Ui.Services.Storage;
using Tamro.Madam.Ui.Store.Actions;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Utils;

namespace Tamro.Madam.Ui.Services.Auth;

public class TamroAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IDispatcher _dispatcher;
    private readonly IState<UserProfileState> _userProfileState;
    private TaskCompletionSource<bool> _updateCompletionSource;
    private readonly ILocalStorageService _localStorageService;
    private readonly IUserContext _userContext;
    private readonly UserAccessor _userAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserSettingsService _userSettingsService;

    public TamroAuthenticationStateProvider(
        IDispatcher dispatcher,
        IState<UserProfileState> userProfileState,
        ILocalStorageService localStorageService,
        IUserContext userContext,
        UserAccessor userAccessor,
        IHttpContextAccessor httpContextAccessor,
        IUserSettingsService userSettingsService)
    {
        _dispatcher = dispatcher;
        _userProfileState = userProfileState;
        _localStorageService = localStorageService;
        _userContext = userContext;
        _userAccessor = userAccessor;
        _userSettingsService = userSettingsService;
        _httpContextAccessor = httpContextAccessor;
        _userProfileState.StateChanged += UserProfileStateChanged;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        _updateCompletionSource = new TaskCompletionSource<bool>();
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            var permissions = user.Claims.GetPermissions();
            var displayName = user.Identity.Name!;
            var userName = user.Claims.GetUserName();
            _userAccessor.SetUsername(displayName);
            _userContext.Permissions = permissions;
            _userContext.DisplayName = displayName;
            _userContext.UserName = userName;

            BalticCountry? selectedCountry = await _localStorageService.ResolveCountry(permissions);

            var userSettingsResult = await _userSettingsService.ResolveUserSettings(userName);
            var userSettings = userSettingsResult.Data;
            _dispatcher.Dispatch(
                 new UpdateUserProfileAction()
                 {
                     UserProfile = new UserProfileStateModel()
                     {
                         DisplayName = displayName,
                         Permissions = permissions,
                         UserName = userName,
                         Country = selectedCountry,
                         Settings =
                         new SettingsModel()
                         {
                             Usability = new UsabilitySettingsModel
                             {
                                 RowsPerPage = userSettings.Usability.RowsPerPage,
                             },
                         }
                     }
                 });

            await _updateCompletionSource.Task;

            return new AuthenticationState(user);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    private void UserProfileStateChanged(object sender, EventArgs e)
    {
        if (_updateCompletionSource != null && !_updateCompletionSource.Task.IsCompleted)
        {
            _updateCompletionSource.SetResult(true);
        }
    }
}