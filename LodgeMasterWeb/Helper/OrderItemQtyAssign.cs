namespace LodgeMasterWeb.Helper
{
    public class OrderItemQtyAssign
    {
        public int OrderId { get; set; } = 0;
        public string? ItemID { get; set; }
        public string? ItemName { get; set; }
        public int qty { get; set; } = 0;
        public string? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }

        public string EmpAssignID { get; set; } = string.Empty;
        public string EmpAssignName { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> LstEmpDepartment { get; set; } = Enumerable.Empty<SelectListItem>();

        public string? EmployeeID { get; set; }
        //public string? EmployeeName { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; } = string.Empty;
        public string? BasketID { get; set; }

    }
}
