using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeersCheersVasis.Data.Migrations
{
    /// <inheritdoc />
    public partial class V1_2_AddScheduledPublishDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SCHEDULED_PUBLISH_DATE",
                table: "Script",
                type: "DATETIME",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SCHEDULED_PUBLISH_DATE",
                table: "Script");
        }
    }
}
