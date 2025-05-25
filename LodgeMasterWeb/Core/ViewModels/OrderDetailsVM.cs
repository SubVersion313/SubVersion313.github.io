namespace LodgeMasterWeb.Core.ViewModels
{
    public class OrderDetailsVM : VMOrders
    {
        public string? dtCraeteDisplay { get; set; } = string.Empty;
        public string? AtTime { get; set; }

        public string? UserIDAction { get; set; }
        public string? AssigneesActionName { get; set; }
        public string? AssigneesActionPic { get; set; }

        public string? RunTime { get; set; }
        public string? bgClass { get; set; } = string.Empty;

        public int CountCurrent { get; set; }
        public int CountItems { get; set; }
        public int Percent { get; set; }
        public int ForSuperviser { get; set; }

        [ForeignKey("OrderID")]
        public IEnumerable<OrdersDetailItems> OrderDetItem { get; set; }
        [ForeignKey("OrderID")]
        public IEnumerable<OrderActionViewModel> OrderAction { get; set; }
        [ForeignKey("OrderID")]
        public IEnumerable<AttachmentFile> MistakePics { get; set; }


    }
}
