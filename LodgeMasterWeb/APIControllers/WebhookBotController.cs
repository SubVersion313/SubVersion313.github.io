//using DocumentFormat.OpenXml.Drawing.Charts;
using LodgeMasterWeb.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace LodgeMasterWeb.APIControllers;


[Route("webhookbot")]
[ApiController]
[AllowAnonymous]
public class WebhookBotController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    //private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<WebhookBotController> _logger;
  
    private readonly WhatsappOrder _WhatsappOrder;
    //private readonly string _empImagePath;
    //apikey=1gJahvwZrQjD
    public WebhookBotController(WhatsappOrder whatsappOrder, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, ILogger<WebhookBotController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        //_webHostEnvironment = webHostEnvironment;
       // _empImagePath = _webHostEnvironment.WebRootPath;
        _clientFactory = clientFactory;
        _WhatsappOrder = whatsappOrder;
        _httpContextAccessor = httpContextAccessor;
    }

    //Such Data
    /*
         {
        "type": "text",
        "from": "549191919191.",
        "from_name": "Joan",
        "to": "54134123123",
        "file": "null",
        "message": "Text Message received in WhatsApp"
        }
     */

    [HttpPost]
    public async Task<IActionResult> ReceiveWebhook()
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            string _CompanyID = "";// "fab9e047-8111-4af6-b4ec-0ea6e9c7ad3e";
            string _DepartmentID = "";
            using (StreamReader reader = new StreamReader(Request.Body))
            {
                string jsonContent = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(jsonContent)!;

                string message = data["message"].ToString();
                string sender = data["from"].ToString();

                if (message != null || message != string.Empty)
                {
                    string number = sender.Substring(3);
                    //للتحقق من المستخدم المسجل في الموقع
                    var user = await _context.Users.FirstOrDefaultAsync(f => f.PhoneNumber == number);//&& f.CompanyID == _CompanyID);
                    if (user == null)
                    {
                        //await Sender.SendWhatsapp(sender, "Not found user", 3);
                        return StatusCode(403);
                    }
                    string moveUserId = string.Empty;

                    //جلب العمليات والاسئلة خاص لهذا المستخدم
                    var whatsappBotAction = await _context.WhatsappBotActions
                            .Include(i => i.FlowChartDetail)
                            .Include(i => i.OrderMaster)
                            .Where(f => f.UserId == user.Id && f.Close == false)
                            .OrderBy(o => o.Sorted)
                            .FirstOrDefaultAsync();

                    if (whatsappBotAction != null)
                    {
                        //Company ID 
                        _CompanyID = whatsappBotAction.CompanyId;
                        _DepartmentID = whatsappBotAction.OrderMaster!.DepartmentID;
                        //تخزين تفاصيل السؤال
                        var flowChartDetail = whatsappBotAction.FlowChartDetail!;
                        int FCDetailsSorted = flowChartDetail.Sorted, answer = 0;

                        //اذا لم يكون سؤال مقالي
                        if (FCDetailsSorted != 6)
                        {
                            if (int.TryParse(message, out int awr))
                            {
                                if (awr < 1 || awr > 9) //if (answer < 1 && answer > 9)
                                {
                                    return StatusCode(403);
                                }
                                answer = awr;
                            }
                            else
                            {
                                return StatusCode(403);
                            }
                        }
                        else
                        {
                            //اذا كان سؤال مقالي أو نصي يتم تخزينه كما هو
                            whatsappBotAction.Answer = message;
                            whatsappBotAction.Close = true;
                            await _context.SaveChangesAsync();

                            //جلب رقم المعرف للمستخدم صاحب منشئ الطلب
                            var userOwner = await _userManager.FindByIdAsync(whatsappBotAction.OrderMaster!.UserIDCreate);
                            string result = $"The Order {whatsappBotAction.OrderMaster!.Order_cd} holded\n{message}.\n";

                            await _WhatsappOrder.HoldOrder(whatsappBotAction.OrderId, user.Id , _CompanyID, message);

                            //رسالة للمستخدم
                            await Sender.SendWhatsapp(sender, result, 3);

                            //رسالة للأدمن الطلب
                            await Sender.SendWhatsapp("966" + userOwner.PhoneNumber, $"{result} by user {user.FirstName} {user.LastName}.", 3);

                           // await _WhatsappOrder.HoldOrder(whatsappBotAction.OrderId, user.Id , _CompanyID);
                            
                            return Ok();
                        }

                        //هنا سؤال لنقل الى موظف معين
                        if (flowChartDetail.MultiAnwser < 0)
                        {
                           
                            if (flowChartDetail.Sorted == 2)
                            {
                                
                                //قائمة الموظفين بالترتيب حصراً عبر
                                //Counter
                                //من اجل اختيار الموظف بناء على رقم وهو Counter
                                var users = await _context.Users
                                .Where(w => w.DepartmentID == _DepartmentID && w.bActive == 1 && w.IsDeleted == 0 && w.CompanyID == _CompanyID)
                                .Take(9)
                                .ToListAsync();

                                var listEmployee = users.Select((u, index) => new
                                {
                                    ID = u.Id,
                                    Counter = index + 1,
                                    FullName = $"{u.FirstName} {u.LastName}",
                                    Phone = u.PhoneNumber
                                }).ToList();

                                //جلب الموظف على حسب الترتيب
                                var employeeDetails = listEmployee.FirstOrDefault(e => e.Counter == answer);

                                if (employeeDetails != null)
                                {
                                    if (employeeDetails.Phone == null)
                                    {
                                        await Sender.SendWhatsapp(sender, "The selected user does not have a number.", 3);
                                        return Ok();
                                    }
                                    //رسالة لصاحب الطلب
                                    //await Sender.SendWhatsapp(sender, $"Order {whatsappBotAction.OrderMaster!.Order_cd} has been successfully transferred to a user {employeeDetails.FullName}.", 3);

                                    //تم نقل الى موظف التي تم اختياره من قائمة الترتيب
                                    //ثم متابعة الرسالة او  السؤال التالي الى  هذا الموظف
                                    moveUserId = employeeDetails.ID;
                                    sender = "966" + employeeDetails.Phone;

                                    whatsappBotAction.Answer = answer.ToString() + " - " + employeeDetails.FullName;
                                    whatsappBotAction.Close = true;
                                    await _context.SaveChangesAsync();
                                    FCDetailsSorted++;

                                    await _WhatsappOrder.ChangeEmployee(whatsappBotAction.OrderId, user.Id,  moveUserId, _CompanyID);

                                    //رسالة لصاحب الطلب
                                    await Sender.SendWhatsapp(sender, $"Order {whatsappBotAction.OrderMaster!.Order_cd} has been successfully transferred to a user {employeeDetails.FullName}.", 3);
                                }
                                else
                                {
                                    await Sender.SendWhatsapp(sender, "Not found username", 3);
                                    return Ok();
                                }
                            }
                            else if (flowChartDetail.Sorted == 7)
                            {
                                var depts = await _context.Departments
                                            .Where(w => w.CompanyID == _CompanyID 
                                                   && w.IsDeleted == 0 && w.bActive == 1)
                                            .Take(9)
                                            .ToListAsync();

                                var listDept = depts.Select((u, index) => new
                                {
                                    Id = u.DepartmentID,
                                    Counter = index + 1,
                                    Dname = $"{u.DepName_E}"
                                }).ToList();

                                var departmentDetails = listDept.FirstOrDefault(e => e.Counter == answer);
                                //تحديث الطلب الى قسم اخر
                                if (departmentDetails != null)
                                {
                                    var orderMaster = whatsappBotAction.OrderMaster!;
                                    orderMaster.DepartmentID = departmentDetails.Id;
                                    _context.OrdersMaster.Update(orderMaster);

                                    whatsappBotAction.Answer = answer.ToString() + " - " + departmentDetails.Dname;
                                    whatsappBotAction.Close = true;

                                    await _context.SaveChangesAsync();
                                    
                                    await Sender.SendWhatsapp(sender, $"The order {orderMaster.Order_cd} transferred to {departmentDetails.Dname} department.", 3);

                                    await _WhatsappOrder.ChangeDepartment(whatsappBotAction.OrderId, user.Id, departmentDetails.Id,_CompanyID);
                                    return Ok();
                                }
                            }
                        }
                        else
                        {
                            //اذا لم يكون السؤال لاختيار الموظفين او يحتوى على أجوبة لهذا السؤال
                            var flowChartActions = _context.FlowChartActions.Where(f => f.FCDetailsID == whatsappBotAction.FCDetailsID);
                           
                            if (flowChartActions != null)
                            {
                                //اذا كانت الإجابة موجودة في السؤال
                                bool exists = await flowChartActions.AnyAsync(a => a.Sorted == answer);
                                if (!exists)
                                {
                                    //اذا كانت الإجابة غير موجودة في هذا السؤال
                                    await Sender.SendWhatsapp(sender, "The answer number is not available for question.", 3);
                                    return Ok();
                                }
                                else
                                {
                                    //اذا كانت الإجابة موجودة يتم تخزينها مباشرة
                                    whatsappBotAction.Answer = answer.ToString();
                                    whatsappBotAction.Close = true;
                                    await _context.SaveChangesAsync();

                                    //هنا لجلب الأكشن عبرالجواب
                                    var flowChartAction = await flowChartActions.FirstAsync(a => a.Sorted == answer);

                                    //اذا كان هذا السؤال للموظفين حصراً أي من ثالث سؤال فما بعد
                                    if (FCDetailsSorted >= 3)
                                    {
                                        //لمعرفة اذا تم الرفض
                                        if (flowChartAction.DisplayAnswer == "Reject")
                                        {
                                            //يتم ارجاع السؤال الى صاحب أو منشئ الطلب
                                            moveUserId = whatsappBotAction.OrderMaster!.UserIDCreate;
                                            var userOwner = await _userManager.FindByIdAsync(moveUserId);
                                            sender = "966" + userOwner.PhoneNumber;

                                            //رسالة تدل على رفض الطلب من قبل هذا الموظف
                                            await Sender.SendWhatsapp(sender, $"The Order {whatsappBotAction.OrderMaster!.Order_cd} rejeted by user {user.FirstName} {user.LastName}.", 3);
                                           
                                            await _WhatsappOrder.ReopenOrder(whatsappBotAction.OrderId, user.Id, _CompanyID);
                                            //ارجاع ترتيب السؤال
                                            FCDetailsSorted--;
                                        }
                                        else
                                        {
                                            //هنا اذا لم يقم بالرفض ف لدينا عدة اجابات ومنها (القبول او تأجيل أو تعليق أو إكمال) الطلب

                                            //جلب رقم المعرف مالك الطلب
                                            //من أجل آخذ رقم الهاتف لارسال له رسالة حالة الطلب
                                            var userOwner = await _userManager.FindByIdAsync(whatsappBotAction.OrderMaster!.UserIDCreate);

                                            //تحقق من الاجابة
                                            switch (flowChartAction.DisplayAnswer)
                                            {
                                                //ان كان مقبول او مؤجل فسوف يزيد ترتيب السؤال بمقدار واحد
                                                case "Take":
                                                    await _WhatsappOrder.TakeOrder(whatsappBotAction.OrderId,user.Id,_CompanyID);
                                                    //هنا يكون الترتيب السؤال مختلف
                                                    FCDetailsSorted++;
                                                    break;
                                                case "Defer":
                                                    //هنا يكون الترتيب السؤال مختلف
                                                    FCDetailsSorted++;
                                                    break;
                                                //ان كان معلق فسوف يزيد ترتيب السؤال بمقدار اثنان
                                                case "Hold":
                                                    FCDetailsSorted = FCDetailsSorted + 2;
                                                    break;
                                                //ان كان إكمال الطلب فسوف يتم ارسال رسالة للموظف وصاحب الطلب على انه تم اكمال اطلب
                                                case "Complete"://case "Complete Order":
                                                    await _WhatsappOrder.CompleteOrder(whatsappBotAction.OrderId, user.Id, _CompanyID);

                                                    await Sender.SendWhatsapp(sender, $"The Order {whatsappBotAction.OrderMaster!.Order_cd} completed.", 3);
                                                    await Sender.SendWhatsapp("966" + userOwner.PhoneNumber, $"The Order {whatsappBotAction.OrderMaster!.Order_cd} completed by user {user.FirstName} {user.LastName}.", 3);
                                                    
                                                    //await _WhatsappOrder.CompleteOrder(whatsappBotAction.OrderId, user.Id, _CompanyID);
                                                    return Ok();
                                                default:
                                                    //اذا لم يكن من هذه الاجابات السابقة فسوف نتحقق الاجابة اذا كانت تدل على تأجيل الطلب ام لا وهو السؤال من مرتبة الخامسة
                                                    string result = string.Empty;
                                                    if (FCDetailsSorted == 5)
                                                    {
                                                        //سوف يتم ارسال رسالة للموظف وصاحب الطلب على انه تم تأجيل الطلب ويتنهي
                                                        var dateTimeDefer = GeneralFun.GetCurrentTime();// DateTime.UtcNow;
                                                        switch (flowChartAction.Sorted)
                                                        {
                                                            case 1:
                                                                dateTimeDefer.AddMinutes(15);
                                                                break;
                                                            case 2:
                                                                dateTimeDefer.AddMinutes(30);
                                                                break;
                                                            case 3:
                                                                dateTimeDefer.AddHours(1);
                                                                break;
                                                            case 4:
                                                                dateTimeDefer.AddHours(2);
                                                                break;
                                                            case 5:
                                                                dateTimeDefer.AddHours(3);
                                                                break;
                                                            case 6:
                                                                dateTimeDefer.AddDays(1);
                                                                break;
                                                        }
                                                        await _WhatsappOrder.ChangeDefer(whatsappBotAction.OrderId, user.Id, dateTimeDefer, _CompanyID);

                                                        result = $"The Order {whatsappBotAction.OrderMaster!.Order_cd} defered to {dateTimeDefer.ToString("yyyy-MM-dd hh:mm tt")}.";
                                                        await Sender.SendWhatsapp(sender, result, 3);
                                                        await Sender.SendWhatsapp("966" + userOwner.PhoneNumber, $"{result} by user {user.FirstName} {user.LastName}.", 3);

                                                       // await _WhatsappOrder.ChangeDefer(whatsappBotAction.OrderId, user.Id, ,, _CompanyID);
                                                        return Ok();
                                                    }
                                                    break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //اذا لم يكن السؤال الثالث او اكثر
                                        if (flowChartAction.DisplayAnswer == "Assign")
                                        {
                                            FCDetailsSorted++;
                                        }
                                        else
                                        {
                                            switch (flowChartAction.DisplayAnswer)
                                            {
                                                case "Hold":
                                                    FCDetailsSorted = 6;
                                                    break;
                                                case "Change Department":
                                                    FCDetailsSorted = 7;
                                                    break;
                                                case "Purchase"://case "Purchase Order":
                                                    var orderMaster = whatsappBotAction.OrderMaster!;
                                                    //orderMaster.DepartmentID = "Dp4";
                                                    //_context.OrdersMaster.Update(orderMaster);
                                                    //await _context.SaveChangesAsync();

                                                    //await Sender.SendWhatsapp(sender, $"The order {orderMaster.Order_cd} transferred to purchase department.", 3);
                                                    var departmentPurchase=await _context.Departments
                                                                .FirstOrDefaultAsync(d => d.CompanyID==_CompanyID && d.MasterNameDefault == "Purchases");
                                                    if (departmentPurchase != null) {
                                                        await _WhatsappOrder.ChangePurchase(whatsappBotAction.OrderId,user.Id, departmentPurchase.DepartmentID, _CompanyID);
                                                    }
                                                    await Sender.SendWhatsapp(sender, $"The order {orderMaster.Order_cd} transferred to purchase department.", 3);
                                                    //Purchase Order Closed
                                                    return Ok();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //اذا لم تكن الاجابة موجودة
                                await Sender.SendWhatsapp(sender, "(Error) Not found actions.", 3);
                                return Ok();
                            }
                        }

                        //هنا يتم متابعة السؤال التالي عن طريق متغير 
                        //FCDetailsSorted
                        //وهو عبارة عن ترتيب السؤال بزيادة او نقصان
                        var nextQuestion = await _context.FlowChartDetails.FirstOrDefaultAsync(f => f.Sorted == FCDetailsSorted);
                        if (nextQuestion != null)
                        {
                            //تجهيز الرسالة
                            message = $"{nextQuestion.HeaderMessage_E}\n" +
                                     $"{nextQuestion.BodyMessage_E}\n" +
                                     $"{nextQuestion.FooterMessage_E}\n";

                            //اذا كان السؤال يحتوي على أجوبة متعددة
                            if (nextQuestion.MultiAnwser > 0)
                            {
                                //تجهيز الإجابات بناء على الترتيب ودمجها مع الرسالة
                                var answers = await _context.FlowChartActions
                                    .Where(fa => fa.FCDetailsID == nextQuestion.FCDetailsID && fa.bActive == 1)
                                    .OrderBy(fa => fa.Sorted)
                                    .ToListAsync();

                                if (nextQuestion.Sorted == 6)
                                {
                                    message += "Write a comment...\n";
                                }
                                else
                                {
                                    foreach (var answerNo in answers)
                                    {
                                        message += $"{answerNo.Sorted} - {answerNo.DisplayAnswer}\n";
                                    }
                                }

                                //ادراج رقم الطلب
                                if (message.Contains("{order_no}"))
                                {
                                    long order_cd = _context.OrdersMaster.FirstAsync(f => f.OrderID == whatsappBotAction.OrderId).Result.Order_cd;
                                    message = message.Replace("{order_no}", order_cd.ToString());
                                }

                                //Replace {item_order}
                                //تفاصيل الطلب
                                if (message.Contains("{item_order}"))
                                {
                                    string DetailItems = "";
                                    var ItemsOrder = await _context.VMOrderDetItems
                                        .Where(c => c.OrderID == whatsappBotAction.OrderId)
                                        .ToListAsync();
                                    if (ItemsOrder != null && ItemsOrder.Count > 0)
                                    {
                                        foreach (var Item in ItemsOrder)
                                        {
                                            DetailItems += Item.ItemName_E + $" ({Item.Qty})\n";
                                        }
                                    }
                                    message = message.Replace("{item_order}", DetailItems);
                                }
                            }
                            else
                            {
                                if (nextQuestion.Sorted == 2)
                                {
                                    //اذا كان السؤال من فئة اختيار الموظفين مع المحافظة على ترتيب الموظفين عبر 
                                    //Counter
                                    //من أجل الإجابة او الاختيار بالرقم
                                    var users = await _context.Users
                                            .Where(w => w.DepartmentID == _DepartmentID && w.bActive == 1 && w.IsDeleted == 0 && w.CompanyID == _CompanyID)
                                            .ToListAsync();

                                    var listEmployee = users.Select((u, index) => new
                                    {
                                        Counter = index + 1,
                                        FullName = $"{u.FirstName} {u.LastName}"
                                    }).ToList();

                                    //تجهير الرسالة مع قائمة الموظفين
                                    foreach (var emp in listEmployee)
                                    {
                                        message += $"{emp.Counter} - {emp.FullName}\n";
                                    }
                                }
                                else if (nextQuestion.Sorted == 7)
                                {
                                    var depts = await _context.Departments.Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0 && w.bActive == 1).Take(9).ToListAsync();
                                    var listDept = depts.Select((u, index) => new
                                    {
                                        Counter = index + 1,
                                        Dname = $"{u.DepName_E}"
                                    }).ToList();

                                    foreach (var dept in listDept)
                                    {
                                        message += $"{dept.Counter} - {dept.Dname}\n";
                                    }
                                }
                            }

                            //هنا الرقم المعرف للمستخدم خاص للسؤال اذا كان منقول او نفس المستخدم المرسل
                            string userId = moveUserId != string.Empty ? moveUserId : user.Id;

                            //هنا محافظة على ترتيب الأجوبة التي تمت الإجابة عنها في البوت لان لكل مستخدم جواب مع ترتيبه
                            var whByUser = await _context.WhatsappBotActions.Where(w => w.UserId == userId).ToListAsync();
                            int maxSorted = whByUser.Count > 0 ? whByUser.Max(m => m.Sorted) + 1 : 1;
                            //تخزين السؤال
                            var newWhatsappBotAction = new WhatsappBotAction()
                            {
                                Id = Guid.NewGuid().ToString(),
                                FCDetailsID = nextQuestion.FCDetailsID,
                                OrderId = whatsappBotAction.OrderId,
                                UserId = userId,
                                Sorted = maxSorted,
                                CompanyId=_CompanyID
                            };
                            await _context.WhatsappBotActions.AddAsync(newWhatsappBotAction);
                            await _context.SaveChangesAsync();
                            await Sender.SendWhatsapp(sender, message, 3);
                        }
                        else
                        {
                            //اذا كان لا يوجد اي سؤال تالي يدل على انهاء الطلب
                            await Sender.SendWhatsapp(sender, $"The order {whatsappBotAction.OrderMaster!.Order_cd} ended.", 3);
                            return StatusCode(403);
                        }
                    }
                    else
                    {
                        //اذا كان لا يوجد اي اكشن للاجابة على البوت
                        await Sender.SendWhatsapp(sender, "No active questions found for the specified flowchart.", 3);
                    }
                }
                else
                {
                    //اذا كانت الرسال فارغة مثل سمايلات او حركات يتم قراءتها على انها رسالة فارغة
                    await Sender.SendWhatsapp(sender, "The Message empty", 3);
                }
                return Ok();
            }
        }
        catch
        {
            return StatusCode(404);
        }
    }


}
