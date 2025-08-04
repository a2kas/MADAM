using Fluxor;
using Tamro.Madam.Common.Constants;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Utils;

public class UserSettingsUtils
{
    private IState<UserProfileState> _userProfileState;

    public UserSettingsUtils(IState<UserProfileState> userProfileState)
    {
        _userProfileState = userProfileState;
    }

    public int GetRowsPerPageSetting()
    {
        return _userProfileState.Value.UserProfile?.Settings?.Usability.RowsPerPage ?? UserSettingsConstants.DefaultRowsPerPage;
    }

    public bool HasPermission(string permission)
    {
        return _userProfileState.Value.HasPermission(permission);
    }
}
