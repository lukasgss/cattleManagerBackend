using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddIsInLactationPeriodColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInLactationPeriod",
                table: "Cattle",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInLactationPeriod",
                table: "Cattle");
        }
    }
}
