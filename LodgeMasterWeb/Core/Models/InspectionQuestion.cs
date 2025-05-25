namespace LodgeMasterWeb.Core.Models
{
    public class InspectionQuestion : BaseModel
    {
        [Key]
        [MaxLength(250)]
        public string QuestionID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        // public int Question_cd { get; set; } //= string.Empty;
        public string QuestionDisplay { get; set; } = string.Empty;


    }
}
