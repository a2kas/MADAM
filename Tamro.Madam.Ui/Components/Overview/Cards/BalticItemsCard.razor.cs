using Fluxor;
using MediatR;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Application.Queries.Items;
using Tamro.Madam.Models.Overview.Cards;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Components.Overview.Cards;

public partial class BalticItemsCard
{
    public BalticItemsCardModel Model { get; set; } = new();
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }
    [Inject]
    private IMediator _mediator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var balticItemCount = await _mediator.Send(new ItemCountQuery());
        Model.Count = balticItemCount;
    }
}
