namespace LodgeMasterWeb.Core.ViewModels
{
    public class ItemViewModel
    {
        public long BasketOrderId { get; set; } = 0;
        public string? BasketID { get; set; }
        public string? OldItemID { get; set; }
        public string? OldItemName { get; set; }
        public int? OldItemQty { get; set; }
        public int? NewItemQty { get; set; }

        public string? OldDepartmentID { get; set; }
        public string? OldDepartmentName { get; set; }
        public string? NewDepartmentID { get; set; }
        public string? NewDepartmentName { get; set; }

        public string? OldEmpAssigneeID { get; set; }
        public string? OldEmpAssigneeName { get; set; }
        public string? NewEmpAssigneeID { get; set; }
        public string? NewEmpAssigneeName { get; set; }

        public IEnumerable<SelectListItem> LstItems { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> LstDepartment { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> LstEmpAssignee { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
