using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class UnitCat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnitCat",
                table: "CompanyUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CompanyUnitsCat",
                columns: table => new
                {
                    UnitCatId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCatName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUnitsCat", x => x.UnitCatId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUnitsCatType",
                columns: table => new
                {
                    UnitCatTypeId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCatTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUnitsCatType", x => x.UnitCatTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUnitsLinkCatType",
                columns: table => new
                {
                    LinkCartypeAndCatID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCatId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitCatTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUnitsLinkCatType", x => x.LinkCartypeAndCatID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUnitsCat");

            migrationBuilder.DropTable(
                name: "CompanyUnitsCatType");

            migrationBuilder.DropTable(
                name: "CompanyUnitsLinkCatType");

            migrationBuilder.DropColumn(
                name: "UnitCat",
                table: "CompanyUnits");
        }
    }
}
