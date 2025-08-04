using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Updatev_SafetyStockGrid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_SafetyStockGrid]'))
                        DROP VIEW [dbo].[v_SafetyStockGrid];");

            migrationBuilder.Sql(@"CREATE VIEW v_SafetyStockGrid AS
			            SELECT
			            ssc.Id,
			            ssi.Country,
			            ssc.CanBuy,
			            ssc.SafetyStockPharmacyChainGroup,
			            sspc.DisplayName as PharmacyChainDisplayName,
                        sspc.Id as PharmacyChainId,
			            ssi.ItemNo, 
			            i.ItemDescription1 as ItemName,
			            ssc.CheckDays,
			            ss.WholesaleQuantity,
			            ss.RetailQuantity,
			            CASE WHEN (sspc.[Group] = 'Benu' OR sspc.Value = 'Benu') THEN ss.WholesaleQuantity ELSE 
				            CASE WHEN (ss.WholesaleQuantity-ss.RetailQuantity) < 0 THEN 0
				            ELSE CAST(ss.WholesaleQuantity-ROUND(ss.RetailQuantity,+1) AS INT) END 
				            END as QuantityToBuy,
			            i.ItemGroup, 
			            i.ProductClass,
			            ISNULL(RTRIM(retail.Brand),'') as Brand,
			            i.SupplierNumber, 
			            i.PrincipalIdDescription AS SupplierNick,
			            t.cn3,
			            t.cn1,
			            CASE WHEN p.atcname = '-' THEN i.ActiveIngredient ELSE ISNULL(p.atcname,i.ActiveIngredient) COLLATE database_default END as Substance,
			            ssc.Comment
			            FROM [MADAM].[dbo].[SafetyStockCondition] ssc
				            LEFT JOIN [MADAM].[dbo].[SafetyStockItem] ssi ON ssi.Id = ssc.SafetyStockItemId
				            LEFT JOIN [MADAM].[dbo].[SafetyStockPharmacyChain] sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
				            LEFT JOIN [MADAM].[dbo].[SafetyStock] ss ON ss.SafetyStockItemId = ssc.SafetyStockItemId
				            LEFT JOIN [WH_RAW_LT].[dbo].[Items] i ON ssi.ItemNo = i.ItemNo2
				            LEFT JOIN [WH_OLAP].[dbo].[Item] ii ON i.ItemNo2 = ii.ItemNo2 AND ii.Company = '00701'
				            LEFT JOIN tools.dbo.BalticCategoryTree() t ON ii.[BaltCatId] = t.id
				            LEFT JOIN [RT_DATA].[dbo].[ItemInfo] retail ON retail.ItemNo2 = i.ItemNo2 AND retail.Company = '00705'
				            LEFT JOIN [LTKA1S015].[VAISTINES].[dbo].[Products] p ON p.tamro_item = ii.ItemNo2 COLLATE database_default
			            WHERE i.ProductClass NOT IN ('PVZ', 'REK', 'SAU', 'FI', 'CTR', 'GIF')
			            AND i.PrincipalIdDescription NOT IN ('EXPORT', 'EXPORT COSMETICS')
			            AND i.SalesRestriction NOT IN ('PVZ', 'FEE', 'NFS', 'WRH', 'CTR', 'LIG', 'NLI', 'NEX', 'END')
			            AND i.StockingType <> 'O'
			            AND i.ReportingCode1 <> 'N'
			            AND LEFT(i.ItemNo2, 1) not in ('8', '9')
			            AND t.cn3 in ('A. Rx', 'B. OTC')
			            GROUP BY
			            ssc.Id,
			            ssi.Country,
			            ssc.CanBuy,
			            ssc.SafetyStockPharmacyChainGroup,
			            sspc.DisplayName,
                        sspc.Id,
			            ssi.ItemNo,
			            ItemDescription1,
			            ssc.CheckDays,
			            ss.WholesaleQuantity,
			            ss.RetailQuantity,
			            CASE WHEN (sspc.[Group] = 'Benu' OR sspc.Value = 'Benu') THEN ss.WholesaleQuantity ELSE 
				            CASE WHEN (ss.WholesaleQuantity-ss.RetailQuantity) < 0 THEN 0
				            ELSE CAST(ss.WholesaleQuantity-ROUND(ss.RetailQuantity,+1) AS INT) END 
				            END,
			            i.ItemGroup, 
			            i.ProductClass,
			            ISNULL(RTRIM(retail.Brand),''),
			            i.SupplierNumber, 
			            i.PrincipalIdDescription,
			            t.cn3,
			            t.cn1,
			            CASE WHEN p.atcname = '-' THEN i.ActiveIngredient ELSE ISNULL(p.atcname,i.ActiveIngredient) COLLATE database_default END,
			            ssc.Comment;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_SafetyStockGrid]'))
                        DROP VIEW [dbo].[v_SafetyStockGrid];");

            migrationBuilder.Sql(@"CREATE VIEW v_SafetyStockGrid AS
			            SELECT
			            ssc.Id,
			            ssi.Country,
			            ssc.CanBuy,
			            ssc.SafetyStockPharmacyChainGroup,
			            sspc.DisplayName as PharmacyChain,
			            ssi.ItemNo, 
			            i.ItemDescription1 as ItemName,
			            ssc.CheckDays,
			            ss.WholesaleQuantity,
			            ss.RetailQuantity,
			            CASE WHEN (sspc.[Group] = 'Benu' OR sspc.Value = 'Benu') THEN ss.WholesaleQuantity ELSE 
				            CASE WHEN (ss.WholesaleQuantity-ss.RetailQuantity) < 0 THEN 0
				            ELSE CAST(ss.WholesaleQuantity-ROUND(ss.RetailQuantity,+1) AS INT) END 
				            END as QuantityToBuy,
			            i.ItemGroup, 
			            i.ProductClass,
			            ISNULL(RTRIM(retail.Brand),'') as Brand,
			            i.SupplierNumber, 
			            i.PrincipalIdDescription AS SupplierNick,
			            t.cn3,
			            t.cn1,
			            CASE WHEN p.atcname = '-' THEN i.ActiveIngredient ELSE ISNULL(p.atcname,i.ActiveIngredient) COLLATE database_default END as Substance,
			            ssc.Comment
			            FROM [MADAM].[dbo].[SafetyStockCondition] ssc
				            LEFT JOIN [MADAM].[dbo].[SafetyStockItem] ssi ON ssi.Id = ssc.SafetyStockItemId
				            LEFT JOIN [MADAM].[dbo].[SafetyStockPharmacyChain] sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
				            LEFT JOIN [MADAM].[dbo].[SafetyStock] ss ON ss.SafetyStockItemId = ssc.SafetyStockItemId
				            LEFT JOIN [WH_RAW_LT].[dbo].[Items] i ON ssi.ItemNo = i.ItemNo2
				            LEFT JOIN [WH_OLAP].[dbo].[Item] ii ON i.ItemNo2 = ii.ItemNo2 AND ii.Company = '00701'
				            LEFT JOIN tools.dbo.BalticCategoryTree() t ON ii.[BaltCatId] = t.id
				            LEFT JOIN [RT_DATA].[dbo].[ItemInfo] retail ON retail.ItemNo2 = i.ItemNo2 AND retail.Company = '00705'
				            LEFT JOIN [LTKA1S015].[VAISTINES].[dbo].[Products] p ON p.tamro_item = ii.ItemNo2 COLLATE database_default
			            WHERE i.ProductClass NOT IN ('PVZ', 'REK', 'SAU', 'FI', 'CTR', 'GIF')
			            AND i.PrincipalIdDescription NOT IN ('EXPORT', 'EXPORT COSMETICS')
			            AND i.SalesRestriction NOT IN ('PVZ', 'FEE', 'NFS', 'WRH', 'CTR', 'LIG', 'NLI', 'NEX', 'END')
			            AND i.StockingType <> 'O'
			            AND i.ReportingCode1 <> 'N'
			            AND LEFT(i.ItemNo2, 1) not in ('8', '9')
			            AND t.cn3 in ('A. Rx', 'B. OTC')
			            GROUP BY
			            ssc.Id,
			            ssi.Country,
			            ssc.CanBuy,
			            ssc.SafetyStockPharmacyChainGroup,
			            sspc.DisplayName,
			            ssi.ItemNo,
			            ItemDescription1,
			            ssc.CheckDays,
			            ss.WholesaleQuantity,
			            ss.RetailQuantity,
			            CASE WHEN (sspc.[Group] = 'Benu' OR sspc.Value = 'Benu') THEN ss.WholesaleQuantity ELSE 
				            CASE WHEN (ss.WholesaleQuantity-ss.RetailQuantity) < 0 THEN 0
				            ELSE CAST(ss.WholesaleQuantity-ROUND(ss.RetailQuantity,+1) AS INT) END 
				            END,
			            i.ItemGroup, 
			            i.ProductClass,
			            ISNULL(RTRIM(retail.Brand),''),
			            i.SupplierNumber, 
			            i.PrincipalIdDescription,
			            t.cn3,
			            t.cn1,
			            CASE WHEN p.atcname = '-' THEN i.ActiveIngredient ELSE ISNULL(p.atcname,i.ActiveIngredient) COLLATE database_default END,
			            ssc.Comment;");
        }
    }
}
