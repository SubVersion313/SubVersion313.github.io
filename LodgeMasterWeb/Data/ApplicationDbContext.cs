using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LodgeMasterWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyBranche> CompanyBarnches { get; set; }
        public DbSet<CompanyLinkBrancheUser> CompanyLinkBrancheUsers { get; set; }
        public DbSet<CompanyUnit> CompanyUnits { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public DbSet<InspectionDet> InspectionsDet { get; set; }
        public DbSet<InspectionMaster> InspectionsMaster { get; set; }
        public DbSet<InspectionLinkPartQuestion> InspectionLinkPartQuestions { get; set; }
        public DbSet<InspectionQuestion> InspectionQuestions { get; set; }
        public DbSet<InspectionDep> InspectionDeps { get; set; }
        public DbSet<InspectionInfo> InspectionInfos { get; set; }
        public DbSet<InspectionBasket> InspectionBaskets { get; set; }
        public DbSet<InspectionLinkInspLocation> InspectionLinkInspLocations { get; set; }


        public DbSet<Item> Items { get; set; }
        public DbSet<ItemService> ItemServices { get; set; }
        public DbSet<ItemLinkDepartment> ItemsLinkDepartment { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<OrderMaster> OrdersMaster { get; set; }
        public DbSet<OrderDet> OrdersDet { get; set; }
        public DbSet<OrderAction> OrdersAction { get; set; }
        public DbSet<RoomLinkTypeComUnit> RoomsLinkTypeComUnit { get; set; }
        public DbSet<RoomLinkTypePart> RoomsLinkTypePart { get; set; }
        public DbSet<RoomPart> RoomsPart { get; set; }
        public DbSet<RoomType> RoomsType { get; set; }
        //public DbSet<userRole> UserRoles { get; set; }

        public DbSet<sys_Item> SysItems { get; set; }
        public DbSet<sys_Menu> SysMenus { get; set; }
        public DbSet<sys_Problem> SysProblem { get; set; }
        //public DbSet<sys_Role> SysRoles { get; set; }
        public DbSet<sys_Status> SysStatus { get; set; }
        public DbSet<TempFile> TempFiles { get; set; }
        public DbSet<UnitGroup> UnitsGroup { get; set; }
        public DbSet<UnitType> UnitsType { get; set; }

        public DbSet<BasketTemp> BasketTemps { get; set; }
        public DbSet<VMOrders> Vmorders { get; set; }
        public DbSet<VMInspMaster> VMInspMasters { get; set; }
        public DbSet<ItemsGridVM> VMItems { get; set; }
        public DbSet<UsersViewModel> UsersVM { get; set; }
        public DbSet<OrderActionViewModel> OrderActionVM { get; set; }
        public DbSet<InspectionWizardViewModel> InspectionWizardVM { get; set; }
        public DbSet<VMLinkQuestion> VMLinkQuestions { get; set; }
        public DbSet<CompanyUnitCat> CompanyUnitsCat { get; set; }
        public DbSet<CompanyUnitCatType> CompanyUnitsCatType { get; set; }
        public DbSet<CompanyUnitLinkCatType> CompanyUnitsLinkCatType { get; set; }
        public DbSet<CompanyUnitFloor> CompanyUnitFloors { get; set; }
        public DbSet<chatbothook> chatbothooks { get; set; }
        //public object InspectionQutions { get; internal set; }
        public DbSet<AvgTicketsViewModel> AvgTicketsVM { get; set; }
        public DbSet<TotalItemsViewModel> TotalItemsVM { get; set; }
        public DbSet<FlowChartMaster> FlowChartMasters { get; set; }
        public DbSet<FlowChartDetail> FlowChartDetails { get; set; }
        public DbSet<FlowChartAction> FlowChartActions { get; set; }
        public DbSet<GoalMaster> GoalsMaster { get; set; }
        public DbSet<GoalDetail> GoalsDetail { get; set; }
        public DbSet<StaffSchedule> StaffsSchedule { get; set; }

        public DbSet<ChartLineVM> ChartLinesVM { get; set; }
        public DbSet<ChartLineWeekVM> ChartLinesWeekVM { get; set; }
        public DbSet<ChartLineMonthVM> ChartLinesMonthVM { get; set; }
        

        public DbSet<ColorCategory> ColorsCategory { get; set; }
        public DbSet<VMOrderDetItem> VMOrderDetItems { get; set; }

        public DbSet<WhatsappBotAction> WhatsappBotActions { get; set; }
        //time Managemte Tables
        public DbSet<TmCreditRoom> TmCreditRooms { get; set; }
        public DbSet<TmShiftsWork> TmShiftsWorks { get; set; }
        public DbSet<TmShiftWeek> TmShiftWeeks { get; set; }
        public DbSet<TmEmpCredit> TmEmployeesCredit { get; set; }

        public DbSet<TmSys_Service> TmSys_Services { get; set; }
        public DbSet<TmWeekMaster> TmWeeksMaster { get; set; }
        public DbSet<TmWeekDetails> TmWeeksDetails { get; set; }
        public DbSet<TmWeekTasks> TmWeeksTasks { get; set; }
        //#region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<BasketTemp>()
            //        .HasNoKey();
            modelBuilder.Entity<InspectionBasket>()
                .HasKey(i => i.InspectionBasketId);

            modelBuilder.Entity<VMOrders>().HasNoKey();
            modelBuilder.Entity<VMOrders>().ToView("VMOrders");

            modelBuilder.Entity<AvgTicketsViewModel>().HasNoKey();
            modelBuilder.Entity<AvgTicketsViewModel>().ToView("VMAvgTickets");

            modelBuilder.Entity<TotalItemsViewModel>().HasNoKey();
            modelBuilder.Entity<TotalItemsViewModel>().ToView("q_OrderDet");

            modelBuilder.Entity<VMInspMaster>().HasNoKey();
            modelBuilder.Entity<VMInspMaster>().ToView("VMInspMaster");

            modelBuilder.Entity<ItemsGridVM>().HasNoKey();
            modelBuilder.Entity<ItemsGridVM>().ToView("VMItems");

            modelBuilder.Entity<UsersViewModel>().HasNoKey();
            modelBuilder.Entity<UsersViewModel>().ToView("VMUsers");

            modelBuilder.Entity<OrderActionViewModel>().HasNoKey();
            modelBuilder.Entity<OrderActionViewModel>().ToView("VMOrderAction");

            modelBuilder.Entity<InspectionWizardViewModel>().HasNoKey();
            //modelBuilder.Entity<InspectionWizardViewModel>().ToView("VMLinkQuestions"); 

            modelBuilder.Entity<VMLinkQuestion>().HasNoKey();
            modelBuilder.Entity<VMLinkQuestion>().ToView("VMLinkQuestions");

            modelBuilder.Entity<ChartLineVM>().HasNoKey();
            modelBuilder.Entity<ChartLineVM>().ToView("VMChartLine");
            
            modelBuilder.Entity<ChartLineWeekVM>().HasNoKey();
            modelBuilder.Entity<ChartLineWeekVM>().ToView("VMChartLineWeek");

            modelBuilder.Entity<ChartLineMonthVM>().HasNoKey();
            modelBuilder.Entity<ChartLineMonthVM>().ToView("VMChartLineMonth");
            
            modelBuilder.Entity<VMOrderDetItem>().HasNoKey();
            modelBuilder.Entity<VMOrderDetItem>().ToView("q_OrderDetItems");
            //modelBuilder.Entity<InspectionBasket>().HasNoKey();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Ignore<IdentityUserLogin<string>>();
            //modelBuilder.Ignore<IdentityUserRole<string>>();
            //modelBuilder.Ignore<IdentityUserClaim<string>>();
            //modelBuilder.Ignore<IdentityUserToken<string>>();
            //modelBuilder.Ignore<IdentityUser<string>>();
            //modelBuilder.Ignore<ApplicationUser>();
            //modelBuilder.Ignore<ApplicationRole>();


            // علاقة بين TmWeekMaster و TmWeekDetails
            modelBuilder.Entity<TmWeekDetails>()
                .HasOne(d => d.TmWeekMaster)
                .WithMany()  // لا توجد مجموعة لتخزين التفاصيل داخل TmWeekMaster
                .HasForeignKey(d => d.WeekMsID)
                .IsRequired();

            // علاقة بين TmWeekDetails و TmWeekTasks
            modelBuilder.Entity<TmWeekTasks>()
                .HasOne(t => t.TmWeekDetails)
                .WithMany()  // لا توجد مجموعة لتخزين المهام داخل TmWeekDetails
                .HasForeignKey(t => t.TmDet_ID)
                .IsRequired();


            modelBuilder.Entity<WhatsappBotAction>().HasKey(wb => wb.Id);

            modelBuilder.Entity<WhatsappBotAction>()
                .HasOne(fd => fd.FlowChartDetail)
                .WithMany(fd => fd.WhatsappBotActions)
                .HasForeignKey(fd => fd.FCDetailsID);

            modelBuilder.Entity<WhatsappBotAction>()
                .HasOne(fd => fd.OrderMaster)
                .WithMany(fd => fd.WhatsappBotActions)
                .HasForeignKey(fd => fd.OrderId);

            modelBuilder.Entity<WhatsappBotAction>()
                .HasOne(fd => fd.ApplicationUser)
                .WithMany(fd => fd.WhatsappBotActions)
                .HasForeignKey(fd => fd.UserId);

        }
        //    modelBuilder.Entity<AttachmentFile>()
        //        .Property(b => b.FileID)
        //        .IsRequired();
        //    modelBuilder.Entity<AttachmentFile>()
        //        .Property(b => b.CompanyID)
        //        .IsRequired();

        //    modelBuilder.Entity<CompanyUnit>()
        //        .Property(b => b.LocationID)
        //        .IsRequired();

        //    modelBuilder.Entity<Department>()
        //        .Property(b => b.DepartmentID)
        //        .IsRequired();

        //    modelBuilder.Entity<Department>()
        //        .Property(b => b.CompanyID)
        //        .IsRequired();


        //    modelBuilder.Entity<Employee>()
        //        .Property(b => b.EmpID)
        //        .IsRequired();

        //    modelBuilder.Entity<Employee>()
        //        .Property(b => b.UserLogin)
        //        .IsRequired();

        //    modelBuilder.Entity<Employee>()
        //        .Property(b => b.CompanyID)
        //        .IsRequired();

        //    modelBuilder.Entity<InspectionMaster>()
        //     .Property(b => b.InspectionID)
        //     .IsRequired();
        //}
        //#endregion
    }
}
