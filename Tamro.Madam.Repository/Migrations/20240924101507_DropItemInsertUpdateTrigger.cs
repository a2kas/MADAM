using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DropItemInsertUpdateTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TRIGGER [dbo].[TR_Item_InsertUpdate]
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				SET ANSI_NULLS ON
				GO

				SET QUOTED_IDENTIFIER ON
				GO

				CREATE TRIGGER [dbo].[TR_Item_InsertUpdate]
					ON [dbo].[Item]
					AFTER INSERT, UPDATE
					AS
					BEGIN
						SET NOCOUNT ON; 
						IF NOT EXISTS(SELECT * FROM DELETED) OR (NOT UPDATE(ItemName) AND NOT UPDATE(EditedAt))
						BEGIN  
							--Update EditedAt, ItemName
							UPDATE Item 
							SET 
							ItemName = f.ConstructedItemName,
							EditedAt = GETDATE() 
							FROM Item i
							JOIN
							(SELECT
								i.Id,
								CASE WHEN COALESCE(b.Name, '') <> '' THEN b.Name + ' ' ELSE '' END--BRAND SUBBRAND
								+ CASE WHEN COALESCE(i.Description, '') <> '' THEN i.Description + ' ' ELSE '' END--ITEM DESCRIPTION
								+ CASE WHEN COALESCE(i.Strength, '') <> '' THEN i.Strength + ' ' ELSE '' END--STRENGTH
								+ CASE WHEN COALESCE(f.Name, '') <> '' THEN f.Name + ' ' ELSE '' END--FORM
								+ CASE WHEN COALESCE(i.Measure, 0) <> 0 AND COALESCE(u.Name, '') <> '' THEN CAST(CAST(COALESCE(i.Measure, 0) AS FLOAT) AS NVARCHAR) + ' ' + u.Name + ' ' ELSE '' END--Measure UOM
								+ CASE WHEN COALESCE(i.Numero, 0) <> 0 THEN 'N' + CAST(i.Numero AS NVARCHAR) + ' ' ELSE '' END
								+ CASE WHEN COALESCE(p.Name, '') <> '' AND COALESCE(i.ParallelParentItemId, 0) = 0 THEN '(' + p.Name + ') ' ELSE '' END 
								+ CASE WHEN COALESCE(i.ParallelParentItemId, 0) <> 0 THEN '(' + COALESCE(s.Name, '') + ') P' ELSE '' END
								AS ConstructedItemName 
								FROM INSERTED i
  								LEFT JOIN Brand b on i.BrandId = b.Id
								LEFT JOIN Producer p on i.ProducerId = p.Id
								LEFT JOIN Form f on i.FormId = f.Id 
								LEFT JOIN ATC a on i.ATCId = a.Id
								LEFT JOIN UOM u ON i.UOMId = u.Id
								LEFT JOIN SupplierNick s ON i.SupplierNickId = s.Id) f
								ON i.Id = f.Id;
							--LOG
							INSERT INTO ItemLog (Id, Description, ProducerId, BrandId, Strength, FormId, Measure, UOMId, ATCId, SupplierNickId, Numero, Dose, ActiveSubstance, Active, ParallelParentItemId, RequestorId, ItemName, EditedAt, EditedBy)
							SELECT ins.Id, ins.Description, ins.ProducerId, ins.BrandId, ins.Strength, ins.FormId, ins.Measure, ins.UOMId, ins.ATCId, ins.SupplierNickId, ins.Numero, ins.Dose, ins.ActiveSubstance, ins.Active, ins.ParallelParentItemId, ins.RequestorId, i.ItemName, i.EditedAt, ins.EditedBy
							FROM Item i 
							JOIN INSERTED ins on ins.Id = i.Id
						END 
					END
				GO

				ALTER TABLE [dbo].[Item] ENABLE TRIGGER [TR_Item_InsertUpdate]
				GO
            ");
        }
    }
}
