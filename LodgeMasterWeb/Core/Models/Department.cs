namespace LodgeMasterWeb.Core.Models
{
    public class Department : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string DepartmentID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepName_E { get; set; } = string.Empty;
        [MaxLength(250)]
        public string DepName_A { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public string MasterNameDefault { get; set; } = string.Empty;
        public int isDefault { get; set; } = 0;
        public int iSorted { get; set; } = 0;

    }
}
