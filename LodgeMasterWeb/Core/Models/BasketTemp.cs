namespace LodgeMasterWeb.Core.Models
{
    public class BasketTemp
    {
        [Key]
        public int OrderId { get; set; }
        public string? BasketID { get; set; }
        public string? ItemID { get; set; }
        // public string Item_cd { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int Qty { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
        //public int iSorted { get; set; } = 0;
        public string CompanyID { get; set; } = string.Empty;
        public string DepartmentID { get; set; } = string.Empty;
        public string EmpAssignID { get; set; } = string.Empty;

        public string EmpID { get; set; } = string.Empty;


    }
}
