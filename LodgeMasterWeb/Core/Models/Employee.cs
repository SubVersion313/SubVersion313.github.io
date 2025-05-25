namespace LodgeMasterWeb.Core.Models
{
    public class Employee
    {

        [Key]
        public string EmpID { get; set; } = string.Empty;
        public int User_cd { get; set; }// = string.Empty;
        public string UserLogin { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string CompanyID { get; set; } = string.Empty;

        public string DepartmentID { get; set; } = string.Empty;
        //[ForeignKey(nameof(DepartmentID))]
        //public Department DepartmentData { get; set; }

        public string sEmail { get; set; } = string.Empty;

        public string CreateEmpID { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        //public byte[] bPhoto { get; set; } = null;
        public string bPhoto { get; set; } = string.Empty;
        public string Photopath { get; set; } = string.Empty;

        public int bActiveAccept { get; set; } = 0;
        public int LangDef { get; set; } = 0;
        public int expiredate { get; set; } = 0;
        public string mobile { get; set; } = string.Empty;
        public int supervisor { get; set; } = 0;
        public DateTime dtpasswordupdate { get; set; } = GeneralFun.GetCurrentTime(); //
        public int iSorted { get; set; }
        public int isDeleted { get; set; } = 0;

    }
}
