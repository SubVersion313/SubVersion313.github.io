using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class Floors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyUnitFloors",
                columns: table => new
                {
                    FloorId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FloorName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CompanyID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BranchID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUnitFloors", x => x.FloorId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUnitFloors");
        }
    }
}
