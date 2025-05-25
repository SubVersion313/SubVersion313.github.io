namespace LodgeMasterWeb.Core.Models
{
    public class CompanyBranche : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string BrancheID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string BrancheName { get; set; } = string.Empty;
        [MaxLength(500)]
        public string BrancheDesc { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;

    }
}
