
namespace LodgeMasterWeb.Core.Models;

public class TmCreditRoom
{
    [Key]
    [MaxLength(250)]
    public string CreditRoomId { get; set; }
    [MaxLength(250)]
    public string CompanyID { get; set; } = string.Empty;
    public string EmpId { get; set; }
    public string UFShiftStatus { get; set; }
    public int MaxCredits { get; set; }=0;
    public int MaxZones { get; set; }=0;
    public int RoomSets { get; set; }=0;
}
