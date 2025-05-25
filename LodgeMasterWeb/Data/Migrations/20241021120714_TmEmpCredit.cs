using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class TmEmpCredit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TmEmployeesCredit",
                columns: table => new
                {
                    UFShiftId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EmpId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UFShiftStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxCredits = table.Column<int>(type: "int", nullable: false),
                    MaxZones = table.Column<int>(type: "int", nullable: false),
                    RoomSets = table.Column<int>(type: "int", nullable: false),
                    ShiftTypeId = table.Column<int>(type: "int", nullable: false),
                    Weekend = table.Column<int>(type: "int", nullable: false),
                    sNotes = table.Column<string>(type: "nvarchar(1500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TmEmployeesCredit", x => x.UFShiftId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TmEmployeesCredit");
        }
    }
}
