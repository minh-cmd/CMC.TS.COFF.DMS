using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMC.TS.COFF.DMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintForCategoriesCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                table: "Categories");
        }
    }
}
