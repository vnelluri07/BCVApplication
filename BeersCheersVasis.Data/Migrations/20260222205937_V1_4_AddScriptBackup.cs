using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_4_AddScriptBackup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScriptBackups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCRIPT_ID = table.Column<int>(type: "int", nullable: false),
                    PROVIDER = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EXTERNAL_ID = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EXTERNAL_URL = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    BACKED_UP_AT = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ERROR_MESSAGE = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScriptBackups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ScriptBackups_Script_SCRIPT_ID",
                        column: x => x.SCRIPT_ID,
                        principalTable: "Script",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScriptBackups_SCRIPT_ID",
                table: "ScriptBackups",
                column: "SCRIPT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScriptBackups");
        }
    }
}
