﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp_GozenBv.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarMaintenances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    MaintenanceKm = table.Column<int>(type: "int", nullable: true),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaintenanceInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Done = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarMaintenances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarPark",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicencePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChassisNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Km = table.Column<double>(type: "float", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeuringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeadlineKeuringDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarPark", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    MinQuantity = table.Column<int>(type: "int", nullable: false),
                    NoReturn = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    Used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    StockAmount = table.Column<int>(type: "int", nullable: false),
                    ProductNameCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoReturn = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    IsDamaged = table.Column<bool>(type: "bit", nullable: false),
                    DamagedAmount = table.Column<int>(type: "int", nullable: true),
                    RepairedAmount = table.Column<int>(type: "int", nullable: true),
                    DeletedAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockLogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LogCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Damaged = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<int>(type: "int", nullable: false),
                    Controller = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockLogs_EmployeeId",
                table: "StockLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId",
                table: "UserLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarMaintenances");

            migrationBuilder.DropTable(
                name: "CarPark");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "StockLogItems");

            migrationBuilder.DropTable(
                name: "StockLogs");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}