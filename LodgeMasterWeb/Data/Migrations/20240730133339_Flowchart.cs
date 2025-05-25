using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class Flowchart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowChartActions",
                columns: table => new
                {
                    FCActionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FCDetailsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NoAnswer = table.Column<int>(type: "int", nullable: false),
                    DisplayAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionAnwser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultiAnwserExtraData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    Sorted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowChartActions", x => x.FCActionID);
                });

            migrationBuilder.CreateTable(
                name: "FlowChartDetails",
                columns: table => new
                {
                    FCDetailsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FlowchartID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DisplayMessage_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayMessage_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultiAnwser = table.Column<int>(type: "int", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    Sorted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowChartDetails", x => x.FCDetailsID);
                });

            migrationBuilder.CreateTable(
                name: "FlowChartMasters",
                columns: table => new
                {
                    FlowchartID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FlowchartName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowChartMasters", x => x.FlowchartID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowChartActions");

            migrationBuilder.DropTable(
                name: "FlowChartDetails");

            migrationBuilder.DropTable(
                name: "FlowChartMasters");
        }
    }
}
