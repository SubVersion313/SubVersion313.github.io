namespace LodgeMasterWeb.Core.ViewModels
{
    public class OrderStepTowViewModel
    {
        public string? LocationID { get; set; }
        public string? LocationName { get; set; }
        public string OrderID { get; set; } = string.Empty;

        public List<OrderItemQtyAssign> OrderItems { get; set; } = new List<OrderItemQtyAssign>();
        public IEnumerable<SelectListItem> LstDepartment { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> LstEmployees { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
