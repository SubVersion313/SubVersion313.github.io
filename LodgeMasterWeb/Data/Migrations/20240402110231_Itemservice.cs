using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class Itemservice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "UserCreate",
            //    table: "ItemServices");

            migrationBuilder.AddColumn<int>(
                name: "IsDeleted",
                table: "ItemServices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ItemServices");

            //migrationBuilder.AddColumn<string>(
            //    name: "UserCreate",
            //    table: "ItemServices",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }
    }
}
