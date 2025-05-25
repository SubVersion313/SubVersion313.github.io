namespace LodgeMasterWeb.chatbot;

public class BaseRequest<TRequest>
{
    public BaseRequest()
    {

    }
    public BaseRequest(TRequest data)
    {
        this.data = data;
    }
    public TRequest data { get; set; }
}
