using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AdjustHideItemView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[v_hide_item_Benu] AS
                SELECT
	                ssc.Id,
                    ssi.ItemNo AS ItemNo2,
                    ssc.RowVer AS DStamp,
                    CASE WHEN ssc.SafetyStockPharmacyChainId IS NOT NULL THEN sspc.DisplayName ELSE ssc.SafetyStockPharmacyChainGroup END AS CompType,
                    ssc.[User] AS WindowsUser,
                    ssc.CanBuy AS Deleted,
                    ISNULL(REPLACE(ssc.Comment, NCHAR(10), ''),'') as Comment
                FROM 
                    MADAM.dbo.SafetyStockCondition ssc
                    LEFT JOIN MADAM.dbo.SafetyStockItem ssi ON ssi.Id = ssc.SafetyStockItemId
                    LEFT JOIN MADAM.dbo.SafetyStockPharmacyChain sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
                WHERE ssi.Country = 'LT' AND (ssc.SafetyStockPharmacyChainGroup = 'Benu' OR sspc.[Group] = 'Benu')");

            migrationBuilder.Sql("DROP VIEW [dbo].[v_hide_item]");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[v_hide_item] AS
                SELECT 
                    ssc.Id,
                    ssi.ItemNo AS ItemNo2,
                    ssc.RowVer AS DStamp,
                    COALESCE(sspc.DisplayName, ssc.SafetyStockPharmacyChainGroup) AS CompType,
                    ssc.[User] AS WindowsUser,
                    CASE 
                        WHEN ssc.SafetyStockPharmacyChainGroup IS NULL THEN 
                            CASE 
                                WHEN ISNULL(ss.WholesaleQuantity, 0) <= ISNULL(ss.RetailQuantity, 0) AND sspc.[Group] <> 'Benu' THEN 0 
                                ELSE ssc.CanBuy 
                            END
                        WHEN ssc.SafetyStockPharmacyChainGroup <> 'All' AND ssc.SafetyStockPharmacyChainId IS NULL THEN 
                            CASE 
                                WHEN ISNULL(ss.WholesaleQuantity, 0) <= ISNULL(ss.RetailQuantity, 0) AND sspc.[Group] <> 'Benu' THEN 0 
                                ELSE ssc.CanBuy 
                            END
                        WHEN ssc.SafetyStockPharmacyChainGroup = 'All' AND ssc.CanBuy = 1 AND ISNULL(ss.WholesaleQuantity, 0) <= ISNULL(ss.RetailQuantity, 0) THEN 0
                        ELSE ssc.CanBuy
                    END AS Deleted,
                    ISNULL(REPLACE(ssc.Comment, NCHAR(10), ''), '') AS Comment
                FROM MADAM.dbo.SafetyStockCondition AS ssc
                LEFT JOIN MADAM.dbo.SafetyStock AS ss ON ss.SafetyStockItemId = ssc.SafetyStockItemId
                LEFT JOIN MADAM.dbo.SafetyStockItem AS ssi ON ssi.Id = ssc.SafetyStockItemId
                LEFT JOIN MADAM.dbo.SafetyStockPharmacyChain AS sspc 
                    ON sspc.Id = ssc.SafetyStockPharmacyChainId
                WHERE 
                    ssi.Country = 'LT'
                    AND (
                        ssc.SafetyStockPharmacyChainGroup IS NULL 
                        OR 
                        (ssc.SafetyStockPharmacyChainGroup <> 'All' AND ssc.SafetyStockPharmacyChainId IS NULL)
                        OR 
                        (ssc.SafetyStockPharmacyChainGroup = 'All' AND ssc.CanBuy = 1 AND ISNULL(ss.WholesaleQuantity, 0) <= ISNULL(ss.RetailQuantity, 0))
                    );");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW [dbo].[v_hide_item]");

            migrationBuilder.Sql("DROP VIEW [dbo].[v_hide_item_Benu]");

            migrationBuilder.Sql(@"CREATE VIEW [dbo].[v_hide_item_Benu] AS
                SELECT
	                ssc.Id,
                    ssi.ItemNo AS ItemNo2,
                    ssc.RowVer AS DStamp,
                    CASE WHEN ssc.SafetyStockPharmacyChainId IS NOT NULL THEN sspc.DisplayName ELSE ssc.SafetyStockPharmacyChainGroup END AS CompType,
                    ssc.[User] AS WindowsUser,
                    ssc.CanBuy AS Deleted,
                    ISNULL(REPLACE(ssc.Comment, NCHAR(10), ''),'') as Comment
                FROM 
                    MADAM.dbo.SafetyStockCondition ssc
                    LEFT JOIN MADAM.dbo.SafetyStockItem ssi ON ssi.Id = ssc.SafetyStockItemId
                    LEFT JOIN MADAM.dbo.SafetyStockPharmacyChain sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
                WHERE (ssi.Country = 'LT') AND (ssc.SafetyStockPharmacyChainGroup <> 'All' OR ssc.SafetyStockPharmacyChainGroup IS NULL)");
        }
    }
}
