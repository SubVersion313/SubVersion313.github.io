using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace LodgeMasterWeb.Services;

public class WhatsappOrder
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    //public WhatsappOrder(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, UserManager<ApplicationUser> UserManager, HttpContext httpContext, IWebHostEnvironment webHostEnvironment)
    public WhatsappOrder(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, UserManager<ApplicationUser> UserManager)
    {
        _context = context;
        _userManager = UserManager;
        _httpContextAccessor = httpContextAccessor;
    }
   // [HttpPost("takeorder")]
    public async Task<bool> TakeOrder(string OrderId, string Userid,string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = Userid;// _UserID;
                    empDep.Status = (int)enumStatus.Inprocess;
                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();
                    var UserName = "";
                    var UserData = await _userManager.Users.FirstOrDefaultAsync(u => u.CompanyID == _CompanyID && u.Id == Userid);

                    if (UserData != null)
                    {
                        UserName = UserData.FirstName + " " + UserData.LastName;
                    }
                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Inprocess, "", Userid, "Take employee order");
                    //GeneralFun.RecordLogOrdersAction(orderId, _UserID, _CompanyID, _context, (int)enumStatus.Inprocess, "", _UserID, "Take employee order");
                }
            }

            //var datajson = new { success = true, returnData = "Saved" };
            return true;// Json(datajson);
        }
        catch (Exception ex)
        {
            return false;// Json(new { success = false, returnData = ex.Message });
        }


    }
    //[HttpPost("Completeorder")]
    public async Task<bool> CompleteOrder(string OrderId, string Userid, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");

            //if (!GeneralFun.CheckLoginUser(HttpContext))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity" });
            //}

            if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = Userid;// _UserID;
                    empDep.Status = (int)enumStatus.Completed;
                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    var UserName = "";
                    var UserData = await _userManager.Users.FirstOrDefaultAsync(u => u.CompanyID == _CompanyID && u.Id == Userid);

                    if (UserData != null)
                    {
                        UserName = UserData.FirstName + " " + UserData.LastName;
                    }
                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Completed, "", Userid, $"Completed order > {UserName}");
                    await GeneralFun.UpdateOrderQuantity(OrderId, _CompanyID, _context);
                }
            }

            //var datajson = new { success = true, returnData = "Saved" };
            return true;//Json(datajson);
        }
        catch (Exception)
        {
            //return Json(new { success = false, returnData = ex.Message });
            return false;
        }

    }
    //[HttpPost("ReopenOrder")]
    public async Task<bool> ReopenOrder(string OrderId, string Userid, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");


            //if (!GeneralFun.CheckLoginUser(HttpContext))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity" });
            //}

            if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = Userid;
                    empDep.Status = (int)enumStatus.Open;
                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    var UserName = "";
                    var UserData = await _userManager.Users.FirstOrDefaultAsync(u => u.CompanyID == _CompanyID && u.Id == Userid);

                    if (UserData != null)
                    {
                        UserName = UserData.FirstName + " " + UserData.LastName;
                    }
                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.ReOpen, "", Userid, $"Reopen order > {UserName}");
                }
            }

            //var datajson = new { success = true, returnData = "Saved" };
            return true;
        }
        catch (Exception )
        {
            return false;
        }

    }
    //[HttpPost("HoldOrder")]
    public async Task<bool> HoldOrder(string OrderId, string Userid, string _CompanyID,string NotesData)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");


            //if (!GeneralFun.CheckLoginUser(HttpContext))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity" });
            //}

            if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
            {
                var OrderData = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (OrderData != null)
                {
                    OrderData.UserIDAssign = Userid;
                    OrderData.Status = (int)enumStatus.Hold;
                    _context.OrdersMaster.Update(OrderData);
                    await _context.SaveChangesAsync();

                    var UserName = "";
                    var UserData = await _userManager.Users.FirstOrDefaultAsync(u => u.CompanyID == _CompanyID && u.Id == Userid);

                    if (UserData != null)
                    {
                        UserName = UserData.FirstName + " " + UserData.LastName;
                    }
                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Hold,  "", Userid, $"Hold order > {UserName} {(string.IsNullOrEmpty(NotesData) ? "" : ":" + NotesData)}");
                }
            }


            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
    //[HttpPost("ChangeEmployee")]
    public async Task<bool> ChangeEmployee(string OrderId, string Userid, string EmpId, string _CompanyID)
    {

        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");


            //if (!GeneralFun.CheckLoginUser(HttpContext))
            //{
            //    return RedirectToPage("/Account/Login", new { area = "Identity" });
            //}

            if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(EmpId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = EmpId;

                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();


                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, 1, "", EmpId, "Change employee");
                }
            }

            return true;
        }
        catch (Exception )
        {
            return false;
        }
    }
   // [HttpPost("ChangeDepartment")]
    public async Task<bool> ChangeDepartment(string OrderId, string Userid, string DepartmentID, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(DepartmentID))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                if (empDep != null)
                {
                    empDep.DepartmentID = DepartmentID;

                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, 1, DepartmentID, "", "Change Department");
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    //[HttpPost("ChangeDefer")]
    public async Task<bool> ChangeDefer(string OrderId, string Userid, DateTime delayTime, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(OrderId)/* || !string.IsNullOrEmpty(inputdate) || !string.IsNullOrEmpty(inputtime)*/)
            {
                // Combine date and time strings into a single string in "yyyy-MM-dd HH:mm:ss" format
                //var combinedDateTimeString = $"{inputdate} {inputtime}";
                // Parse the combined string into a DateTime object
                //if (DateTime.TryParseExact(combinedDateTimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime delayTime))
                {
                    var deferOrder = await _context.OrdersMaster
                                    .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                    if (deferOrder != null)
                    {
                        deferOrder.bDelay = 1;
                        deferOrder.DelayTime = delayTime;
                        deferOrder.Status = 13;

                        _context.OrdersMaster.Update(deferOrder);
                        await _context.SaveChangesAsync();

                        GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, 1, "", "", $"defered to {delayTime.ToString("yyyy-MM-dd hh:mm tt")}");
                    }
                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    //[HttpPost("addNoteforOrder")]
    public async Task<bool> addNoteforOrder(string OrderId, string Userid, string NoteOrder, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(NoteOrder))
            {
                var orderNote = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                if (orderNote != null)
                {
                    orderNote.sNotes = NoteOrder;

                    _context.OrdersMaster.Update(orderNote);
                    await _context.SaveChangesAsync();

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, 1, "", "", NoteOrder);

                }
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    //[HttpPost("ChangePurchase")]
    public async Task<bool> ChangePurchase(string OrderId, string Userid, string DepartmentID, string _CompanyID)
    {
        try
        {
            //Aziz
            //var httpContext = _httpContextAccessor.HttpContext;
            //var _CompanyID = httpContext.Session.GetString("CompanyID");
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");



            if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(DepartmentID))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                if (empDep != null)
                {
                    empDep.DepartmentID = DepartmentID;

                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, 1, DepartmentID, "", "Change Department");
                }
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
