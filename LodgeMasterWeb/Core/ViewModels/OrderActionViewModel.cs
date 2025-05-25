namespace LodgeMasterWeb.Core.ViewModels
{
    public class OrderActionViewModel
    {
        public long SerialID { get; set; }
        public string? SerGUID { get; set; } = string.Empty;
        public string OrderID { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public DateTime dtAction { get; set; } = GeneralFun.GetCurrentTime(); //
        public string? sNotes { get; set; } = string.Empty;
        public int Satatus { get; set; }
        public string? UserIDAction { get; set; } = string.Empty;
        public string? ToEmp { get; set; } = string.Empty;
        public string? ToDepartment { get; set; } = string.Empty;

        public string? DepName_E { get; set; }
        public string? DepName_A { get; set; }
        //public int isDeleted { get; set; }= 0;
        public string? CreatedName { get; set; }
        public string? CreatedDep { get; set; }
        public string? AssignName { get; set; }
        public string? AssignDep { get; set; }
        public int DaysDifference { get; set; }
        public int RemainingHours { get; set; }
        public string? CurrentDate { get; set; }
    }
}

