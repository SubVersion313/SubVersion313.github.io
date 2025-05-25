namespace LodgeMasterWeb.Core.Models
{
    public class RoomLinkTypePart
    {
        [Key]
        [MaxLength(250)]
        public string LinkTPID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string PartID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string QuestionID { get; set; } = string.Empty;
    }
}
