using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Models.Sales.CanceledOrderLines.Statistics;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Repository.Repositories.Sales;
public class CanceledLineStatisticRepository : ICanceledLineStatisticRepository
{
    private readonly IE1GatewayDbContext _context;

    private readonly IMapper _mapper;

    public CanceledLineStatisticRepository(IE1GatewayDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CanceledLineStatisticModel>> GetStatistics(BalticCountry country, DateTime dateFrom, DateTime dateTill)
    {
        var parameters = new[]
        {
            new SqlParameter("@Country", country.ToString()),
            new SqlParameter("@DateFrom", dateFrom),
            new SqlParameter("@DateTill", dateTill)
        };

        var sqlQuery = BuildStatisticsQuery();

        var result = await _context.CanceledLineStatistics.FromSqlRaw(sqlQuery, parameters).ToListAsync();

        return _mapper.Map<IEnumerable<CanceledLineStatisticModel>>(result);
    }

    private static string BuildStatisticsQuery()
    {
        var headerId = nameof(E1CanceledOrderHeader.Id);
        var e1ShipTo = nameof(E1CanceledOrderHeader.E1ShipTo);
        var country = nameof(E1CanceledOrderHeader.Country);
        var canceledQuantity = nameof(E1CanceledOrderLine.CanceledQuantity);
        var createdDate = nameof(E1CanceledOrderLine.CreatedDate);
        var status = nameof(E1CanceledOrderLine.EmailStatus);
        var itemNo2 = nameof(E1CanceledOrderLine.ItemNo2);
        var lineHeaderId = nameof(E1CanceledOrderLine.E1CanceledOrderHeaderId);
        var cancelationReason = nameof(E1CanceledOrderLine.CancelationReason);

        return $@"
        SELECT 
            e.{e1ShipTo}, 
            t.{canceledQuantity}, 
            t.{createdDate}, 
            t.{itemNo2},
            t.{cancelationReason}  
        FROM E1CanceledOrderLines t
        LEFT JOIN E1CanceledOrderHeaders e ON e.{headerId} = t.{lineHeaderId}
        WHERE e.{country} = @Country 
        AND t.{createdDate} BETWEEN @DateFrom AND @DateTill
        And t.{status} = '{CanceledOrderLineEmailStatus.Sent}'";
    }
}
