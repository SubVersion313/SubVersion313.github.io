namespace LodgeMasterWeb.Core.Models;

public class FlowChartDetail
{
    [Key]
    [MaxLength(250)]
    public string FCDetailsID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string FlowchartID { get; set; } = string.Empty;
    public string HeaderMessage_E { get; set; } = string.Empty;
    public string BodyMessage_E { get; set; } = string.Empty;
    public string FooterMessage_E { get; set; } = string.Empty;
    public string HeaderMessage_A { get; set; } = string.Empty;
    public string BodyMessage_A { get; set; } = string.Empty;
    public string FooterMessage_A { get; set; } = string.Empty;
    public int MultiAnwser { get; set; } = 0;
    public string ActionAnwser { get; set; } = string.Empty;
    public string CondationAnwser { get; set; } = string.Empty;
    public int bActive { get; set; } = 1;
    public int Sorted { get; set; } = 0;

    public ICollection<WhatsappBotAction>? WhatsappBotActions { get; set; }

}
