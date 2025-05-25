namespace LodgeMasterWeb.Controllers;
public class TestChatController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _INSTANCEId = "136623";
    private readonly string _APITOKEN = "32efea8e-89b4-4423-988d-11f5bdbd7bd1";
    public TestChatController(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageModel model)
    {
        if (ModelState.IsValid)
        {
            var client = _clientFactory.CreateClient();

            var content = new FormUrlEncodedContent(new[]
            {
                         new KeyValuePair<string, string>("phone", model.PhoneNumber),
                         new KeyValuePair<string, string>("body", model.Message),
                         new KeyValuePair<string, string>("instanceid", _INSTANCEId),
                         new KeyValuePair<string, string>("token", _APITOKEN),
                         // إضافة المعلمات الأخرى المطلوبة بواسطة API هنا
                     });

            //var response = await client.PostAsync("https://api.4whats.net/send", content);
            var response = await client.PostAsync("https://user.4whats.net/api/sendMessage", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Message sent successfully!";
            }
            else
            {
                ViewBag.Message = "Failed to send message.";
            }
        }

        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] WebhookPayload payload)
    {
        //_logger.LogInformation("Received webhook: {0}", payload);

        // قم بمعالجة البيانات الواردة هنا
        // ...

        // payload.payload
        return Ok();
    }
    public class WebhookPayload
    {
        // public string Message { get; set; }
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
}
