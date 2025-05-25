namespace LodgeMasterWeb.Core.Models
{
    public class OrderAction
    {
        [Key]
        public long SerialID { get; set; }
        [MaxLength(250)]
        public string SerGUID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string OrderID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public DateTime dtAction { get; set; } = GeneralFun.GetCurrentTime();
        public string sNotes { get; set; } = string.Empty;
        public int Satatus { get; set; }
        public string UserIDAction { get; set; } = string.Empty;
        public string ToEmp { get; set; } = string.Empty;
        public string ToDepartment { get; set; } = string.Empty;
    }
}

