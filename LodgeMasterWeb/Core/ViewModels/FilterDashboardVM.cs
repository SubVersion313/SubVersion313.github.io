namespace LodgeMasterWeb.Core.ViewModels
{
    public class FilterDashboardVM
    {
        public int StatusOpen { get; set; }
        public int StatusHold { get; set; }
        public int StatusClosed { get; set; }
        public int StatusInprocess { get; set; }
        public int StatusCompleted { get; set; }
        public string OrdernumberFrom { get; set; }
        public string OrdernumberTo { get; set; }
        public string RuntimeFrom { get; set; }
        public string RuntimeTo { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string textSearch { get; set; }

    }
}
