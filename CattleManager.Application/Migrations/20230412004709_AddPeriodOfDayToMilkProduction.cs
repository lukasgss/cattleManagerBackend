using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodOfDayToMilkProduction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MilkPerDayInLiters",
                table: "MilkProductions",
                newName: "MilkInLiters");

            migrationBuilder.AddColumn<string>(
                name: "PeriodOfDay",
                table: "MilkProductions",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions",
                sql: "\"MilkProductions\".\"PeriodOfDay\" = 'morning' OR \"MilkProductions\".\"PeriodOfDay\" = 'afternoon' OR \"MilkProductions\".\"PeriodOfDay\" = 'night' OR \"MilkProductions\".\"PeriodOfDay\" = 'whole day'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions");

            migrationBuilder.DropColumn(
                name: "PeriodOfDay",
                table: "MilkProductions");

            migrationBuilder.RenameColumn(
                name: "MilkInLiters",
                table: "MilkProductions",
                newName: "MilkPerDayInLiters");
        }
    }
}
