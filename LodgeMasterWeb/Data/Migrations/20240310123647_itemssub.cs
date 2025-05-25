using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class itemssub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemServices",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemIDSub = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    UserCreate = table.Column<string>(type: "nvarchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemServices", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemServices");
        }
    }
}
