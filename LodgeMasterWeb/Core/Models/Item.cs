namespace LodgeMasterWeb.Core.Models
{
    public class Item : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string ItemID { get; set; } = string.Empty;
        public int Item_cd { get; set; }
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepartmentID { get; set; } = string.Empty;
        [ForeignKey("DepartmentID")]
        public Department DepartmentData { get; set; }
        public int ItemType { get; set; } = 0;
        public int ItemStock { get; set; }
        public int isService { get; set; } = 0;
        public int minQty { get; set; } = 0;
        public int Qty { get; set; } = 1;
        public int isQty { get; set; } = 0;
        public int priorityOrder { get; set; } = 0;
        [MaxLength(250)]
        public string ItemName_E { get; set; } = string.Empty;
        [MaxLength(250)]
        public string ItemName_A { get; set; } = string.Empty;

        public int iSorted { get; set; } = 0;
        public int isDefault { get; set; }
        public string ItemIDDefault { get; set; } = string.Empty;
        public string? UserCreate { get; set; } = string.Empty;
        public int isInspection { get; set; } = 1;

    }
}
