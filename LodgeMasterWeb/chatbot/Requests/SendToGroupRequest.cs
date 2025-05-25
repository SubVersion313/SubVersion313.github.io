namespace LodgeMasterWeb.chatbot.Requests;

public class SendToGroupRequest
{
    public long GroupId { get; set; }
    public string Message { get; set; }
    public string Base64Image { get; set; }
}
