using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace LodgeMasterWeb.APIControllers
{
    [Route("api/Webhook")]
    [ApiController]
    [EnableCors("Allow4Whats")]
    [AllowAnonymous]
    public class Webhook1Controller : ControllerBase
    {
        private readonly string _INSTANCEId = "136623";
        private readonly string _APITOKEN = "32efea8e-89b4-4423-988d-11f5bdbd7bd1";
        private readonly IHttpClientFactory _clientFactory;

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;
        private readonly ILogger<Webhook1Controller> _logger;

        public Webhook1Controller(IHttpClientFactory clientFactory, ILogger<Webhook1Controller> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
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
        public async Task<ActionResult<WebHookData>> ReceiveMessage([FromBody] WebHookData payload)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                _logger.LogInformation("Received webhook: {0}", payload);

                if (payload != null)
                {
                    var OrdDetVM = new chatbothook
                    {
                        cbId = Guid.NewGuid().ToString(),
                        CompanyID = _CompanyID,
                        QuestionId = payload.QuestionId,
                        Question = payload.Question,
                        AnswerId = payload.AnswerId,
                        Answer = payload.Answer,
                        AnswerCount = payload.AnswerCount,
                        PhoneNumber = payload.PhoneNumber,
                    };

                    await _context.AddAsync(OrdDetVM);
                    await _context.SaveChangesAsync();
                }

                return Ok(payload);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing webhook: {Message}", ex.Message);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost("startMessage")]
        [AllowAnonymous]
        public async Task<IActionResult> StartMessage([FromBody] WebHookData payload)
        {
            try
            {
                var client = _clientFactory.CreateClient();

                string bodymsg = "مرحبا بكم يوجد طلب رقم 5050 للموافقة اضغط 1 او 2 للالغاء";

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("phone", "201017162758"),
                    new KeyValuePair<string, string>("body", bodymsg),
                    new KeyValuePair<string, string>("instanceid", _INSTANCEId),
                    new KeyValuePair<string, string>("token", _APITOKEN),
                });

                var response = await client.PostAsync("https://user.4whats.net/api/sendMessage", content);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Message sent successfully");

                    //string responseBody = await response.Content.ReadAsStringAsync();
                    //var json = JObject.Parse(responseBody);
                    //var message = json["message"]?.ToString();


                    //payload.Answer = message;

                    var webhookResponse = await ReceiveMessage(payload);

                    if (webhookResponse is WebHookData)

                        return Ok(new { success = true, returnData = "Message sent successfully!" });
                    else
                        return StatusCode(500, new { success = false, returnData = "Failed to process the response." });
                }
                else
                {
                    _logger.LogError("Failed to send message");
                    return BadRequest(new { success = false, returnData = "Failed to send message." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in startMessage: {Message}", ex.Message);
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }
}


//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;
//using System.Threading.Tasks;
//namespace LodgeMasterWeb.APIControllers;

//[Route("api/Webhook")]
//[ApiController]
//[EnableCors("Allow4Whats")]
//public class WebhookController : ControllerBase
//{
//    private readonly string _INSTANCEId = "136623";
//    private readonly string _APITOKEN = "32efea8e-89b4-4423-988d-11f5bdbd7bd1";
//    private readonly IHttpClientFactory _clientFactory;

//    private readonly ApplicationDbContext _context;
//    private readonly UserManager<ApplicationUser> _userManager;

//    private readonly IWebHostEnvironment _webHostEnvironment;
//    private readonly string _empImagePath;
//    private readonly ILogger<WebhookController> _logger;

//    public WebhookController(IHttpClientFactory clientFactory, ILogger<WebhookController> logger, ApplicationDbContext context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment webHostEnvironment)
//    {
//        _logger = logger;
//        _context = context;
//        _userManager = UserManager;
//        _webHostEnvironment = webHostEnvironment;
//        _empImagePath = _webHostEnvironment.WebRootPath;
//        _clientFactory = clientFactory;
//    }

//[HttpPost]
//public async Task<ActionResult<WebHookData>> Post([FromBody] WebHookData payload)
//{
//    try
//    {
//        var _CompanyID = HttpContext.Session.GetString("CompanyID");
//        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
//        var _UserID = HttpContext.Session.GetString("UserID");

//        _logger.LogInformation("Received webhook: {0}", payload);

//        if (payload != null)
//        {
//            var OrdDetVM = new chatbothook
//            {
//                cbId = Guid.NewGuid().ToString(),
//                CompanyID = _CompanyID,
//                QuestionId = payload.QuestionId,
//                Question = payload.Question,
//                AnswerId = payload.AnswerId,
//                Answer = payload.Answer,
//                AnswerCount = payload.AnswerCount,
//                PhoneNumber = payload.PhoneNumber,
//            };

//            await _context.AddAsync(OrdDetVM);
//            await _context.SaveChangesAsync();
//        }

//        return Ok(payload);
//    }
//    catch (Exception ex)
//    {
//        _logger.LogError("Error processing webhook: {Message}", ex.Message);
//        return StatusCode(500, new { error = "Internal server error", details = ex.Message });
//    }
//}

//[HttpPost("startMessage")]  //Request url: https://www.yourdomain.com/api/webhook/startMessage
////public async Task<IActionResult> startMessage([FromBody] object dstsObj)
//public async Task<IActionResult> startMessage([FromBody] WebHookData payload)
//{
//    //.Write("start");

//    //if (ModelState.IsValid)
//    //{
//    //Debug.Write("valid");
//    var client = _clientFactory.CreateClient();

//    string bodymsg = "مرحبا بكم يوجد طلب رقم 5050 للموافقة اضغط 1 او 2 للالغاء";

//    var content = new FormUrlEncodedContent(new[]
//    {
//        new KeyValuePair<string, string>("phone","201111235794"),
//        new KeyValuePair<string, string>("body", bodymsg),
//        new KeyValuePair<string, string>("instanceid", _INSTANCEId),
//        new KeyValuePair<string, string>("token", _APITOKEN),
//        // إضافة المعلمات الأخرى المطلوبة بواسطة API هنا
//    });

//    var response = await client.PostAsync("https://user.4whats.net/api/sendMessage", content);

//    if (response.IsSuccessStatusCode)
//    {
//        Debug.Write("1");
//        return Ok(new { success = true, returnData = "Message sent successfully!" });
//    }
//    else
//    {
//        Debug.Write("2");
//        return BadRequest(new { success = false, returnData = "Failed to send message." });
//    }


//}
//}
