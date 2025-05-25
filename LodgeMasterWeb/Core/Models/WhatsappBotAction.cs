namespace LodgeMasterWeb.Core.Models;

public class WhatsappBotAction
{
    [Key]
    [MaxLength(250)]
    public string Id { get; set; } = string.Empty;
    public string FCDetailsID { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public DateTime dtcreated { get; set; } = GeneralFun.GetCurrentTime();

    public bool Close { get; set; } = false;
    public string? Answer { get; set; } = null;
    public int Sorted { get; set; } = 1;

    [ForeignKey("FCDetailsID")]
    public FlowChartDetail? FlowChartDetail { get; set; }

    [ForeignKey("OrderId")]
    public OrderMaster? OrderMaster { get; set; }

    [ForeignKey("UserId")]
    public ApplicationUser? ApplicationUser { get; set; }
}
