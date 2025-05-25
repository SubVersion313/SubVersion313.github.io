namespace LodgeMasterWeb.Core.Models;

public class TmWeekDetails
{

    [Key]
    [MaxLength(250)]
    public string TmDet_ID { get; set; }
    [MaxLength(250)]
    public string WeekMsID { get; set; } = string.Empty;
    [MaxLength(250)]
    public string CompanyID { get; set; } = string.Empty;
    public int dtCraete { get; set; } = 0;
    public string DayNo { get; set; } = string.Empty;
    public string MonthNo { get; set; } = string.Empty;
    public string YearNo { get; set; } = string.Empty;

    public int StatusDay { get; set; } = 0;//NoTask=0;Weeknd=1;Vacation=2
    public string DayNotes { get; set; } = string.Empty;

    [ForeignKey("WeekMsID")]
    public TmWeekMaster? TmWeekMaster { get; set; }


}
