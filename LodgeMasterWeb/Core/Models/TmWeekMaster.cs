namespace LodgeMasterWeb.Core.Models
{
    public class TmWeekMaster
    {
        [Key]
        [MaxLength(250)]
        public string WeekMsID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public string EmployeeID { get; set; } = string.Empty;
        public int dtCraete { get; set; } = 0;
        public DateTime dtCraeteStamp { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public int WeekNo { get; set; } = 0;
        public int WeekYear { get; set; } = 0;
        public string DepartmentID { get; set; } = string.Empty;
        public string UserIDCreate { get; set; } = string.Empty;
        public string sNotes { get; set; } = string.Empty;

    }
}
