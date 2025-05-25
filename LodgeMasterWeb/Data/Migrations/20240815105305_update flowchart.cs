using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class updateflowchart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayMessage_E",
                table: "FlowChartDetails",
                newName: "HeaderMessage_E");

            migrationBuilder.RenameColumn(
                name: "DisplayMessage_A",
                table: "FlowChartDetails",
                newName: "HeaderMessage_A");

            migrationBuilder.RenameColumn(
                name: "MultiAnwserExtraData",
                table: "FlowChartActions",
                newName: "CondestionAnwser");

            migrationBuilder.AddColumn<string>(
                name: "BodyMessage_A",
                table: "FlowChartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BodyMessage_E",
                table: "FlowChartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterMessage_A",
                table: "FlowChartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FooterMessage_E",
                table: "FlowChartDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BodyMessage_A",
                table: "FlowChartDetails");

            migrationBuilder.DropColumn(
                name: "BodyMessage_E",
                table: "FlowChartDetails");

            migrationBuilder.DropColumn(
                name: "FooterMessage_A",
                table: "FlowChartDetails");

            migrationBuilder.DropColumn(
                name: "FooterMessage_E",
                table: "FlowChartDetails");

            migrationBuilder.RenameColumn(
                name: "HeaderMessage_E",
                table: "FlowChartDetails",
                newName: "DisplayMessage_E");

            migrationBuilder.RenameColumn(
                name: "HeaderMessage_A",
                table: "FlowChartDetails",
                newName: "DisplayMessage_A");

            migrationBuilder.RenameColumn(
                name: "CondestionAnwser",
                table: "FlowChartActions",
                newName: "MultiAnwserExtraData");
        }
    }
}
