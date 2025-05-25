using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class UnitUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BranchID",
                table: "CompanyUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FloorID",
                table: "CompanyUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BranchID",
                table: "CompanyUnitFloors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "CompanyUnits");

            migrationBuilder.DropColumn(
                name: "FloorID",
                table: "CompanyUnits");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "CompanyUnitFloors");
        }
    }
}
