﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp_GozenBv.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmaName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Used = table.Column<bool>(nullable: false),
                    Cost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WagenPark",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicencePlate = table.Column<string>(nullable: true),
                    ChassisNumber = table.Column<string>(nullable: true),
                    Km = table.Column<double>(nullable: false),
                    FirmaName = table.Column<string>(nullable: true),
                    KeuringDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WagenPark", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(nullable: true),
                    StockId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    StockId = table.Column<int>(nullable: true)
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
                    table.ForeignKey(
                        name: "FK_StockLogs_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Orders_StockId",
                table: "Orders",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLogs_EmployeeId",
                table: "StockLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLogs_StockId",
                table: "StockLogs",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_WagenMaintenances_WagenParkId",
                table: "WagenMaintenances",
                column: "WagenParkId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "StockLogs");

            migrationBuilder.DropTable(
                name: "WagenMaintenances");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "WagenPark");
        }
    }
}
