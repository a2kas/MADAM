using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class SafetyStockGridView_refactoring : Migration
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
            ssi.ItemName,
            ssc.CheckDays,
            ss.WholesaleQuantity,
            ss.RetailQuantity,
            CASE WHEN (ssc.SafetyStockPharmacyChainGroup = 'Benu' OR sspc.Value = 'Benu') THEN ISNULL(ss.WholesaleQuantity,0) ELSE 
                CASE WHEN (ISNULL(ss.WholesaleQuantity,0)-ISNULL(ss.RetailQuantity,0)) < 0 THEN 0
                ELSE CAST(ISNULL(ss.WholesaleQuantity,0)-ROUND(ISNULL(ss.RetailQuantity,0),+1) AS INT) END 
                END as QuantityToBuy,
            ssi.ItemGroup, 
            ssi.ProductClass,
            ssi.Brand,
            ssi.SupplierNumber, 
            ssi.SupplierNick,
            ssi.cn3,
            ssi.cn1,
            ssi.Substance,
            ssc.Comment
            FROM [MADAM].[dbo].[SafetyStockCondition] ssc
                LEFT JOIN [MADAM].[dbo].[SafetyStockItem] ssi ON ssi.Id = ssc.SafetyStockItemId
                LEFT JOIN [MADAM].[dbo].[SafetyStockPharmacyChain] sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
                LEFT JOIN [MADAM].[dbo].[SafetyStock] ss ON ss.SafetyStockItemId = ssc.SafetyStockItemId;");
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
            sspc.DisplayName as PharmacyChainDisplayName,
            sspc.Id as PharmacyChainId,
            ssi.ItemNo, 
            ssi.ItemName,
            ssc.CheckDays,
            ss.WholesaleQuantity,
            ss.RetailQuantity,
            CASE WHEN (sspc.[Group] = 'Benu' OR sspc.Value = 'Benu') THEN ISNULL(ss.WholesaleQuantity,0) ELSE 
                CASE WHEN (ISNULL(ss.WholesaleQuantity,0)-ISNULL(ss.RetailQuantity,0)) < 0 THEN 0
                ELSE CAST(ISNULL(ss.WholesaleQuantity,0)-ROUND(ISNULL(ss.RetailQuantity,0),+1) AS INT) END 
                END as QuantityToBuy,
            ssi.ItemGroup, 
            ssi.ProductClass,
            ssi.Brand,
            ssi.SupplierNumber, 
            ssi.SupplierNick,
            ssi.cn3,
            ssi.cn1,
            ssi.Substance,
            ssc.Comment
            FROM [MADAM].[dbo].[SafetyStockCondition] ssc
                LEFT JOIN [MADAM].[dbo].[SafetyStockItem] ssi ON ssi.Id = ssc.SafetyStockItemId
                LEFT JOIN [MADAM].[dbo].[SafetyStockPharmacyChain] sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId
                LEFT JOIN [MADAM].[dbo].[SafetyStock] ss ON ss.SafetyStockItemId = ssc.SafetyStockItemId;");
        }
    }
}

