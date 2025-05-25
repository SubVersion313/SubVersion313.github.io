namespace LodgeMasterWeb.Core.Models;

public class CompanyUnitFloor
{
    [Key]
    [MaxLength(450)]
    public string FloorId { get; set; }
    public string FloorName { get; set; }
    public string CompanyID { get; set; }
    public string BranchID { get; set; }
    public int iSorted { get; set; }
    public int bActive { get; set; } = 1;

}
