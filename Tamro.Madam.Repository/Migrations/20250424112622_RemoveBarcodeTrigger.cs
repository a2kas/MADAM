using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBarcodeTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER TR_Barcode_EditedAt;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            create TRIGGER [dbo].[TR_Barcode_EditedAt]
		      ON [dbo].[Barcode] 
		      FOR UPDATE 
		      NOT FOR REPLICATION AS 
		    BEGIN 
		      SET NOCOUNT ON

		      UPDATE Barcode SET EditedAt = GETDATE() FROM Barcode a JOIN INSERTED i ON a.Id = i.Id
			    WHERE i.EditedAt IS NULL OR i.EditedAt < GETDATE() - 0.0001

		      RETURN
		    END");
        }
    }
}
