using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class schedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalsDetail",
                columns: table => new
                {
                    GoalDetailsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GoalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyUnitId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VIP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TT = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalsDetail", x => x.GoalDetailsID);
                });

            migrationBuilder.CreateTable(
                name: "GoalsMaster",
                columns: table => new
                {
                    GoalId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dt_stamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalsMaster", x => x.GoalId);
                });

            migrationBuilder.CreateTable(
                name: "StaffsSchedule",
                columns: table => new
                {
                    SSId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dt_stamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrntDate = table.Column<int>(type: "int", nullable: false),
                    CurrntDay = table.Column<int>(type: "int", nullable: false),
                    CurrntMonth = table.Column<int>(type: "int", nullable: false),
                    CurrntYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffsSchedule", x => x.SSId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoalsDetail");

            migrationBuilder.DropTable(
                name: "GoalsMaster");

            migrationBuilder.DropTable(
                name: "StaffsSchedule");
        }
    }
}
