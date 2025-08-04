using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class PimProductTypesName300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[PimProductTypes]
                ALTER COLUMN [Name] [nvarchar](300) NULL
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE [dbo].[PimProductTypes]
                SET [Name] = LEFT(LTRIM(RTRIM([Name])), 100)

                ALTER TABLE [dbo].[PimProductTypes]
                ALTER COLUMN [Name] [nvarchar](100) NULL
            ");
        }
    }
}
