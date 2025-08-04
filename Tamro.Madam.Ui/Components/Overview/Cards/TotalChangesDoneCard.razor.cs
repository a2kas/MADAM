using ApexCharts;
using MediatR;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Application.Queries.Audit;
using Tamro.Madam.Models.Overview;

namespace Tamro.Madam.Ui.Components.Overview.Cards;

public partial class TotalChangesDoneCard
{
    private IEnumerable<AuditEntriesByEntityCountModel> _entries;
    private ApexChartOptions<AuditEntriesByEntityCountModel> _options;

    [Inject]
    private IMediator _mediator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _options = new ApexChartOptions<AuditEntriesByEntityCountModel>
        {
            PlotOptions = new PlotOptions()
            {
                Bar = new PlotOptionsBar()
                {
                    Horizontal = true,
                },
            },
        };
        _entries = await _mediator.Send(new AuditEntriesByEntityCountQuery());
    }
}
