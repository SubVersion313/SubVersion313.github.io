namespace LodgeMasterWeb.chatbot.Responses;

public class EmptyResponse
{
    public class EmptyResponseDto
    {
    }

    public class MessageIdDto
    {
        public long MessageId { get; set; }
    }

    public class GetOtpMessageStatusResponseDto
    {
        public DateTime? SendDate { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
    }
}
