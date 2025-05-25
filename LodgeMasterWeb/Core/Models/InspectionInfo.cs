namespace LodgeMasterWeb.Core.Models
{
    public class InspectionInfo
    {
        [Key]
        [MaxLength(250)]
        public string InspInfoId { get; set; } = string.Empty;
        [MaxLength(250)]
        public string InspName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string InspDesc { get; set; } = string.Empty;
        public string DepartmentID { get; set; } = string.Empty;
        public int InspToCreateOrder { get; set; } = 0;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public string CreateEmpID { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public int IsDeleted { get; set; } = 0;
        public string DeleteEmpID { get; set; } = string.Empty;
    }
}
