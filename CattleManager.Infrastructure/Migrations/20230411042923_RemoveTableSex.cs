using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTableSex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cattle_Sex_SexId",
                table: "Cattle");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropIndex(
                name: "IX_Cattle_SexId",
                table: "Cattle");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Cattle_SexId",
                table: "Cattle",
                sql: "\"Cattle\".\"SexId\" = 0 OR \"Cattle\".\"SexId\" = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Cattle_SexId",
                table: "Cattle");

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                    table.UniqueConstraint("AK_Sex_Gender", x => x.Gender);
                    table.CheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1");
                });

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Gender" },
                values: new object[,]
                {
                    { (byte)0, "Fêmea" },
                    { (byte)1, "Macho" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cattle_SexId",
                table: "Cattle",
                column: "SexId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cattle_Sex_SexId",
                table: "Cattle",
                column: "SexId",
                principalTable: "Sex",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
