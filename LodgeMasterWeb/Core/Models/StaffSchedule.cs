namespace LodgeMasterWeb.Core.Models;

public class StaffSchedule
{
    [Key]
    [MaxLength(250)]
    public string SSId { get; set; }
    public string CompanyID { get; set; }
    public string BranchID { get; set; }
    public string EmpID { get; set; }
    public DateTime dt_stamp { get; set; }

    public int CurrntDate { get; private set; } = 0;
    public int CurrntDay { get; private set; } = 0;
    public int CurrntMonth { get; private set; } = 0;
    public int CurrntYear { get; private set; } = 0;

}
