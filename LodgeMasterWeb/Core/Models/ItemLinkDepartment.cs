namespace LodgeMasterWeb.Core.Models
{
    public class ItemLinkDepartment
    {
        [Key]
        [MaxLength(250)]
        public string LinkItemDepID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepartmentID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string ItemID { get; set; } = string.Empty;
    }
}
