using System;
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
                    ChassisNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
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
                name: "Material",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewQty = table.Column<int>(type: "int", nullable: false),
                    UsedQty = table.Column<int>(type: "int", nullable: false),
                    MinQty = table.Column<int>(type: "int", nullable: false),
                    InUseAmount = table.Column<int>(type: "int", nullable: false),
                    InDepotAmount = table.Column<int>(type: "int", nullable: false),
                    InRepairQty = table.Column<int>(type: "int", nullable: false),
                    DeletedQty = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    NoReturn = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
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
                name: "MaterialLogHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    EditTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Damaged = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLogHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialLogHistory_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Damaged = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialAmount = table.Column<int>(type: "int", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    IsDamaged = table.Column<bool>(type: "bit", nullable: false),
                    DamagedAmount = table.Column<int>(type: "int", nullable: true),
                    RepairAmount = table.Column<int>(type: "int", nullable: true),
                    DeleteAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialLogItems_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialLogItemsHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialLogItemId = table.Column<int>(type: "int", nullable: false),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    EditTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialAmount = table.Column<int>(type: "int", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false),
                    IsDamaged = table.Column<bool>(type: "bit", nullable: false),
                    DamagedAmount = table.Column<int>(type: "int", nullable: true),
                    RepairAmount = table.Column<int>(type: "int", nullable: true),
                    DeleteAmount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialLogItemsHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialLogItemsHistory_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    RepairInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairTickets_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Material",
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
                    ControllerId = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_MaterialLogHistory_EmployeeId",
                table: "MaterialLogHistory",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLogItems_MaterialId",
                table: "MaterialLogItems",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLogItemsHistory_MaterialId",
                table: "MaterialLogItemsHistory",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialLogs_EmployeeId",
                table: "MaterialLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTickets_MaterialId",
                table: "RepairTickets",
                column: "MaterialId");

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
                name: "MaterialLogHistory");

            migrationBuilder.DropTable(
                name: "MaterialLogItems");

            migrationBuilder.DropTable(
                name: "MaterialLogItemsHistory");

            migrationBuilder.DropTable(
                name: "MaterialLogs");

            migrationBuilder.DropTable(
                name: "RepairTickets");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Material");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
