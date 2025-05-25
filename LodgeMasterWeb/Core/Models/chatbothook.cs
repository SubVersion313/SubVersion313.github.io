namespace LodgeMasterWeb.Core.Models;

public class chatbothook : BaseModel
{
    [Key]
    public string cbId { get; set; }
    public string CompanyID { get; set; }
    public string PhoneNumber { get; set; }
    public int QuestionId { get; set; }
    public string Question { get; set; }
    public int AnswerId { get; set; }
    public int AnswerCount { get; set; }
    public string Answer { get; set; }
}
