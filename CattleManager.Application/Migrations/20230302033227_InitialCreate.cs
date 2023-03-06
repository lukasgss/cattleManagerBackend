using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    table.CheckConstraint("IdIs0Or1", "\"Sex\".\"Id\" >= 0 AND \"Sex\".\"Id\" <= 1");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vaccines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cattle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ConceptionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    YearOfBirth = table.Column<int>(type: "integer", nullable: false),
                    Image = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DateOfDeath = table.Column<DateOnly>(type: "date", nullable: true),
                    CauseOfDeath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DateOfSale = table.Column<DateOnly>(type: "date", nullable: true),
                    FatherId = table.Column<Guid>(type: "uuid", nullable: true),
                    MotherId = table.Column<Guid>(type: "uuid", nullable: true),
                    SexId = table.Column<byte>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cattle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cattle_Cattle_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cattle_Cattle_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Cattle_Sex_SexId",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MilkProductions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MilkPerDayInLiters = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CattleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MilkProductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MilkProductions_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CattleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Owners_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Owners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DosageInMl = table.Column<decimal>(type: "numeric(9,4)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    CattleId = table.Column<Guid>(type: "uuid", nullable: false),
                    VaccineId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccinations_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vaccinations_Vaccines_VaccineId",
                        column: x => x.VaccineId,
                        principalTable: "Vaccines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cattle_FatherId",
                table: "Cattle",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Cattle_MotherId",
                table: "Cattle",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Cattle_SexId",
                table: "Cattle",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_MilkProductions_CattleId",
                table: "MilkProductions",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_CattleId",
                table: "Owners",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_UserId",
                table: "Owners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_CattleId",
                table: "Vaccinations",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_VaccineId",
                table: "Vaccinations",
                column: "VaccineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MilkProductions");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "Vaccinations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cattle");

            migrationBuilder.DropTable(
                name: "Vaccines");

            migrationBuilder.DropTable(
                name: "Sex");
        }
    }
}
