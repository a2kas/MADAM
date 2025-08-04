using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class createhideitemview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW v_hideitem AS
                SELECT
                    ssc.Id,
                    ssi.ItemNo2,
                    ssc.RowVer AS DStamp,
                    sspc.Value AS CompType,
                    ssc.[User] AS WindowsUser,
                    ssc.CanBuy AS Deleted,
                    ssc.Comment
                FROM 
                    MADAM.dbo.SafetyStockCondition ssc
                    LEFT JOIN MADAM.dbo.SafetyStockItem ssi ON ssi.Id = ssc.SafetyStockItemId
                    LEFT JOIN MADAM.dbo.SafetyStockPharmacyChain sspc ON sspc.Id = ssc.SafetyStockPharmacyChainId;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[v_hideitem]'))
                        DROP VIEW [dbo].[v_hideitem];");
        }
    }
}
