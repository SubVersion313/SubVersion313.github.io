namespace LodgeMasterWeb.Core.Models
{
    public class InspectionMaster
    {
        [Key]
        [MaxLength(250)]
        public string InspectionID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string InspInfoId { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        // public int dtEntry { get; set; } = 0;
        [MaxLength(250)]
        public string LocationID { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public string CreateEmpID { get; set; } = string.Empty;
        public string EmpDepId { get; set; } = string.Empty;
        public string statusName { get; set; } = string.Empty;
        public int isDeleted { get; set; }
        //public DateTime LastUpdate { get; set; }
    }
}
