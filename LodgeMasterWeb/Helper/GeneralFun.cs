

//using DotVVM.Framework.Controls;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LodgeMasterWeb.Helper
{
    public static class GeneralFun
    {
        public static string GetbgColorClass(int StatusId)
        {

            switch (StatusId)
            {
                case 1:
                    return "bg-open";
                case 2:
                    return "bg-Completed";
                case 3:
                    return "bg-process";
                case 9:
                    return "bg-Hold";
                case 12:
                    return "bg-Closed";
                default:
                    return "";
            }
        }

        public static string ShowDate()
        {
            string CurrntDate = "";
            DateTime ZoneLocal = GeneralFun.GetCurrentTime();
            CultureInfo cultureInfo = new CultureInfo("en-US");
            CurrntDate = ZoneLocal.ToString("dddd, dd MMMM yyyy", cultureInfo);

            return CurrntDate;
        }
        public static bool CheckLoginUser(HttpContext httpContext)
        {
            //التحقق من دخول المستخدم 
            try
            {
                var bOk = false;
                if (!string.IsNullOrEmpty(httpContext.Session.GetString("UserID")))
                    bOk = true;

                return bOk;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public static string GetSupervisorDep_org(string DepartmentID, string _CompanyID, ApplicationDbContext _context)
        {
            //غير مستخدمة في الوقت الحالي وتم استبدالها
            try
            {
                var result = "";
                var DepartmentDataSuper = _context.Employees
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == DepartmentID && x.supervisor == 1);

                if (DepartmentDataSuper != null)
                {
                    result = DepartmentDataSuper.EmpID;
                }
                else
                {
                    var DepartmentDataUser = _context.Employees
                       .AsNoTracking()
                       .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == DepartmentID);
                    if (DepartmentDataUser != null)
                    {
                        result = DepartmentDataUser.EmpID;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static async Task<string> GetSupervisorDep(string _DepartmentID, string _CompanyID, UserManager<ApplicationUser> _userManager)
        {
            // احضار اول شخص سوبرفايزر في القسم
            try
            {
                var result = "";
                var DepartmentDataSuper = _userManager.Users
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == _DepartmentID && x.supervisor == 1);

                if (DepartmentDataSuper != null)
                {
                    result = DepartmentDataSuper.Id;
                }
                else
                {
                    var DepartmentDataUser = _userManager.Users
                       .AsNoTracking()
                       .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == _DepartmentID);
                    if (DepartmentDataUser != null)
                    {
                        result = DepartmentDataUser.Id;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string GetDepartmentIDForUserId(string UserID, string _CompanyID, UserManager<ApplicationUser> _userManager)
        {
            //احضار رقم قسم الموظف عن طريق رقمه
            try
            {
                var result = "";
                var DepartmentData = _userManager.Users
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.Id == UserID);

                if (DepartmentData != null)
                {
                    result = DepartmentData.DepartmentID;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string GetDepartmentIdByName(string DepartmentName, string _CompanyID, ApplicationDbContext _context)
        {
            //احضار رقم القسم عن طريق اسمه
            try
            {
                var result = "";
                var DepartmentData = _context.Departments
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepName_E == DepartmentName);

                if (DepartmentData != null)
                {
                    result = DepartmentData.DepartmentID;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string GetDepartmentNameById(string DepartmentID, string _CompanyID, ApplicationDbContext _context)
        {
            //احضار اسم القسم عن طريق رقمه
            try
            {
                var result = "";
                var DepartmentData = _context.Departments
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == DepartmentID);

                if (DepartmentData != null)
                {
                    result = DepartmentData.DepName_E;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static string GetLocationNameById(string locationid, string _CompanyID, ApplicationDbContext _context)
        {
            //احضار اسم الموقع عن طريق ؤقمه
            try
            {
                var result = "";
                var locationData = _context.CompanyUnits
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == _CompanyID && x.LocationID == locationid);

                if (locationData != null)
                {
                    result = locationData.LocationName;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public static List<SelectListItem> GetEmployeesByDepartmentId_org(string DepartmentID, string _CompanyID, ApplicationDbContext _context)
        {
            //غير مستخدم وتم استبدالها
            try
            {
                var result = new List<SelectListItem>();

                var EmpDepartmentData = _context.Employees
                               .AsNoTracking()
                               .Where(x => x.CompanyID == _CompanyID && x.DepartmentID == DepartmentID)
                               .OrderBy(x => x.FirstName)
                               .ThenBy(x => x.LastName)
                               .Select(item => new SelectListItem { Text = (item.FirstName + " " + item.LastName), Value = item.EmpID })
                               .ToList();

                if (EmpDepartmentData != null)
                {
                    result = EmpDepartmentData;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static async Task<List<SelectListItem>> GetEmployeesByDepartmentId(string DepartmentID, string _CompanyID, UserManager<ApplicationUser> _userManager)
        {

            try
            {
                var result = new List<SelectListItem>();

                var EmpDepartmentData = await _userManager.Users
                               .AsNoTracking()
                               .Where(x => x.CompanyID == _CompanyID && x.DepartmentID == DepartmentID)
                               .OrderBy(x => x.FirstName)
                               .ThenBy(x => x.LastName)
                               .Select(item => new SelectListItem { Text = (item.FirstName + " " + item.LastName), Value = item.Id })
                               .ToListAsync();

                if (EmpDepartmentData != null)
                {
                    result = EmpDepartmentData;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public static async Task<string> GetRolesAllstringById(string _UserID, UserManager<ApplicationUser> _userManager)
        {
            //احضار جميع الرول للموظف  عن طريق رقمه
            try
            {
                string result = "";// new List<SelectListItem>();
                var user = await _userManager.FindByIdAsync(_UserID);

                if (user != null)
                {
                    var RolesData = await _userManager.GetRolesAsync(user);
                    if (RolesData != null)
                    {
                        foreach (var item in RolesData)
                        {
                            result += item + ",";
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }

        }
        public static DateTime GetCurrentTime()
        {
            //ضبط الوقت عن سيرفر الاستضافة +3 ساعات
            DateTime serverTime = DateTime.Now;
            DateTime _localTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(serverTime, TimeZoneInfo.Local.Id, "Arabic Standard Time");// "Mountain Standard Time");
            return _localTime;
        }
        public static int GetTotalDiffByType(DateTime startDate, char type)
        {
            DateTime currentDate = GetCurrentTime();// DateTime.Now; // or DateTime.UtcNow if you want to use UTC time

            TimeSpan difference = currentDate - startDate;

            switch (type)
            {
                case 'D': // Days
                    return (int)difference.TotalDays;
                case 'H': // Hours
                    return (int)difference.TotalHours;
                case 'M': // Minutes
                    return (int)difference.TotalMinutes;
                default:
                    throw new ArgumentException("Invalid time type. Use 'D' for days, 'H' for hours, or 'M' for minutes.");
            }
        }
        public static async Task<string> GetCompanyDataByUserId(string UserIdCompany, ApplicationDbContext _context)
        {
            //ارجاع اسم المجلد التابع للشركة
            try
            {
                var result = "";
                var CompanyData = _context.Companies
                               .AsNoTracking()
                               .FirstOrDefault(x => x.CompanyID == UserIdCompany);

                if (CompanyData != null)
                {
                    result = CompanyData.CompanyFolder;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        #region "Record log"
        public static void RecordLogOrdersAction(string OrderId, string UserID, string CompanyID,
                            ApplicationDbContext _context, int SatatusType, string ToDepartment = "", string ToEmp = "", string Notes = "")
        {
            //تسجيل الاكشن في الجدول
            try
            {

                OrderAction orderAction = new OrderAction
                {
                    SerGUID = Guid.NewGuid().ToString(),
                    OrderID = OrderId,
                    CompanyID = CompanyID,
                    dtAction = GeneralFun.GetCurrentTime(),// DateTime.Now,
                    sNotes = Notes,
                    Satatus = SatatusType,
                    UserIDAction = UserID,
                    ToDepartment = ToDepartment,
                    ToEmp = ToEmp
                };

                _context.OrdersAction.Add(orderAction);
                _context.SaveChanges();



                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }

        }
        #endregion

        public static async Task UpdateItemQuantity(string ItemId, int Qty, string _CompanyId, ApplicationDbContext _context)
        {
            try
            {

                var itemUpdate = _context.Items.FirstOrDefault(i => i.CompanyID == _CompanyId && i.ItemID == ItemId);

                if (itemUpdate != null)
                {
                    itemUpdate.Qty -= Qty;

                    _context.Items.Update(itemUpdate);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task UpdateOrderQuantity(string OrderId, string _CompanyId, ApplicationDbContext _context)
        {
            try
            {

                var orderUpdate = _context.OrdersDet
                        .Where(o => o.CompanyID == _CompanyId && o.OrderID == OrderId);

                if (orderUpdate != null)
                {

                    foreach (var item in orderUpdate)
                    {
                        var itemUpdate = _context.Items.FirstOrDefault(i => i.CompanyID == _CompanyId && i.ItemID == item.ItemID);

                        if (itemUpdate != null)
                        {
                            itemUpdate.Qty -= item.Qty;

                            _context.Items.Update(itemUpdate);
                        }
                    }
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public static bool IsFirstCharBetweenAandZ(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            char firstChar = input[0];
            return (firstChar >= 'A' && firstChar <= 'Z') || (firstChar >= 'a' && firstChar <= 'z');
        }

        public static string FormatPhoneNumber(string phoneNumber)
        {
            // إزالة أي مسافات أو فواصل من الرقم
            phoneNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "");

            // التحقق من أن الرقم بالفعل في الصيغة الصحيحة (يبدأ بـ 9665 ويليه 8 أرقام)
            string pattern = @"^9665\d{8}$";
            if (Regex.IsMatch(phoneNumber, pattern))
            {
                return phoneNumber; // إذا كان في الصيغة الصحيحة، لا حاجة للتعديل
            }

            // التحقق إذا كان الرقم يحتوي على 9 أرقام ويبدأ بـ 05
            if (Regex.IsMatch(phoneNumber, @"^05\d{8}$"))
            {
                // تحويل الرقم إلى الصيغة المطلوبة
                return "966" + phoneNumber.Substring(1);
            }

                // التحقق إذا كان الرقم يحتوي على 9 أرقام ويبدأ بـ 5 فقط
                if (Regex.IsMatch(phoneNumber, @"^5\d{8}$"))
            {
                // تحويل الرقم إلى الصيغة المطلوبة
                return "966" + phoneNumber;
            }

            // إذا لم يكن بالإمكان تحويل الرقم، ارجع null
            return phoneNumber;
        }
        public static string ChekPhoneNumber(string phoneNumber)
        {
            // إزالة أي مسافات أو فواصل من الرقم
            phoneNumber = phoneNumber.Trim().Replace(" ", "").Replace("-", "");

            // التحقق من أن الرقم بالفعل في الصيغة الصحيحة (يبدأ بـ 9665 ويليه 8 أرقام)
            string pattern = @"^9665\d{8}$";
            if (Regex.IsMatch(phoneNumber, pattern))
            {
                return phoneNumber; // إذا كان في الصيغة الصحيحة، لا حاجة للتعديل
            }

            // التحقق إذا كان الرقم يحتوي على 9 أرقام ويبدأ بـ 05
            if (Regex.IsMatch(phoneNumber, @"^05\d{8}$"))
            {
                // تحويل الرقم إلى الصيغة المطلوبة
                return "966" + phoneNumber.Substring(1);
            }

            // التحقق إذا كان الرقم يحتوي على 9 أرقام ويبدأ بـ 5 فقط
            if (Regex.IsMatch(phoneNumber, @"^5\d{8}$"))
            {
                // تحويل الرقم إلى الصيغة المطلوبة
                return "966" + phoneNumber;
            }

            // إذا لم يكن بالإمكان تحويل الرقم، ارجع null
            return phoneNumber;
        }
    }
}
