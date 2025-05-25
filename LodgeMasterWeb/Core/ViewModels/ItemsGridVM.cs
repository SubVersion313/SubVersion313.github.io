//using DotVVM.Framework.Controls;

namespace LodgeMasterWeb.Core.ViewModels
{
    public class ItemsGridVM
    {
        public string? ItemID { get; set; } = string.Empty;
        public int Item_cd { get; set; } //= string.Empty;
        public string? CompanyID { get; set; } = string.Empty;
        public string DepartmentID { get; set; } = string.Empty;
        public int ItemType { get; set; }
        public int ItemStock { get; set; }

        public string ItemName_E { get; set; } = string.Empty;
        public string ItemName_A { get; set; } = string.Empty;
        public int bActive { get; set; }
        public int iSorted { get; set; }
        public int isDeleted { get; set; }
        public int isDefault { get; set; }
        public string ItemIDDefault { get; set; } = string.Empty;

        public string? UserCreate { get; set; } = string.Empty;
        public int isService { get; set; } = 0;
        public int minQty { get; set; } = 0;
        public int Qty { get; set; } = 1;
        public string DepartmentName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public string CreateName { get; set; } = string.Empty;

        public int isInspection { get; set; } = 0;
        public IEnumerable<SelectListItem> LstDepartment { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
