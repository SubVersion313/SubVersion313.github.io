using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class orderdetailitems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_WhatsappBotActions_AspNetUsers_ApplicationUserId",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_WhatsappBotActions_FlowChartDetails_FlowChartDetailFCDetailsID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_WhatsappBotActions_OrdersMaster_OrderMasterOrderID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropIndex(
            //    name: "IX_WhatsappBotActions_ApplicationUserId",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropIndex(
            //    name: "IX_WhatsappBotActions_FlowChartDetailFCDetailsID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropIndex(
            //    name: "IX_WhatsappBotActions_OrderMasterOrderID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropColumn(
            //    name: "ApplicationUserId",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropColumn(
            //    name: "FlowChartDetailFCDetailsID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.DropColumn(
            //    name: "OrderMasterOrderID",
            //    table: "WhatsappBotActions");

            //migrationBuilder.AddColumn<int>(
            //    name: "Sorted",
            //    table: "WhatsappBotActions",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sorted",
                table: "WhatsappBotActions");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "WhatsappBotActions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FlowChartDetailFCDetailsID",
                table: "WhatsappBotActions",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderMasterOrderID",
                table: "WhatsappBotActions",
                type: "nvarchar(250)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_ApplicationUserId",
                table: "WhatsappBotActions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_FlowChartDetailFCDetailsID",
                table: "WhatsappBotActions",
                column: "FlowChartDetailFCDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_OrderMasterOrderID",
                table: "WhatsappBotActions",
                column: "OrderMasterOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_WhatsappBotActions_AspNetUsers_ApplicationUserId",
                table: "WhatsappBotActions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WhatsappBotActions_FlowChartDetails_FlowChartDetailFCDetailsID",
                table: "WhatsappBotActions",
                column: "FlowChartDetailFCDetailsID",
                principalTable: "FlowChartDetails",
                principalColumn: "FCDetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_WhatsappBotActions_OrdersMaster_OrderMasterOrderID",
                table: "WhatsappBotActions",
                column: "OrderMasterOrderID",
                principalTable: "OrdersMaster",
                principalColumn: "OrderID");
        }
    }
}
