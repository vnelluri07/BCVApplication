using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_1_AddCategoryCommentAppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // New table: AppUsers
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GOOGLE_ID = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DISPLAY_NAME = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AVATAR_URL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ROLE = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IS_ANONYMOUS = table.Column<bool>(type: "bit", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.ID);
                });

            // New table: Categories
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            // Add new columns to Script
            migrationBuilder.AddColumn<int>(
                name: "CATEGORY_ID",
                table: "Script",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_DELETED",
                table: "Script",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IS_PUBLISHED",
                table: "Script",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PUBLISHED_DATE",
                table: "Script",
                type: "DATETIME",
                nullable: true);

            // New table: Comments
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCRIPT_ID = table.Column<int>(type: "int", nullable: false),
                    APP_USER_ID = table.Column<int>(type: "int", nullable: false),
                    PARENT_COMMENT_ID = table.Column<int>(type: "int", nullable: true),
                    CONTENT = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IS_DELETED = table.Column<bool>(type: "bit", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    MODIFIED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comments_AppUsers_APP_USER_ID",
                        column: x => x.APP_USER_ID,
                        principalTable: "AppUsers",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Comments_Comments_PARENT_COMMENT_ID",
                        column: x => x.PARENT_COMMENT_ID,
                        principalTable: "Comments",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Comments_Script_SCRIPT_ID",
                        column: x => x.SCRIPT_ID,
                        principalTable: "Script",
                        principalColumn: "ID");
                });

            // FK: Script -> Categories
            migrationBuilder.CreateIndex(
                name: "IX_Script_CATEGORY_ID",
                table: "Script",
                column: "CATEGORY_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Script_Categories_CATEGORY_ID",
                table: "Script",
                column: "CATEGORY_ID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            // Indexes for Comments
            migrationBuilder.CreateIndex(
                name: "IX_Comments_APP_USER_ID",
                table: "Comments",
                column: "APP_USER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PARENT_COMMENT_ID",
                table: "Comments",
                column: "PARENT_COMMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SCRIPT_ID",
                table: "Comments",
                column: "SCRIPT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Comments");
            migrationBuilder.DropForeignKey(name: "FK_Script_Categories_CATEGORY_ID", table: "Script");
            migrationBuilder.DropIndex(name: "IX_Script_CATEGORY_ID", table: "Script");
            migrationBuilder.DropColumn(name: "CATEGORY_ID", table: "Script");
            migrationBuilder.DropColumn(name: "IS_DELETED", table: "Script");
            migrationBuilder.DropColumn(name: "IS_PUBLISHED", table: "Script");
            migrationBuilder.DropColumn(name: "PUBLISHED_DATE", table: "Script");
            migrationBuilder.DropTable(name: "Categories");
            migrationBuilder.DropTable(name: "AppUsers");
        }
    }
}
