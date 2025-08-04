using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Components.Overview.Cards;

public partial class AttentionNeededCard
{
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
}
