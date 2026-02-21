using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_2_AddReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SCRIPT_ID = table.Column<int>(type: "int", nullable: true),
                    COMMENT_ID = table.Column<int>(type: "int", nullable: true),
                    VOTER_KEY = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    REACTION_TYPE = table.Column<int>(type: "int", nullable: false),
                    CREATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Reactions_Comments_COMMENT_ID",
                        column: x => x.COMMENT_ID,
                        principalTable: "Comments",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Reactions_Script_SCRIPT_ID",
                        column: x => x.SCRIPT_ID,
                        principalTable: "Script",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_COMMENT_ID",
                table: "Reactions",
                column: "COMMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_SCRIPT_ID",
                table: "Reactions",
                column: "SCRIPT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_VOTER_KEY_SCRIPT_ID_COMMENT_ID",
                table: "Reactions",
                columns: new[] { "VOTER_KEY", "SCRIPT_ID", "COMMENT_ID" },
                unique: true,
                filter: "[SCRIPT_ID] IS NOT NULL AND [COMMENT_ID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reactions");
        }
    }
}
