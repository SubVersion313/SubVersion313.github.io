namespace LodgeMasterWeb.Core.Models;

public class FlowChartMaster
{
    [Key]
    [MaxLength(250)]
    public string FlowchartID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string CompanyID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string FlowchartName { get; set; } = string.Empty;
    public int bActive { get; set; } = 1;

}
