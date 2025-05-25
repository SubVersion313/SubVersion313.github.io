//using DotVVM.Framework.Compilation.ControlTree;

namespace LodgeMasterWeb.Core.ViewModels
{
    public class DashboardViewModel : DashboardCount
    {

        public string? OrderID { get; set; }
        public long Order_cd { get; set; }
        public string? CompanyID { get; set; }
        public int dtCraete { get; set; }
        public DateTime dtCraeteStamp { get; set; } = GeneralFun.GetCurrentTime();
        public string? LocationID { get; set; }
        public string? LocationName { get; set; }
        public string? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? ItemsName { get; set; }
        public string UserIDAssign { get; set; }
        public string? EmpAssignName { get; set; }
        public string? RunTime { get; set; }
        public int StatusID { get; set; }
        public string? StatusName { get; set; }
        public string? sNotes { get; set; }
        public string UserIDCreate { get; set; }
        public string CreateName { get; set; }


    }
}
