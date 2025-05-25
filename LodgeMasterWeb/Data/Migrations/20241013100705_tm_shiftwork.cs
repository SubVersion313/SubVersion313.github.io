using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class tm_shiftwork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TmCreditRooms",
                columns: table => new
                {
                    CreditRoomId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UFShiftStatus = table.Column<string>(type: "nvarchar(400)", nullable: false),
                    MaxCredits = table.Column<int>(type: "int", nullable: false),
                    MaxZones = table.Column<int>(type: "int", nullable: false),
                    RoomSets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmCreditRooms", x => x.CreditRoomId);
                });

            migrationBuilder.CreateTable(
                name: "TmShiftsWorks",
                columns: table => new
                {
                    ShiftID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShiftName_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShiftName_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    MasterNameDefault = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    isDefault = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmShiftsWorks", x => x.ShiftID);
                });

            migrationBuilder.CreateTable(
                name: "TmShiftWeeks",
                columns: table => new
                {
                    ShiftWeekId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreditRoomId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    EmpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    dt_Start = table.Column<int>(type: "int", nullable: false),
                    dt_End = table.Column<int>(type: "int", nullable: false),
                    UFShiftStatus = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    MaxCredits = table.Column<int>(type: "int", nullable: false),
                    MaxZones = table.Column<int>(type: "int", nullable: false),
                    RoomSets = table.Column<int>(type: "int", nullable: false),
                    ShiftTypeId = table.Column<int>(type: "int", nullable: false),
                    Weekend = table.Column<int>(type: "int", nullable: false),
                    sNotes = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmShiftWeeks", x => x.ShiftWeekId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TmCreditRooms");

            migrationBuilder.DropTable(
                name: "TmShiftsWorks");

            migrationBuilder.DropTable(
                name: "TmShiftWeeks");
        }
    }
}
