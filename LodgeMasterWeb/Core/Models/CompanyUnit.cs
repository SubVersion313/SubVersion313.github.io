namespace LodgeMasterWeb.Core.Models
{
    public class CompanyUnit
    {
        [Key]
        [MaxLength(250)]
        public string LocationID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string LocationName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string LocDesc { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public string BranchID { get; set; } = string.Empty;
        public string FloorID { get; set; } = string.Empty;

        public int LocType { get; set; }
        public int LocGroup { get; set; }
        public string UnitCat { get; set; } = string.Empty;
        public string CreateEmpID { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;
        public int iSorted { get; set; }
        public int isDeleted { get; set; } = 0;
        public int StatusRoom { get; set; } = 0;

    }
}
