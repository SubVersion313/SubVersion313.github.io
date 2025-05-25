namespace LodgeMasterWeb.Core.Models;

public class GoalMaster
{
    [Key]
    [MaxLength(250)]
    public string GoalId { get; set; }
    public string CompanyID { get; set; }
    public string BranchID { get; set; }
    public DateTime dt_stamp { get; set; }
    public string EmpID { get; set; }


}
