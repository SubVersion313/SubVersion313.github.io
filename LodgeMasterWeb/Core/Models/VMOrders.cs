//using DotVVM.Framework.Controls;

namespace LodgeMasterWeb.Core.Models
{
    public class VMOrders
    {
        [MaxLength(250)]
        public string OrderID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string? CompanyID { get; set; }
        public string? CompanyFolder { get; set; } = string.Empty;
        public long Order_cd { get; set; }
        public int? dtCraete { get; set; }
        public string CurrentDate { get; set; } = string.Empty;
        public DateTime? dtCraeteStamp { get; set; } = GeneralFun.GetCurrentTime(); //
        public string? LocationID { get; set; }
        public string? DepartmentID { get; set; }
        public string? sNotes { get; set; }
        public int StatusId { get; set; }
        public string? LocationName { get; set; }
        public string? StatusName { get; set; }
        public string? DepName { get; set; }
        public string? UserIDCreate { get; set; }
        public string? CreateName { get; set; }
        public string? CreatePic { get; set; }
        public string? UserIDAssign { get; set; }
        public string? AssignName { get; set; }
        public string? AssignPic { get; set; }
        public string? DepartmentAssignUserId { get; set; }
        public string? DepartmentAssignName { get; set; }
        public string? DepartmentAssignPic { get; set; }
        public DateTime DelayTime { get; set; } //= GeneralFun.GetCurrentTime(); //
        public int DaysDifference { get; set; }
        public int RemainingHours { get; set; }
        public int StatusSortShow { get; set; }
        public int isInspection { get; set; }
        public int ForSuperviser { get; set; }
        public int TestRecord { get; set; }
        public int SupervisorUserAssign { get; set; }
        public int FromGuest { get; set; } = 0;
 
    }
}
