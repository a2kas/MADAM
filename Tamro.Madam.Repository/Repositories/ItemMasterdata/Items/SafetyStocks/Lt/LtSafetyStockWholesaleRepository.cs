using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks.Lt;

public class LtSafetyStockWholesaleRepository : ISafetyStockWholesaleRepository
{
    private readonly WhRawLtDatabaseContext _context;

    public LtSafetyStockWholesaleRepository(WhRawLtDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockItems()
    {
        return await _context.SafetyStockItems.FromSqlRaw($"{SafetyStockBaseQuery} {SafetyStockBaseSearchClause} {SafetyStockGroupByClause}").ToListAsync();
    }

    public async Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockExistingItems()
    {
        return await _context.SafetyStockItems.FromSqlRaw(SafetyStockExistingItemsQuery).ToListAsync();
    }

    public async Task<IEnumerable<WholesaleSafetyStockItem>> GetSafetyStockImportedItems(string[] importedItemNumbers)
    {
        string parameterizedQuery = $"{SafetyStockBaseQuery} WHERE i.ItemNo2 IN ({string.Join(",", importedItemNumbers.Select((_, index) => $"@p{index}"))}) {SafetyStockGroupByClause}";
        List<object> parameters = importedItemNumbers.Select(itemNumber => new SqlParameter("@p" + Array.IndexOf(importedItemNumbers, itemNumber), itemNumber)).Cast<object>().ToList();
        return await _context.SafetyStockItems.FromSqlRaw(parameterizedQuery, parameters.ToArray()).ToListAsync();
    }

    public async Task<WholesaleSafetyStockItemRetailQty> GetRetailQtyByItemNo(string itemNo, int checkDays)
    {
        string parameterizedQuery = $"{SafetyStockRtlQtyByItemNoQuery}";
        var pItemNo = new SqlParameter("@itemNo", itemNo);
        var pCheckDays = new SqlParameter("@checkDays", checkDays);
        return await _context.SafetyStockItemRetailQty.FromSqlRaw(parameterizedQuery, pItemNo, pCheckDays).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<WholesaleSafetyStockItemRetailQty>> GetRetailQty()
    {
        return await _context.SafetyStockItemRetailQty.FromSqlRaw(SafetyStockRtlQtyQuery).ToListAsync();
    }

    public async Task<WholesaleSafetyStockItem> GetSafetyStockItemByItemNo(string itemNo)
    {
        string parameterizedQuery = $"{SafetyStockBaseQuery} WHERE i.ItemNo2 = @itemNo {SafetyStockGroupByClause}";
        var sItemNo = new SqlParameter("@itemNo", itemNo);
        return await _context.SafetyStockItems.FromSqlRaw(parameterizedQuery, sItemNo).FirstOrDefaultAsync();
    }

    private static string SafetyStockBaseQuery => @$"SELECT
            i.ItemNo2 as ItemNo, i.ItemDescription1 as ItemName,
            i.ItemGroup, i.ProductClass,
            ISNULL(RTRIM(retail.Brand),'') as Brand,
            i.SupplierNumber as SupplierNumber,
            i.PrincipalIdDescription as SupplierNick,
            t.cn3 as Cn3,
            t.cn1 as Cn1,
            CASE WHEN rti.ATCDescription = '-' THEN i.ActiveIngredient ELSE ISNULL(rti.ATCDescription,i.ActiveIngredient) END as Substance
        FROM [WH_RAW_LT].[dbo].[Items] i
	        LEFT JOIN [WH_OLAP].[dbo].[Item] ii ON i.ItemNo2 = ii.ItemNo2 AND ii.Company = '00701'
	        LEFT JOIN tools.dbo.BalticCategoryTree() t ON ii.[BaltCatId] = t.id
	        LEFT JOIN [RT_DATA].[dbo].[ItemInfo] retail ON retail.ItemNo2 = i.ItemNo2 AND retail.Company = '00705'
	        LEFT JOIN [RT_OLAP].[dbo].[Item] rti ON rti.ItemCode2 = i.ItemNo2";

    private static string SafetyStockBaseSearchClause = @"WHERE i.ProductClass NOT IN ('PVZ', 'REK', 'SAU', 'FI', 'CTR', 'GIF')
            AND i.PrincipalIdDescription NOT IN ('EXPORT', 'EXPORT COSMETICS')
            AND i.SalesRestriction NOT IN ('PVZ', 'FEE', 'NFS', 'WRH', 'CTR', 'LIG', 'NLI', 'NEX', 'END')
            AND i.StockingType <> 'O'
            AND i.ReportingCode1 <> 'N'
            AND LEFT(i.ItemNo2, 1) not in ('8', '9')
            AND t.cn3 in ('A. Rx', 'B. OTC')";

    private static string SafetyStockGroupByClause = @"GROUP BY 
            i.ItemNo2, ItemDescription1,
            i.ItemGroup, i.ProductClass,
            ISNULL(RTRIM(retail.Brand),''),
            i.SupplierNumber, i.PrincipalIdDescription,
            t.cn3,
            t.cn1,
            CASE WHEN rti.ATCDescription = '-' THEN i.ActiveIngredient ELSE ISNULL(rti.ATCDescription,i.ActiveIngredient) END";

    private static string SafetyStockExistingItemsQuery => @$"SELECT ssi.ItemNo, i.ItemDescription1 as ItemName,
            i.ItemGroup, i.ProductClass, ISNULL(RTRIM(retail.Brand),'') as Brand, 
            i.SupplierNumber, i.PrincipalIdDescription AS SupplierNick,
            t.cn3, t.cn1,
            CASE WHEN rt_i.ATCDescription = '-' THEN i.ActiveIngredient ELSE ISNULL(rt_i.ATCDescription,i.ActiveIngredient) END as Substance
            FROM [MADAM].[dbo].[SafetyStockItem] ssi
	            LEFT JOIN [WH_RAW_LT].[dbo].[Items] i ON ssi.ItemNo = i.ItemNo2
	            LEFT JOIN [WH_RAW_LT].[dbo].[ItemAvailability] ia ON ia.ItemNo2 = i.ItemNo2
	            LEFT JOIN [WH_OLAP].[dbo].[Item] ii ON i.ItemNo2 = ii.ItemNo2 AND ii.Company = '00701'
	            LEFT JOIN tools.dbo.BalticCategoryTree() t ON ii.[BaltCatId] = t.id
	            LEFT JOIN [RT_DATA].[dbo].[ItemInfo] retail ON retail.ItemNo2 = i.ItemNo2 AND retail.Company = '00705'
	            LEFT JOIN [RT_OLAP].[dbo].[Item] rt_i ON rt_i.ItemCode2 = i.ItemNo2";

    private static string SafetyStockRtlQtyQuery => @"SELECT base.Id as SafetyStockItemId, base.ItemNo as ItemNo2,
	            SUM(trans.Qty) as RtlTransQty
            FROM
            (SELECT DISTINCT ssi.ItemNo, ssi.Id, ssi.CheckDays
            FROM [MADAM].[dbo].[SafetyStockItem] ssi
	            LEFT JOIN [MADAM].[dbo].[SafetyStockCondition] ssc ON ssi.Id = ssc.SafetyStockItemId
            GROUP BY ssi.ItemNo, ssi.Id, CheckDays
            ) base
            LEFT JOIN [RT_OLAP].[dbo].[Transactions] trans ON trans.ItemNo2 = base.ItemNo 
	            AND trans.TransactionDate BETWEEN DATEADD(d,-base.CheckDays,cast(GETDATE() as date)) AND DATEADD(d,-1,CAST(GETDATE() AS DATE)) 
	            AND trans.Company = '00705' AND TransactionType = 'POS' AND trans.PharmacyCode NOT IN ('1228','1238')
            GROUP BY base.Id, base.ItemNo";

    private static string SafetyStockRtlQtyByItemNoQuery => @"SELECT ItemNo2, SUM(Qty) as RtlTransQty, 0 as SafetyStockItemId
            FROM [RT_OLAP].[dbo].[Transactions] 
            WHERE TransactionDate between DATEADD(d,-@checkDays,cast(GETDATE() as date)) AND DATEADD(d,-1,CAST(GETDATE() AS DATE)) 
            AND Company = '00705' AND TransactionType = 'POS' AND PharmacyCode not in ('1228','1238')
            AND ItemNo2 = @itemNo
            Group BY ItemNo2";
}
