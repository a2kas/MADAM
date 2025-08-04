using Fluxor;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Ui.Store.State;

[FeatureState]
public class UserProfileState
{
    public UserProfileState()
    {
        IsLoading = true;
        UserProfile = new UserProfileStateModel()
        {
            DisplayName = "",
            Permissions = [],
        };
    }

    public UserProfileState(bool isLoading, UserProfileStateModel userProfile)
    {
        IsLoading = isLoading;
        UserProfile = userProfile;
    }

    public bool HasPermission(string permission)
    {
        return UserProfile.Permissions.Contains(permission);
    }

    public UserProfileStateModel UserProfile { get; }
    public bool IsLoading { get; }
}
