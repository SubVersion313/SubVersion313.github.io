using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class Timemanagement_Tasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TmWeeksMaster",
                columns: table => new
                {
                    WeekMsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmployeeID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dtCraete = table.Column<int>(type: "int", nullable: false),
                    dtCraeteStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WeekNo = table.Column<int>(type: "int", nullable: false),
                    WeekYear = table.Column<int>(type: "int", nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserIDCreate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmWeeksMaster", x => x.WeekMsID);
                });

            migrationBuilder.CreateTable(
                name: "TmWeeksDetails",
                columns: table => new
                {
                    TmDet_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    WeekMsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    dtCraete = table.Column<int>(type: "int", nullable: false),
                    DayNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDay = table.Column<int>(type: "int", nullable: false),
                    DayNotes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmWeeksDetails", x => x.TmDet_ID);
                    table.ForeignKey(
                        name: "FK_TmWeeksDetails_TmWeeksMaster_WeekMsID",
                        column: x => x.WeekMsID,
                        principalTable: "TmWeeksMaster",
                        principalColumn: "WeekMsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TmWeeksTasks",
                columns: table => new
                {
                    TmWeek_DetID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TmDet_ID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    sItemNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isTransOrder = table.Column<int>(type: "int", nullable: false),
                    dtTransOrder = table.Column<DateTime>(type: "datetime2", nullable: true),
                    iSorted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmWeeksTasks", x => x.TmWeek_DetID);
                    table.ForeignKey(
                        name: "FK_TmWeeksTasks_TmWeeksDetails_TmDet_ID",
                        column: x => x.TmDet_ID,
                        principalTable: "TmWeeksDetails",
                        principalColumn: "TmDet_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TmWeeksDetails_WeekMsID",
                table: "TmWeeksDetails",
                column: "WeekMsID");

            migrationBuilder.CreateIndex(
                name: "IX_TmWeeksTasks_TmDet_ID",
                table: "TmWeeksTasks",
                column: "TmDet_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TmWeeksTasks");

            migrationBuilder.DropTable(
                name: "TmWeeksDetails");

            migrationBuilder.DropTable(
                name: "TmWeeksMaster");
        }
    }
}
