namespace LodgeMasterWeb.Core.ViewModels
{
    public class TmEmpCreditVM
    {

        public string UFShiftId { get; set; }
        //public string CreditRoomId { get; set; }
        public string EmpId { get; set; }
        public string EmpName { get; set; } = string.Empty;
        public string UFShiftStatus { get; set; } = string.Empty;
        public int MaxCredits { get; set; } = 0;
        public int MaxZones { get; set; } = 0;
        public int RoomSets { get; set; } = 0;
        public int ShiftTypeId { get; set; } = 0;
        public int Weekend { get; set; } = 0;
        public string sNotes { get; set; } = string.Empty;


    }
}
