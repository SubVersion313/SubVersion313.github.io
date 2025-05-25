namespace LodgeMasterWeb.Core.Models
{
    public class InspectionLinkInspLocation
    {

        [Key]
        [MaxLength(250)]
        public string LinkInspLocID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string InspectionId { get; set; } = string.Empty;
        [MaxLength(250)]
        public string LocationId { get; set; } = string.Empty;
        public int bActive { get; set; } = 1;

    }
}
