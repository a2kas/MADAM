using Fluxor;
using Tamro.Madam.Ui.Store.Actions;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Store.Reducers;

public static class UserProfileReducers
{
    [ReducerMethod]
    public static UserProfileState ReduceUserProfileUpdateAction(UserProfileState state, UpdateUserProfileAction action)
    {
        return new UserProfileState(false, action.UserProfile);
    }
}
