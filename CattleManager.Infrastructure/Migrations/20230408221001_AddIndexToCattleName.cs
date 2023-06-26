using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexToCattleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConceptionDate",
                table: "Cattle");

            migrationBuilder.CreateIndex(
                name: "IX_Cattle_Name",
                table: "Cattle",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cattle_Name",
                table: "Cattle");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ConceptionDate",
                table: "Cattle",
                type: "date",
                nullable: true);
        }
    }
}
