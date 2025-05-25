namespace LodgeMasterWeb.Core.ViewModels
{
    public class OrdersDetailItems
    {
        public long Order_Det { get; set; }
        public string OrderID { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public string ItemID { get; set; } = string.Empty;
        public int Qty { get; set; } = 0;
        public string sItemNotes { get; set; } = string.Empty;
        public int isClosed { get; set; } = 0;
        public DateTime? dtColsed { get; set; } = GeneralFun.GetCurrentTime(); //
        public string? UserClosed { get; set; } = string.Empty;
        public string DepartmentID { get; set; } = string.Empty;
        public string? DepartmentItemName { get; set; } = string.Empty;
        public int ItemType { get; set; } = 0;
        public int ItemStock { get; set; } = 0;
        public string? ItemName_E { get; set; } = string.Empty;
        public string? ItemName_A { get; set; } = string.Empty;

        public string? CompanyFolder { get; set; } = string.Empty;
        public string? SubFolder { get; set; } = string.Empty;
        public string? PicName { get; set; } = string.Empty;

    }
}
