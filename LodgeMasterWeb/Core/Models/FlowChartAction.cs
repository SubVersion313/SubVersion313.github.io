namespace LodgeMasterWeb.Core.Models;

public class FlowChartAction
{
    [Key]
    [MaxLength(250)]
    public string FCActionID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string FCDetailsID { get; set; } = string.Empty;
    public int NoAnswer { get; set; } = 0;
    public string DisplayAnswer { get; set; } = string.Empty;
    public string ActionAnwser { get; set; } = string.Empty;
    public string conditionAnwser { get; set; } = string.Empty;
    public int isNextAnwser { get; set; } = 0;
    public int bActive { get; set; } = 1;
    public int Sorted { get; set; } = 0;

}
