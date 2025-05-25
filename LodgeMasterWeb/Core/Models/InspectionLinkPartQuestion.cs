namespace LodgeMasterWeb.Core.Models
{
    public class InspectionLinkPartQuestion
    {
        [Key]
        [MaxLength(250)]
        public string LinkPQID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string InspectionId { get; set; } = string.Empty;
        public string PartID { get; set; } = string.Empty;
        public string QuestionID { get; set; } = string.Empty;
        public int isPublish { get; set; } = 0;
        public int isDeleted { get; set; } = 0;
        public int iSorted { get; set; } = 0;


    }
}
