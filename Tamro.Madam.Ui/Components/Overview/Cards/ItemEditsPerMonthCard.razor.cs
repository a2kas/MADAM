using ApexCharts;
using MediatR;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Models.Overview.Cards;

namespace Tamro.Madam.Ui.Components.Overview.Cards;

public partial class ItemEditsPerMonthCard
{
    public ItemEditsPerMonthCardModel Model { get; set; } = new();
    private ApexChartOptions<ItemMonthlyEditCountModel> _options;

    [Inject]
    private IMediator _mediator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _options = new ApexChartOptions<ItemMonthlyEditCountModel>
        {
            Chart = new Chart
            {
                Toolbar = new Toolbar
                {
                    Tools = new Tools()
                    {
                        Download = true,
                        Zoom = false,
                        Zoomin = false,
                        Zoomout = false,
                        Pan = false,
                        Reset = false,                       
                    }
                }
            },
        };
        Model.MonthlyEdits = await _mediator.Send(new ItemMonthlyEditCountQuery());
    }
}
