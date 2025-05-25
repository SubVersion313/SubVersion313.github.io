using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class maigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WhatsappBotActions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FCDetailsID = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    Close = table.Column<bool>(type: "bit", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlowChartDetailFCDetailsID = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    OrderMasterOrderID = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatsappBotActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_FlowChartDetails_FCDetailsID",
                        column: x => x.FCDetailsID,
                        principalTable: "FlowChartDetails",
                        principalColumn: "FCDetailsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_FlowChartDetails_FlowChartDetailFCDetailsID",
                        column: x => x.FlowChartDetailFCDetailsID,
                        principalTable: "FlowChartDetails",
                        principalColumn: "FCDetailsID");
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_OrdersMaster_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrdersMaster",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WhatsappBotActions_OrdersMaster_OrderMasterOrderID",
                        column: x => x.OrderMasterOrderID,
                        principalTable: "OrdersMaster",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_ApplicationUserId",
                table: "WhatsappBotActions",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_FCDetailsID",
                table: "WhatsappBotActions",
                column: "FCDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_FlowChartDetailFCDetailsID",
                table: "WhatsappBotActions",
                column: "FlowChartDetailFCDetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_OrderId",
                table: "WhatsappBotActions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_OrderMasterOrderID",
                table: "WhatsappBotActions",
                column: "OrderMasterOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WhatsappBotActions_UserId",
                table: "WhatsappBotActions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WhatsappBotActions");
        }
    }
}
