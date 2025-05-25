namespace LodgeMasterWeb.Core.ViewModels;

public class OrderDisplayVM
{

    public string? OrderID { get; set; } = string.Empty;
    public long Order_cd { get; set; }
    public string CurrentDate { get; set; } = string.Empty;
    public DateTime? dtCraeteStamp { get; set; } = GeneralFun.GetCurrentTime(); //
    //public string? LocationID { get; set; }
    //public string? DepartmentID { get; set; }
    public int StatusId { get; set; }
    public string? LocationName { get; set; }
    public string? StatusName { get; set; }
    public string? DepName { get; set; }
    public string? CreateName { get; set; }
    //public string? UserIDAssign { get; set; }
    public string? AssignName { get; set; }
    public string? DepartmentID { get; set; }
    public string? UserIDAssign { get; set; }
    public int DaysDifference { get; set; }
    public int RemainingHours { get; set; }
    public int StatusSortShow { get; set; }



}
