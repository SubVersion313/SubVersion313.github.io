namespace LodgeMasterWeb.Core.Models;

public class GoalDetail
{
    [Key]
    [MaxLength(250)]
    public string GoalDetailsID { get; set; }
    public string GoalId { get; set; } = string.Empty;

    public string CompanyUnitId { get; set; } = string.Empty;
    public string Fo { get; set; }=string.Empty;
    public decimal CR { get; set; } = 0;
    public string VIP { get; set; } = string.Empty;
    public string TT { get; set; } = string.Empty;


}
