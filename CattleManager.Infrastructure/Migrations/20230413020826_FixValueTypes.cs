using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class FixValueTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions");

            migrationBuilder.AlterColumn<char>(
                name: "PeriodOfDay",
                table: "MilkProductions",
                type: "character(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<decimal>(
                name: "MilkInLiters",
                table: "MilkProductions",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceInCentsInReais",
                table: "Cattle",
                type: "numeric(12,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Cattle",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                defaultValue: "https://i.imgur.com/xxNaPZH.png",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldDefaultValue: "https://i.imgur.com/xxNaPZH.png");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccines_Name",
                table: "Vaccines",
                column: "Name",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions",
                sql: "\"MilkProductions\".\"PeriodOfDay\" = 'm' OR \"MilkProductions\".\"PeriodOfDay\" = 'a' OR \"MilkProductions\".\"PeriodOfDay\" = 'n' OR \"MilkProductions\".\"PeriodOfDay\" = 'd'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vaccines_Name",
                table: "Vaccines");

            migrationBuilder.DropCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions");

            migrationBuilder.AlterColumn<string>(
                name: "PeriodOfDay",
                table: "MilkProductions",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(char),
                oldType: "character(1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MilkInLiters",
                table: "MilkProductions",
                type: "numeric(6,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)");

            migrationBuilder.AlterColumn<int>(
                name: "PriceInCentsInReais",
                table: "Cattle",
                type: "integer",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(12,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Cattle",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "https://i.imgur.com/xxNaPZH.png",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldDefaultValue: "https://i.imgur.com/xxNaPZH.png");

            migrationBuilder.AddCheckConstraint(
                name: "CK_MilkProduction_PeriodOfDay",
                table: "MilkProductions",
                sql: "\"MilkProductions\".\"PeriodOfDay\" = 'morning' OR \"MilkProductions\".\"PeriodOfDay\" = 'afternoon' OR \"MilkProductions\".\"PeriodOfDay\" = 'night' OR \"MilkProductions\".\"PeriodOfDay\" = 'whole day'");
        }
    }
}
