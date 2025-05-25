namespace LodgeMasterWeb.Core.Models
{
    public class OrderMaster
    {
        [Key]
        [MaxLength(250)]
        public string OrderID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public long Order_cd { get; set; } = 0;
        public int dtCraete { get; set; } = 0;
        public DateTime dtCraeteStamp { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        [MaxLength(250)]
        public string LocationID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepartmentID { get; set; } = string.Empty;
        public string UserIDCreate { get; set; } = string.Empty;
        public string sNotes { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
        public int CatID { get; set; } = 0;
        public DateTime StampAssign { get; set; } = GeneralFun.GetCurrentTime();
        public string UserIDAssign { get; set; } = string.Empty;
        public string DepartmentAssignUserId { get; set; } = string.Empty;
        public string DeptIDAssign { get; set; } = string.Empty;
        public int ForSuperviser { get; set; } = 0;
        public int LinkData { get; set; } = 0;
        public int bDelay { get; set; } = 0;
        public DateTime DelayTime { get; set; } = GeneralFun.GetCurrentTime();

        public int isInspection { get; set; } = 0;
        // Navigation properties
        [ForeignKey("LocationID")]
        public CompanyUnit? Location { get; set; }

        [ForeignKey("Status")]
        public sys_Status? SysStatus { get; set; }

        [ForeignKey("DepartmentID")]
        public Department? Department { get; set; }

        public int TestRecord { get; set; } = 0;
        public int FromGuest { get; set; } = 0;
        public int Priority { get; set; } = 0;

        //[ForeignKey("UserIDCreate")]
        ////public employee UserCreated { get; set; }
        //public ApplicationUser? UserCreated { get; set; }

        //[ForeignKey("UserIDAssign")]
        ////public Employee UserAssigned { get; set; } 
        //public ApplicationUser? UserAssigned { get; set; } 

        //[ForeignKey("DepartmentAssignUserId")]
        ////public Employee DepartmentAssignUser { get; set; }
        //public ApplicationUser? DepartmentAssignUser { get; set; }

        public ICollection<WhatsappBotAction>? WhatsappBotActions { get; set; }
    }
}
