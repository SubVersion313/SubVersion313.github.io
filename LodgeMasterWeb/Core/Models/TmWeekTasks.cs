namespace LodgeMasterWeb.Core.Models;

public class TmWeekTasks
{
    [Key]
    public string TmWeek_DetID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string TmDet_ID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string CompanyID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string ItemID { get; set; } = string.Empty;
    public int Qty { get; set; } = 1;
    public string sItemNotes { get; set; } = string.Empty;
    public int isTransOrder { get; set; } = 0;
    public DateTime? dtTransOrder { get; set; }
    public int iSorted { get; set; } = 0;

    [ForeignKey("TmDet_ID")]
    public TmWeekDetails? TmWeekDetails { get; set; }
}
