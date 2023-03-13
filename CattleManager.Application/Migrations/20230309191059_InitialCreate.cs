using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CattleManager.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                    table.UniqueConstraint("AK_Breeds_Name", x => x.Name);
                });

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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
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
                    table.UniqueConstraint("AK_Vaccines_Name", x => x.Name);
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
                    Image = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DateOfDeath = table.Column<DateOnly>(type: "date", nullable: true),
                    CauseOfDeath = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DateOfSale = table.Column<DateOnly>(type: "date", nullable: true),
                    PriceInCentsInReais = table.Column<int>(type: "integer", nullable: true),
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
                name: "CattleBreed",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuantityInPercentage = table.Column<decimal>(type: "numeric(6,5)", nullable: false),
                    CattleId = table.Column<Guid>(type: "uuid", nullable: false),
                    BreedId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CattleBreed", x => x.Id);
                    table.CheckConstraint("QuantityBetween0And1", "\"CattleBreed\".\"QuantityInPercentage\" >= 0 AND \"CattleBreed\".\"QuantityInPercentage\" <= 1");
                    table.ForeignKey(
                        name: "FK_CattleBreed_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CattleBreed_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CattleOwners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CattleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CattleOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CattleOwners_Cattle_CattleId",
                        column: x => x.CattleId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CattleOwners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    FatherId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotherId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conceptions_Cattle_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Cattle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conceptions_Cattle_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Cattle",
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
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DosageInMl = table.Column<decimal>(type: "numeric(9,4)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(2023, 3, 9)),
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

            migrationBuilder.InsertData(
                table: "Sex",
                columns: new[] { "Id", "Gender" },
                values: new object[,]
                {
                    { (byte)0, "Fêmea" },
                    { (byte)1, "Macho" }
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
                name: "IX_CattleBreed_BreedId",
                table: "CattleBreed",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_CattleBreed_CattleId",
                table: "CattleBreed",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_CattleOwners_CattleId",
                table: "CattleOwners",
                column: "CattleId");

            migrationBuilder.CreateIndex(
                name: "IX_CattleOwners_UserId",
                table: "CattleOwners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conceptions_FatherId",
                table: "Conceptions",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Conceptions_MotherId",
                table: "Conceptions",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_MilkProductions_CattleId",
                table: "MilkProductions",
                column: "CattleId");

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
                name: "CattleBreed");

            migrationBuilder.DropTable(
                name: "CattleOwners");

            migrationBuilder.DropTable(
                name: "Conceptions");

            migrationBuilder.DropTable(
                name: "MilkProductions");

            migrationBuilder.DropTable(
                name: "Vaccinations");

            migrationBuilder.DropTable(
                name: "Breeds");

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
