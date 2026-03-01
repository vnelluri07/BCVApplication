using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_5_AddSubCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SUBCATEGORY_ID",
                table: "Script",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CATEGORY_ID = table.Column<int>(type: "int", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DESCRIPTION = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ICON = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SORT_ORDER = table.Column<int>(type: "int", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CATEGORY_ID",
                        column: x => x.CATEGORY_ID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Script_SUBCATEGORY_ID",
                table: "Script",
                column: "SUBCATEGORY_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CATEGORY_ID",
                table: "SubCategories",
                column: "CATEGORY_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Script_SubCategories_SUBCATEGORY_ID",
                table: "Script",
                column: "SUBCATEGORY_ID",
                principalTable: "SubCategories",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Script_SubCategories_SUBCATEGORY_ID",
                table: "Script");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Script_SUBCATEGORY_ID",
                table: "Script");

            migrationBuilder.DropColumn(
                name: "SUBCATEGORY_ID",
                table: "Script");
        }
    }
}
