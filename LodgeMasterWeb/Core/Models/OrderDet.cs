namespace LodgeMasterWeb.Core.Models
{
    public class OrderDet
    {
        [Key]
        public long Order_Det { get; set; }
        [MaxLength(250)]
        public string OrderID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string ItemID { get; set; } = string.Empty;
        public int Qty { get; set; } = 0;
        public string sItemNotes { get; set; } = string.Empty;
        public int isClosed { get; set; } = 0;
        public DateTime? dtColsed { get; set; }
        public string? UserClosed { get; set; } = string.Empty;
        public string? PhotoName { get; set; } = string.Empty;
    }
}
