using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUsernameFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Vaccines_Name",
                table: "Vaccines");

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("17f01d8f-ba82-4d3e-bd9d-4164533de3e2"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("1d22d7af-e864-4c3c-9e49-351e98bc2b45"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("2d4de1ea-6b8b-47cb-bab6-e7ad6325ae26"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("2eb00e47-7522-4c0f-94bd-a21e2ea1c2bb"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("5db28616-aa0c-4818-9e1c-214e0e07fe40"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("6c4d3582-b300-485d-ae6c-a6b26f437ae2"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("7a5ff6db-5be0-401f-90cf-eadccaa9ac55"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("7cd24abf-3139-4eff-aee5-720853c33649"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("a4ab1168-fed4-450a-8a03-ec746f30f5e0"));

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Users");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Vaccinations",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2023, 3, 9));

            migrationBuilder.AddUniqueConstraint(
                name: "uniqueVaccineName",
                table: "Vaccines",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "uniqueVaccineName",
                table: "Vaccines");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "Vaccinations",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2023, 3, 9),
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Vaccines_Name",
                table: "Vaccines",
                column: "Name");

            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("17f01d8f-ba82-4d3e-bd9d-4164533de3e2"), "Sindi" },
                    { new Guid("1d22d7af-e864-4c3c-9e49-351e98bc2b45"), "Holandês" },
                    { new Guid("2d4de1ea-6b8b-47cb-bab6-e7ad6325ae26"), "Pardo Suíço" },
                    { new Guid("2eb00e47-7522-4c0f-94bd-a21e2ea1c2bb"), "Simental" },
                    { new Guid("5db28616-aa0c-4818-9e1c-214e0e07fe40"), "Nelore" },
                    { new Guid("6c4d3582-b300-485d-ae6c-a6b26f437ae2"), "Brahman" },
                    { new Guid("7a5ff6db-5be0-401f-90cf-eadccaa9ac55"), "Jersey" },
                    { new Guid("7cd24abf-3139-4eff-aee5-720853c33649"), "Gir" },
                    { new Guid("a4ab1168-fed4-450a-8a03-ec746f30f5e0"), "Guzerá" }
                });
        }
    }
}
