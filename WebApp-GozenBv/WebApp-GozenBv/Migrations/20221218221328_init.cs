using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp_GozenBv.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Firmas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmaName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firmas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(nullable: false),
                    ProductBrand = table.Column<string>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    MinQuantity = table.Column<int>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Used = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    FirmaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Firmas_FirmaId",
                        column: x => x.FirmaId,
                        principalTable: "Firmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WagenPark",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicencePlate = table.Column<string>(nullable: true),
                    ChassisNumber = table.Column<string>(nullable: true),
                    Brand = table.Column<string>(nullable: true),
                    Model = table.Column<string>(nullable: true),
                    Km = table.Column<double>(nullable: false),
                    KeuringDate = table.Column<DateTime>(nullable: false),
                    DeadlineKeuring = table.Column<DateTime>(nullable: false),
                    FirmaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenPark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WagenPark_Firmas_FirmaId",
                        column: x => x.FirmaId,
                        principalTable: "Firmas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockDamaged",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogCode = table.Column<string>(nullable: true),
                    StockId = table.Column<int>(nullable: false),
                    StockAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDamaged", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockDamaged_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogCode = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLogItems_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockLogDate = table.Column<DateTime>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    LogCode = table.Column<string>(nullable: true),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    Damaged = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WagenMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaintenanceDate = table.Column<DateTime>(nullable: false),
                    MaintenanceNotes = table.Column<string>(nullable: true),
                    WagenId = table.Column<int>(nullable: false),
                    WagenParkId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WagenMaintenances_WagenPark_WagenParkId",
                        column: x => x.WagenParkId,
                        principalTable: "WagenPark",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FirmaId",
                table: "Employees",
                column: "FirmaId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDamaged_StockId",
                table: "StockDamaged",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLogItems_StockId",
                table: "StockLogItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLogs_EmployeeId",
                table: "StockLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WagenMaintenances_WagenParkId",
                table: "WagenMaintenances",
                column: "WagenParkId");

            migrationBuilder.CreateIndex(
                name: "IX_WagenPark_FirmaId",
                table: "WagenPark",
                column: "FirmaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockDamaged");

            migrationBuilder.DropTable(
                name: "StockLogItems");

            migrationBuilder.DropTable(
                name: "StockLogs");

            migrationBuilder.DropTable(
                name: "WagenMaintenances");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "WagenPark");

            migrationBuilder.DropTable(
                name: "Firmas");
        }
    }
}
