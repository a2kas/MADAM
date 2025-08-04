using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;

namespace Tamro.Madam.Application.Commands.Sales.CanceledOrderLines.Statistics;
public class GetCanceledLinesStatisticsCommand : IRequest<Result<IEnumerable<CanceledLineStatisticModel>>>, IDefaultErrorMessage
{
    public GetCanceledLinesStatisticsCommand(BalticCountry country, DateTime dateFrom, DateTime dateTill)
    {
        Country = country;
        DateFrom = dateFrom;
        DateTill = dateTill;
    }

    public BalticCountry Country { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTill { get; set; }

    public string ErrorMessage { get; set; } = "Failed to retrieve canceled line statistics";
}
