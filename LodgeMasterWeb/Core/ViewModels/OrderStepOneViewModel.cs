
namespace LodgeMasterWeb.Core.ViewModels
{
    [Serializable]
    public class OrderStepOneViewModel
    {

        public string OrderID { get; set; } = string.Empty;
        public string? BasketID { get; set; }
        public string LocationID { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public int FromGuest { get; set; } = 0;
        public int Priority { get; set; } = 0;

        public IEnumerable<SelectListItem> LstLocations { get; set; } = Enumerable.Empty<SelectListItem>(); //new List<SelectListItem>();

        //public List<Tuple<string, int>> LstItemsQty { get; set; } = new List<Tuple<string, int>>();
        public List<OrderItemQtyAssign> OrderItems { get; set; } = new List<OrderItemQtyAssign>();

        public IEnumerable<SelectListItem> LstItems { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> LstDepartment { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> LstEmployees { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
