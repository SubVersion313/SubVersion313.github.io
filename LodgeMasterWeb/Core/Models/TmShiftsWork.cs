namespace LodgeMasterWeb.Core.Models;

public class TmShiftsWork : BaseModel
{
    [Key]
    [MaxLength(250)]
    public string ShiftID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string ShiftName_E { get; set; } = string.Empty;
    [MaxLength(250)]
    public string ShiftName_A { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;

    [MaxLength(250)]
    public string CompanyID { get; set; } = string.Empty;
    
    public string MasterNameDefault { get; set; } = string.Empty;
    public int isDefault { get; set; } = 0;
    public int iSorted { get; set; } = 0;

}
