using Microsoft.AspNetCore.Identity;
using System.Globalization;
namespace LodgeMasterWeb.Services;

public static class GenericWhatsappOrder
{
    public static async Task<bool> TakeOrder(string OrderId, string Userid, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
            //Aziz
            //var _CompanyID = _httpcontext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(OrderId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = Userid;// _UserID;
                    empDep.Status = (int)enumStatus.Inprocess;
                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Inprocess, "", Userid, "Take employee order");
                    //GeneralFun.RecordLogOrdersAction(orderId, _UserID, _CompanyID, _context, (int)enumStatus.Inprocess, "", _UserID, "Take employee order");
                }
            }

            return true;// Json(datajson);
        }
        catch (Exception ex)
        {
            return false;// Json(new { success = false, returnData = ex.Message });
        }
    }
    public static async Task<bool> CompleteOrder(string OrderId, string Userid, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
            //Aziz

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

                    //var UserName = "";
                    //var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == Userid);

                    //if (UserData != null)
                    //{
                    //    UserName = UserData.FirstName + " " + UserData.LastName;
                    //}
                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Completed, "", Userid, $"Completed order > {UserName}");
                    GeneralFun.UpdateOrderQuantity(OrderId, _CompanyID, _context);
                }
            }
            return true;//Json(datajson);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static async Task<bool> ReopenOrder(string OrderId, string Userid, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
            if (!string.IsNullOrEmpty(OrderId))
            {
                var empDep = await _context.OrdersMaster
                     .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                if (empDep != null)
                {
                    empDep.UserIDAssign = Userid;
                    empDep.Status = (int)enumStatus.Open;
                    _context.OrdersMaster.Update(empDep);
                    await _context.SaveChangesAsync();

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.ReOpen, "", Userid, $"Reopen order > {UserName}");
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static async Task<bool> HoldOrder(string OrderId, string Userid, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
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

                    GeneralFun.RecordLogOrdersAction(OrderId, Userid, _CompanyID, _context, (int)enumStatus.Hold, "", Userid, $"Hold order > {UserName}");
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static async Task<bool> ChangeEmployee(string OrderId, string Userid, string EmpId, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public static async Task<bool> ChangeDepartment(string OrderId, string Userid, string DepartmentID, string _CompanyID, string UserName, ApplicationDbContext _context)
    {

        try
        {
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
        catch (Exception ex)
        {
            return false;
        }
    }

    public static async Task<bool> ChangeDefer(string orderId, string userId, string inputDate, string inputTime, string companyId, string userName, ApplicationDbContext context)
    {
        try
        {
            if (!string.IsNullOrEmpty(orderId) || !string.IsNullOrEmpty(inputDate) || !string.IsNullOrEmpty(inputTime))
            {
                var combinedDateTimeString = $"{inputDate} {inputTime}";

                if (DateTime.TryParseExact(combinedDateTimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime delayTime))
                {
                    var order = await context.OrdersMaster
                        .FirstOrDefaultAsync(x => x.CompanyID == companyId && x.OrderID == orderId);

                    if (order != null)
                    {
                        order.bDelay = 1;
                        order.DelayTime = delayTime;
                        order.Status = 9;

                        context.OrdersMaster.Update(order);
                        await context.SaveChangesAsync();

                        GeneralFun.RecordLogOrdersAction(orderId, userId, companyId, context, 1, "", "", $"defered to {delayTime}");
                    }
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            // يمكنك تسجيل الخطأ هنا
            return false;
        }
    }
    public static async Task<bool> addNoteforOrder(string OrderId, string Userid, string NoteOrder, string _CompanyID, string UserName, ApplicationDbContext _context)
    {
        try
        {
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
        catch (Exception ex)
        {
            return false;
        }
    }
}
