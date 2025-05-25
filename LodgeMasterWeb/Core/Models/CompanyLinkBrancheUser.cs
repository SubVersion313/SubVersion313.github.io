namespace LodgeMasterWeb.Core.Models
{
    public class CompanyLinkBrancheUser
    {
        [Key]
        [MaxLength(250)]
        public string CompanyLinkUTBID { get; set; } = string.Empty;
        [MaxLength(450)]
        public string UserID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string BrancheID { get; set; } = string.Empty;
    }
}
