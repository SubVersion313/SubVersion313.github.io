using Newtonsoft.Json.Linq;

namespace LodgeMasterWeb.Core.ViewModels;

public class Sender
{
    public Sender(string json)
    {
        JObject jObject = JObject.Parse(json);
        status = jObject["status"]!.ToString();
        comment = jObject["comment"]!.ToString();
    }
    public string status { get; set; }
    public string comment { get; set; }

    public static async Task<string> SendWhatsapp(string receive, string message, int count)
    {
        using (var client = new HttpClient())
        {
            string link = $"https://api.textmebot.com/send.php?recipient=+{receive}&apikey=1gJahvwZrQjD&text={message}&json=yes";
            string status = string.Empty, comment = string.Empty;
            for (int i = 1; i <= count; i++)
            {
                string json = await client.GetStringAsync(link);
                Sender sender = new Sender(json);
                status = sender.status;
                comment = sender.comment;
                if (sender.status == "success")
                {
                    break;
                }
                await Task.Delay(5000);
            }
            if (status != "success")
            {
                return comment;
            }
            return status;
        }
    }
}
