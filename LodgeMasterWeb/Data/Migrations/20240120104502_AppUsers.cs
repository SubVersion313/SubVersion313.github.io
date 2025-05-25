using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class AppUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_DepartmentAssignUserId",
            //    table: "OrdersMaster");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_UserIDAssign",
            //    table: "OrdersMaster");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_UserIDCreate",
            //    table: "OrdersMaster");

            //migrationBuilder.DropTable(
            //    name: "CompanyLinkBranches");

            //migrationBuilder.DropIndex(
            //    name: "IX_OrdersMaster_DepartmentAssignUserId",
            //    table: "OrdersMaster");

            //migrationBuilder.DropIndex(
            //    name: "IX_OrdersMaster_UserIDAssign",
            //    table: "OrdersMaster");

            //migrationBuilder.DropIndex(
            //    name: "IX_OrdersMaster_UserIDCreate",
            //    table: "OrdersMaster");

            migrationBuilder.DropColumn(
                name: "bytePhoto",
                table: "AspNetUsers");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserIDCreate",
            //    table: "OrdersMaster",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserIDAssign",
            //    table: "OrdersMaster",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "DepartmentAssignUserId",
            //    table: "OrdersMaster",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            //migrationBuilder.CreateTable(
            //    name: "CompanyLinkBrancheUsers",
            //    columns: table => new
            //    {
            //        CompanyLinkUTBID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
            //        UserID = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
            //        BrancheID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CompanyLinkBrancheUsers", x => x.CompanyLinkUTBID);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "CompanyLinkBrancheUsers");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserIDCreate",
            //    table: "OrdersMaster",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "UserIDAssign",
            //    table: "OrdersMaster",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.AlterColumn<string>(
            //    name: "DepartmentAssignUserId",
            //    table: "OrdersMaster",
            //    type: "nvarchar(450)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "bytePhoto",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            //migrationBuilder.CreateTable(
            //    name: "CompanyLinkBranches",
            //    columns: table => new
            //    {
            //        LinkCBID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
            //        BrancheID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
            //        CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_CompanyLinkBranches", x => x.LinkCBID);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrdersMaster_DepartmentAssignUserId",
            //    table: "OrdersMaster",
            //    column: "DepartmentAssignUserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrdersMaster_UserIDAssign",
            //    table: "OrdersMaster",
            //    column: "UserIDAssign");

            //migrationBuilder.CreateIndex(
            //    name: "IX_OrdersMaster_UserIDCreate",
            //    table: "OrdersMaster",
            //    column: "UserIDCreate");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_DepartmentAssignUserId",
            //    table: "OrdersMaster",
            //    column: "DepartmentAssignUserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_UserIDAssign",
            //    table: "OrdersMaster",
            //    column: "UserIDAssign",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_OrdersMaster_AspNetUsers_UserIDCreate",
            //    table: "OrdersMaster",
            //    column: "UserIDCreate",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
