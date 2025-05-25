using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace LodgeMasterWeb.APIControllers;
[Route("api/Webhook")]
[ApiController]
[AllowAnonymous]
public class WebhookController : ControllerBase
{
    private readonly string _INSTANCEId = "136623";
    private readonly string _APITOKEN = "32efea8e-89b4-4423-988d-11f5bdbd7bd1";
    private readonly IHttpClientFactory _clientFactory;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<Webhook1Controller> _logger;
    private readonly string _empImagePath;

    public WebhookController(IHttpClientFactory clientFactory, ILogger<Webhook1Controller> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _empImagePath = _webHostEnvironment.WebRootPath;
        _clientFactory = clientFactory;
    }


    [HttpPost]
    [AllowAnonymous]
    public IActionResult ReceiveMessage([FromBody] JObject payload)
    {
        // Log the incoming payload (for debugging)
        Console.WriteLine(payload.ToString());

        // Parse the payload to extract relevant information
        var messages = payload["entry"]?[0]?["changes"]?[0]?["value"]?["messages"];
        if (messages != null)
        {
            foreach (var message in messages)
            {
                var messageType = message["type"]?.ToString();

                if (messageType == "button")
                {
                    // Handle button click
                    var buttonPayload = message["interactive"]["button_reply"]["payload"]?.ToString();
                    var buttonText = message["interactive"]["button_reply"]["title"]?.ToString();

                    // Based on the button payload or text, decide the next step
                    if (buttonPayload == "next_step")
                    {
                        // Send the next conversation step
                        SendMessage("Next step message");
                    }
                }
                else if (messageType == "text")
                {
                    var userText = message["text"]["body"]?.ToString();
                    // Handle text messages
                }
            }
        }

        return Ok();
    }
    private void SendMessage(string message)
    {
        // Logic to send a message via WhatsApp Cloud API
        // You'll need to use HttpClient to send the next message based on button click
    }

    private async Task SendMessageAsync(string recipientPhoneNumber, string message)
    {
        using (var client = new HttpClient())
        {
            var url = "https://graph.facebook.com/v15.0/{your_whatsapp_number_id}/messages";
            var content = new
            {
                messaging_product = "whatsapp",
                to = recipientPhoneNumber,
                type = "text",
                text = new { body = message }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer {your_access_token}");
            var response = await client.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Message sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send message.");
            }
        }
    }
}
