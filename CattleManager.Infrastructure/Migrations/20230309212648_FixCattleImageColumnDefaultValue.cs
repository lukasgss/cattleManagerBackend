using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class FixCattleImageColumnDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("0347e873-0fb0-47a4-a183-00da20af683c"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("1807cd67-f6ff-4473-ac52-cc5ca9570268"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("1d66c840-f7bf-414c-bd62-e83af12bc417"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("5f2a7d6c-bbd8-4029-9615-c9eba93b162d"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("84dda583-e177-4798-a4a8-efd2eb2bec33"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("b09c73a3-02b4-4218-af87-ca0018e2e87c"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("c6289763-4929-4110-920e-edeec109e50b"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("db455b05-bf98-4f03-8cdd-3ce5151fbd0b"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: new Guid("faf9ffd7-db74-4587-af60-1301ac46b450"));

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Cattle",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "https://i.imgur.com/xxNaPZH.png",
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Cattle",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldDefaultValue: "https://i.imgur.com/xxNaPZH.png");

            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0347e873-0fb0-47a4-a183-00da20af683c"), "Sindi" },
                    { new Guid("1807cd67-f6ff-4473-ac52-cc5ca9570268"), "Simental" },
                    { new Guid("1d66c840-f7bf-414c-bd62-e83af12bc417"), "Holandês" },
                    { new Guid("5f2a7d6c-bbd8-4029-9615-c9eba93b162d"), "Guzerá" },
                    { new Guid("84dda583-e177-4798-a4a8-efd2eb2bec33"), "Gir" },
                    { new Guid("b09c73a3-02b4-4218-af87-ca0018e2e87c"), "Jersey" },
                    { new Guid("c6289763-4929-4110-920e-edeec109e50b"), "Pardo Suíço" },
                    { new Guid("db455b05-bf98-4f03-8cdd-3ce5151fbd0b"), "Nelore" },
                    { new Guid("faf9ffd7-db74-4587-af60-1301ac46b450"), "Brahman" }
                });
        }
    }
}
