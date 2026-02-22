using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_4_AddCategorySoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_DELETED",
                table: "Categories");
        }
    }
}
