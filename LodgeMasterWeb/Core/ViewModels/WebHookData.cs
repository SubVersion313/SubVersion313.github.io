namespace LodgeMasterWeb.Core.ViewModels;

public class WebHookData
{
    //public string Message { get; set; }
    public string PhoneNumber { get; set; }
    public string InstanceId { get; set; }
    public string ApiToken { get; set; }
    public int QuestionId { get; set; }
    public string Question { get; set; }
    public int AnswerId { get; set; }
    public int AnswerCount { get; set; }
    public string Answer { get; set; }

    // أضف الحقول الأخرى بناءً على ما يتم إرساله من 4whats.net
}
