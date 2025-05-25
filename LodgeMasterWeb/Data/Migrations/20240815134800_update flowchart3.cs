using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class updateflowchart3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CondestionAnwser",
                table: "FlowChartActions",
                newName: "conditionAnwser");

            migrationBuilder.AddColumn<int>(
                name: "isNextAnwser",
                table: "FlowChartActions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNextAnwser",
                table: "FlowChartActions");

            migrationBuilder.RenameColumn(
                name: "conditionAnwser",
                table: "FlowChartActions",
                newName: "CondestionAnwser");
        }
    }
}
