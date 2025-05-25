namespace LodgeMasterWeb.Core.Models
{
    public class InspectionDep

    {
        [Key]
        [MaxLength(250)]
        public string InspDepId { get; set; } = string.Empty;
        public string InspDepName { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public string InspInfoId { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; } = GeneralFun.GetCurrentTime(); //DateTime.Now;
        public string CreateEmpID { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public int IsDeleted { get; set; } = 0;
        public int iSorted { get; set; } = 0;
        public string DeleteEmpID { get; set; } = string.Empty;




    }
}
