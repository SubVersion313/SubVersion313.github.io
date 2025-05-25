using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LodgeMasterWeb.Data.Migrations
{
    public partial class StartBuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserLogins");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserTokens");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            //migrationBuilder.DropIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims");

            //migrationBuilder.AlterColumn<string>(
            //    name: "RoleId",
            //    table: "AspNetRoleClaims",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "AttachmentFiles",
                columns: table => new
                {
                    FileID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bShow = table.Column<int>(type: "int", nullable: false),
                    bStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentFiles", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "BasketTemps",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasketID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpAssignID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketTemps", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyName_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyName_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Companylogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MasterEmail = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyFolder = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    CreateCompany = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<long>(type: "bigint", nullable: false),
                    EndDate = table.Column<long>(type: "bigint", nullable: false),
                    CounterUsers = table.Column<int>(type: "int", nullable: false),
                    isDemo = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyBarnches",
                columns: table => new
                {
                    BrancheID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BrancheName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BrancheDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyBarnches", x => x.BrancheID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyLinkBranches",
                columns: table => new
                {
                    LinkCBID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BrancheID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyLinkBranches", x => x.LinkCBID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyUnits",
                columns: table => new
                {
                    LocationID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocType = table.Column<int>(type: "int", nullable: false),
                    LocGroup = table.Column<int>(type: "int", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUnits", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepName_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepName_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmpID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_cd = table.Column<int>(type: "int", nullable: false),
                    UserLogin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    bPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Photopath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActiveAccept = table.Column<int>(type: "int", nullable: false),
                    LangDef = table.Column<int>(type: "int", nullable: false),
                    expiredate = table.Column<int>(type: "int", nullable: false),
                    mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supervisor = table.Column<int>(type: "int", nullable: false),
                    dtpasswordupdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmpID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionBaskets",
                columns: table => new
                {
                    InspectionBasketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionGUID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    dtEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAnswer = table.Column<int>(type: "int", nullable: false),
                    CommetAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrntQuestion = table.Column<int>(type: "int", nullable: false),
                    SortQuestion = table.Column<int>(type: "int", nullable: false),
                    QuestionNo = table.Column<int>(type: "int", nullable: false),
                    QuestionTotal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionBaskets", x => x.InspectionBasketId);
                });

            migrationBuilder.CreateTable(
                name: "InspectionDeps",
                columns: table => new
                {
                    InspDepId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspDepName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspInfoId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionDeps", x => x.InspDepId);
                });

            migrationBuilder.CreateTable(
                name: "InspectionInfos",
                columns: table => new
                {
                    InspInfoId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspDesc = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    InspToCreateOrder = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionInfos", x => x.InspInfoId);
                });

            migrationBuilder.CreateTable(
                name: "InspectionLinkInspLocations",
                columns: table => new
                {
                    LinkInspLocID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspectionId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocationId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionLinkInspLocations", x => x.LinkInspLocID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionLinkPartQuestions",
                columns: table => new
                {
                    LinkPQID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspectionId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PartID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isPublish = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionLinkPartQuestions", x => x.LinkPQID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionQuestions",
                columns: table => new
                {
                    QuestionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuestionDisplay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionQuestions", x => x.QuestionID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionsDet",
                columns: table => new
                {
                    DetailID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InspectionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuestionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuestionDisplay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    UserAnswer = table.Column<int>(type: "int", nullable: false),
                    CommetAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicBefore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicBeforCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PicAfter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicAfterCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bDone = table.Column<int>(type: "int", nullable: false),
                    DoneCreate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionsDet", x => x.DetailID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionsMaster",
                columns: table => new
                {
                    InspectionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    InspInfoId = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocationID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpDepId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    statusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDeleted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionsMaster", x => x.InspectionID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionWizardVM",
                columns: table => new
                {
                    InspectionBasketId = table.Column<int>(type: "int", nullable: false),
                    InspectionGUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dtEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InspectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAnswer = table.Column<int>(type: "int", nullable: false),
                    CommetAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PicAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrntQuestion = table.Column<int>(type: "int", nullable: false),
                    SortQuestion = table.Column<int>(type: "int", nullable: false),
                    QuestionNo = table.Column<int>(type: "int", nullable: false),
                    QuestionTotal = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ItemsLinkDepartment",
                columns: table => new
                {
                    LinkItemDepID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemsLinkDepartment", x => x.LinkItemDepID);
                });

            migrationBuilder.CreateTable(
                name: "Levels",
                columns: table => new
                {
                    LevelID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LevelName_EN = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LevelName_AR = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    isDefault = table.Column<int>(type: "int", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.LevelID);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iParentID = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    sURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Ben = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Ind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iFormID = table.Column<int>(type: "int", nullable: false),
                    sIcon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "OrdersAction",
                columns: table => new
                {
                    SerialID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerGUID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OrderID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    dtAction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Satatus = table.Column<int>(type: "int", nullable: false),
                    UserIDAction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToEmp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDepartment = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersAction", x => x.SerialID);
                });

            migrationBuilder.CreateTable(
                name: "OrdersDet",
                columns: table => new
                {
                    Order_Det = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    sItemNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isClosed = table.Column<int>(type: "int", nullable: false),
                    dtColsed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserClosed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersDet", x => x.Order_Det);
                });

            migrationBuilder.CreateTable(
                name: "RoomsLinkTypeComUnit",
                columns: table => new
                {
                    LinkTCUID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LocationID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RoomTypeID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsLinkTypeComUnit", x => x.LinkTCUID);
                });

            migrationBuilder.CreateTable(
                name: "RoomsLinkTypePart",
                columns: table => new
                {
                    LinkTPID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PartID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    QuestionID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsLinkTypePart", x => x.LinkTPID);
                });

            migrationBuilder.CreateTable(
                name: "RoomsPart",
                columns: table => new
                {
                    PartsID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PartName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsPart", x => x.PartsID);
                });

            migrationBuilder.CreateTable(
                name: "RoomsType",
                columns: table => new
                {
                    RoomTypeID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsType", x => x.RoomTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SysItems",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemName_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemName_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    DefaultID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysItems", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "SysMenus",
                columns: table => new
                {
                    MenuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iParentID = table.Column<int>(type: "int", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    sURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Ben = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MenuText_Ind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    iFormID = table.Column<int>(type: "int", nullable: false),
                    sIcon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenus", x => x.MenuID);
                });

            migrationBuilder.CreateTable(
                name: "SysProblem",
                columns: table => new
                {
                    iProblem_Code = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    sProblem_Description_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sProblem_Description_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sProblem_Description_Ben = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sProblem_Description_Nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sProblem_Description_Ind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sRecommended_Solution_E = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sRecommended_Solution_A = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sRecommended_Solution_Ben = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sRecommended_Solution_Nep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sRecommended_Solution_Ind = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysProblem", x => x.iProblem_Code);
                });

            migrationBuilder.CreateTable(
                name: "SysStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Status_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    StatusActive = table.Column<int>(type: "int", nullable: false),
                    SatausParent = table.Column<int>(type: "int", nullable: false),
                    StatusSortShow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "TempFiles",
                columns: table => new
                {
                    FileID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    OrderID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bShow = table.Column<int>(type: "int", nullable: false),
                    bStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempFiles", x => x.FileID);
                });

            migrationBuilder.CreateTable(
                name: "UnitsGroup",
                columns: table => new
                {
                    GroupID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    GroupName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsGroup", x => x.GroupID);
                });

            migrationBuilder.CreateTable(
                name: "UnitsType",
                columns: table => new
                {
                    UnitID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitsType", x => x.UnitID);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Item_cd = table.Column<int>(type: "int", nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemType = table.Column<int>(type: "int", nullable: false),
                    ItemStock = table.Column<int>(type: "int", nullable: false),
                    isService = table.Column<int>(type: "int", nullable: false),
                    minQty = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    isQty = table.Column<int>(type: "int", nullable: false),
                    priorityOrder = table.Column<int>(type: "int", nullable: false),
                    ItemName_E = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ItemName_A = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    iSorted = table.Column<int>(type: "int", nullable: false),
                    isDefault = table.Column<int>(type: "int", nullable: false),
                    ItemIDDefault = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateEmpID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    bActive = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<int>(type: "int", nullable: false),
                    DeleteEmpID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_Items_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrdersMaster",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CompanyID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Order_cd = table.Column<long>(type: "bigint", nullable: false),
                    dtCraete = table.Column<int>(type: "int", nullable: false),
                    dtCraeteStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DepartmentID = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UserIDCreate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CatID = table.Column<int>(type: "int", nullable: false),
                    StampAssign = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserIDAssign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentAssignUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeptIDAssign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForSuperviser = table.Column<int>(type: "int", nullable: false),
                    LinkData = table.Column<int>(type: "int", nullable: false),
                    bDelay = table.Column<int>(type: "int", nullable: false),
                    DelayTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdersMaster", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_OrdersMaster_CompanyUnits_LocationID",
                        column: x => x.LocationID,
                        principalTable: "CompanyUnits",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersMaster_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdersMaster_SysStatus_Status",
                        column: x => x.Status,
                        principalTable: "SysStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_DepartmentID",
                table: "Items",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersMaster_DepartmentID",
                table: "OrdersMaster",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersMaster_LocationID",
                table: "OrdersMaster",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersMaster_Status",
                table: "OrdersMaster",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachmentFiles");

            migrationBuilder.DropTable(
                name: "BasketTemps");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "CompanyBarnches");

            migrationBuilder.DropTable(
                name: "CompanyLinkBranches");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "InspectionBaskets");

            migrationBuilder.DropTable(
                name: "InspectionDeps");

            migrationBuilder.DropTable(
                name: "InspectionInfos");

            migrationBuilder.DropTable(
                name: "InspectionLinkInspLocations");

            migrationBuilder.DropTable(
                name: "InspectionLinkPartQuestions");

            migrationBuilder.DropTable(
                name: "InspectionQuestions");

            migrationBuilder.DropTable(
                name: "InspectionsDet");

            migrationBuilder.DropTable(
                name: "InspectionsMaster");

            migrationBuilder.DropTable(
                name: "InspectionWizardVM");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemsLinkDepartment");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "OrdersAction");

            migrationBuilder.DropTable(
                name: "OrdersDet");

            migrationBuilder.DropTable(
                name: "OrdersMaster");

            migrationBuilder.DropTable(
                name: "RoomsLinkTypeComUnit");

            migrationBuilder.DropTable(
                name: "RoomsLinkTypePart");

            migrationBuilder.DropTable(
                name: "RoomsPart");

            migrationBuilder.DropTable(
                name: "RoomsType");

            migrationBuilder.DropTable(
                name: "SysItems");

            migrationBuilder.DropTable(
                name: "SysMenus");

            migrationBuilder.DropTable(
                name: "SysProblem");

            migrationBuilder.DropTable(
                name: "TempFiles");

            migrationBuilder.DropTable(
                name: "UnitsGroup");

            migrationBuilder.DropTable(
                name: "UnitsType");

            migrationBuilder.DropTable(
                name: "CompanyUnits");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "SysStatus");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
