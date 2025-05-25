using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class inspectionDet_afterAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommetAnswerAfter",
                table: "InspectionsDet",
                type: "nvarchar(2500)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserAnswerAfter",
                table: "InspectionsDet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommetAnswerAfter",
                table: "InspectionsDet");

            migrationBuilder.DropColumn(
                name: "UserAnswerAfter",
                table: "InspectionsDet");
        }
    }
}
