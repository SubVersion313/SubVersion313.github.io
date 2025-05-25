namespace LodgeMasterWeb.Core.Models;

public class TmShiftWeek
{
    [Key]
    [MaxLength(250)]
    public string ShiftWeekId { get; set; } = string.Empty;
    public string CreditRoomId { get; set; }
    public string EmpId { get; set; }
    public int dt_Start { get; set; } = 0;
    public int dt_End { get; set; } = 0;
    public string UFShiftStatus { get; set; }
    public int MaxCredits { get; set; } = 0;
    public int MaxZones { get; set; } = 0;
    public int RoomSets { get; set; } = 0;
    public int ShiftTypeId { get; set; } = 0;
    public int Weekend { get; set; } = 0;
    public string sNotes { get; set; } = string.Empty;
}
