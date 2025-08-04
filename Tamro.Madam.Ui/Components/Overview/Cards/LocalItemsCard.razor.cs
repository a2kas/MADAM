using MediatR;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Application.Queries.Items.Bindings;
using Tamro.Madam.Models.Overview.Cards;

namespace Tamro.Madam.Ui.Components.Overview.Cards;

public partial class LocalItemsCard
{
    public LocalItemsCardModel Model { get; set; } = new();
    [Inject]
    private IMediator _mediator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model.CountByCompany = await _mediator.Send(new ItemBindingCountByCompanyQuery());
    }
}
