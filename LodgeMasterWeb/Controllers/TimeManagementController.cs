using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using Newtonsoft.Json;
using LodgeMasterWeb.Services;
using LodgeMasterWeb.Core.Models;

namespace LodgeMasterWeb.Controllers;
public class TimeManagementController : Controller
{

    private readonly IHttpClientFactory _clientFactory;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<TimeManagementController> _logger;
    private readonly string _empImagePath;

    public TimeManagementController(IHttpClientFactory clientFactory, ILogger<TimeManagementController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _empImagePath = _webHostEnvironment.WebRootPath;
        _clientFactory = clientFactory;
    }

    public IActionResult TimeManagement()
    {
        return View();
    }
    #region "Credits Rooms"
    public IActionResult CreditsRooms()
    {
        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    #endregion

    #region "Shifts Managments"
    public IActionResult ShiftsManagement()
    {
        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> GetShiftWork()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var Skip = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);

            var sortColumnIndex = Request.Form["order[0][column]"];
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            var sortColumnDirection = Request.Form["order[0][dir]"];

            IQueryable<TmShiftsWork> itemsDep = _context.TmShiftsWorks
                                            .AsNoTracking()
                                            .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0);

            var recordsTotal = itemsDep.Count();
            var data = itemsDep.Skip(Skip).Take(pageSize).ToList();

            var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

            return Json(datajson);
        }
        catch (Exception ex)
        {

            return Json(new { success = false, returnData = ex.Message });
        }

    }
    [HttpPost]
    public async Task<IActionResult> CreateShiftWork(string dataObj)
    {
        try
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            TmShiftsWork dataOk = JsonConvert.DeserializeObject<TmShiftsWork>(dataObj);

            if (string.IsNullOrEmpty(dataOk.ShiftName_E) == true)
            {
                return Json(new { success = false, returnData = "enter english name" });
            }
            else
            {
                isFound = _context.Items.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.ItemName_E == dataOk.ShiftName_E);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "English Name already exists." });
                }
            }
            //if (dataOk.StartTime==isdateTime)
            //{

            //}

            var maxIdService = new GenericService(_context);
            //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
            int maxSortedNumber = maxIdService.GetMaxSorted<TmShiftsWork>(e => e.CompanyID == _CompanyID, e => e.iSorted);

            //var item = new Item();
            dataOk.ShiftID = Guid.NewGuid().ToString();
            dataOk.CompanyID = _CompanyID;
            // dataOk.bActive = dataOk.bActive;
            dataOk.iSorted = maxSortedNumber + 1;
            dataOk.IsDeleted = 0;
            dataOk.isDefault = 0;
            dataOk.CreateEmpID = _UserID;
            //dataOk.StartTime =2;
            //dataOk.EndTime =1;

            await _context.TmShiftsWorks.AddAsync(dataOk);
            await _context.SaveChangesAsync();

            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        catch (Exception ex)
        {

            var resJson = new { success = false, returnData = ex.Message };
            return Json(resJson);
        }

    }
    [HttpPost]
    public async Task<IActionResult> EditShiftWork(string shiftid)
    {
        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        var _UserID = HttpContext.Session.GetString("UserID");
        try
        {

            TmShiftsWork ShiftData = await _context.TmShiftsWorks.FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.ShiftID == shiftid);

            if (ShiftData != null)
            {
                return Json(new { success = true, returnData = ShiftData });
            }
            else
            {
                return Json(new { success = false, returnData = "" });
            }

        }
        catch (Exception ex)
        {

            return Json(new { success = false, returnData = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> SaveEditShiftWork(string dataObj)
    {

        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        var _UserID = HttpContext.Session.GetString("UserID");

        var isFound = false;
        TmShiftsWork dataOk = JsonConvert.DeserializeObject<TmShiftsWork>(dataObj);

        if (string.IsNullOrEmpty(dataOk.ShiftName_E) == true)
        {
            return Json(new { success = false, returnData = "enter english name" });
        }
        else
        {
            isFound = await _context.TmShiftsWorks
                    .AsNoTracking()
                    .AnyAsync(x => x.CompanyID == _CompanyID
                                && x.ShiftName_E == dataOk.ShiftName_E
                                && x.ShiftID != dataOk.ShiftID);

            if (isFound == true)
            {
                return Json(new { success = false, returnData = "English Name already exists." });
            }
        }

        var depUpdate = await _context.TmShiftsWorks.FirstOrDefaultAsync(i => i.CompanyID == _CompanyID && i.ShiftID == dataOk.ShiftID);

        if (depUpdate != null)
        {
            depUpdate.ShiftName_E = dataOk.ShiftName_E;
            depUpdate.ShiftName_A = dataOk.ShiftName_A;
            depUpdate.bActive = dataOk.bActive;

            _context.TmShiftsWorks.Update(depUpdate);
            await _context.SaveChangesAsync();

            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        else
        {
            var resJson = new { success = false, returnData = "not saved" };
            return Json(resJson);
        }

    }
    [HttpPost]
    public async Task<IActionResult> DeleteShiftWork(string shiftid)
    {
        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        var _UserID = HttpContext.Session.GetString("UserID");
        try
        {
            var itemRemove = await _context.TmShiftsWorks.FirstOrDefaultAsync(i => i.CompanyID == _CompanyID && i.ShiftID == shiftid);

            if (itemRemove != null)
            {
                itemRemove.IsDeleted = 1;
                _context.TmShiftsWorks.Update(itemRemove);
                await _context.SaveChangesAsync();

                var resJson = new { success = true, returnData = "Deleted" };
                return Json(resJson);
            }
            else
            {
                var resJson = new { success = false, returnData = "not delete" };
                return Json(resJson);
            }
        }
        catch (Exception ex)
        {
            var resJson = new { success = false, returnData = ex.Message };
            return Json(resJson);
        }

    }
    #endregion

    #region "Shift Employees"
    //public IActionResult ShiftEmployees()
    //{
    //    //ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();


    //}

    public async Task<IActionResult> ShiftEmployees()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            IEnumerable<TmEmpCreditVM> result = await _context.Users
                        .Where(u => u.CompanyID == _CompanyID
                                 //&& u.DepartmentID == ""
                                 && u.bActive == 1
                                 && u.IsDeleted == 0)
                        .OrderBy(o => o.FirstName)
                        .GroupJoin(
                            _context.TmEmployeesCredit,
                            user => user.Id,               // مفتاح الربط من AspNetUsers
                            creditRoom => creditRoom.EmpId, // مفتاح الربط من TmCreditRooms
                            (user, creditRooms) => new { user, creditRooms }) // الربط
                        .SelectMany(
                            temp => temp.creditRooms.DefaultIfEmpty(), // الربط الخارجي LEFT JOIN
                            (temp, creditRoom) => new TmEmpCreditVM
                            {
                                UFShiftId = creditRoom != null ? creditRoom.UFShiftId : "",
                                EmpId = temp.user.Id,
                                UFShiftStatus = creditRoom != null ? creditRoom.UFShiftStatus : "",
                                MaxCredits = creditRoom != null ? creditRoom.MaxCredits : 0,
                                MaxZones = creditRoom != null ? creditRoom.MaxZones : 0,
                                RoomSets = creditRoom != null ? creditRoom.RoomSets : 0,
                                ShiftTypeId = creditRoom != null ? creditRoom.ShiftTypeId : 0,
                                Weekend = creditRoom != null ? creditRoom.Weekend : 0,
                                sNotes = string.Empty,
                                EmpName = temp.user.FirstName + ' ' + temp.user.LastName,
                                //EmpId = temp.user.Id,
                                //UserId = temp.user.Id,
                                //temp.user.DepartmentID,
                                //temp.user.bActive,
                                //temp.user.supervisor,
                                //temp.user.IsDeleted,
                                //temp.user.CompanyID
                            })
                        .ToListAsync();

            if (result != null)
            {
                ViewBag.TotalDepartment = result.Count();
                return View(result); // تمرير البيانات إلى صفحة العرض
            }
            ViewBag.TotalDepartment = "0";
            return View(new List<TmEmpCreditVM>()); // تمرير قائمة فارغة في حالة عدم وجود نتائج
        }
        catch (Exception ex)
        {
            // التعامل مع الخطأ
            return Json("Error: " + ex.Message);
        }
    }
    public async Task<string> GetDepartmentHK()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            var DepIdHk = await _context.Departments
                .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.MasterNameDefault == "");

            if (DepIdHk != null)
            {
                return (DepIdHk.DepartmentID);
            }
            return ("");
        }
        catch (Exception)
        {

            return ("");
        }

    }


    [HttpPost]
    public async Task<IActionResult> SaveEmployeesCredits([FromBody] List<TmEmpCreditVM> employeesCredits)
    {
        try
        {
            //if (ModelState.IsValid)
            //{
            foreach (var credit in employeesCredits)
            {
                // قم بحفظ كل عنصر في قاعدة البيانات
                // مثال:
                var entity = await _context.TmEmployeesCredit.FirstOrDefaultAsync(e => e.EmpId == credit.EmpId);

                if (entity != null)
                {
                    entity.UFShiftStatus = credit.UFShiftStatus;
                    entity.MaxCredits = credit.MaxCredits;
                    entity.MaxZones = credit.MaxZones;
                    entity.RoomSets = credit.RoomSets;
                    entity.ShiftTypeId = credit.ShiftTypeId;
                    entity.Weekend = credit.Weekend;
                    entity.sNotes = credit.sNotes;

                    _context.Update(entity);
                }
                else
                {
                    // إذا كان العنصر جديدًا
                    await _context.TmEmployeesCredit.AddAsync(new TmEmpCredit
                    {
                        UFShiftId = Guid.NewGuid().ToString(),
                        EmpId = credit.EmpId,
                        UFShiftStatus = credit.UFShiftStatus,
                        MaxCredits = credit.MaxCredits,
                        MaxZones = credit.MaxZones,
                        RoomSets = credit.RoomSets,
                        ShiftTypeId = credit.ShiftTypeId,
                        Weekend = credit.Weekend,
                        sNotes = credit.sNotes
                    });
                }
            }
            await _context.SaveChangesAsync();

            return Json(new { success = true });
            //}
            // return Json(new { success = false, message = "Invalid data" });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Invalid data" });
        }

    }

    //[HttpGet]
    //public async Task<string> GetDepartmentHK()
    //{
    //    try
    //    {
    //        var _CompanyID = HttpContext.Session.GetString("CompanyID");

    //        var DepIdHk = await _context.Departments
    //            .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.MasterNameDefault == "");

    //        if (DepIdHk != null)
    //        {
    //            return Json(new { success = true, returnData = DepIdHk.DepartmentID });
    //        }
    //        return Json(new { success = false, returnData = "" });
    //    }
    //    catch (Exception)
    //    {

    //        return Json(new { success = false, returnData = "" });
    //    }

    //}
    #endregion

    #region "Tm Tasks"
    public IActionResult TmTasks()
    {
        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> getRooms()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            var RoomData = await _context.CompanyUnits
                        .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0 && x.bActive == 1)
                        .OrderBy(o => o.LocationName)
                        .Select(xdata => new { id = xdata.LocationID, name = xdata.LocationName })
                        .ToListAsync();
            if (RoomData == null)
            {
                return Json(new { success = false, message = "No Rooms Found" });
            }

            return Json(new { success = true, rooms = RoomData });

        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }

    }
    [HttpGet]
    public async Task<IActionResult> getServices()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            var ItemData = await _context.Items
                        .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.bActive == 1 && x.isInspection == 0)
                        .OrderBy(o => o.ItemName_E)
                        .Select(xdata => new { id = xdata.ItemID, name = xdata.ItemName_E })
                        .ToListAsync();
            if (ItemData == null)
            {
                return Json(new { success = false, message = "No Service Found" });
            }

            return Json(new { success = true, services = ItemData });

        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }

    }
    [HttpPost]
    //public async Task<IActionResult> getDataWeek([FromBody] List<string> dates)
    public async Task<IActionResult> getDataWeek(int WeekNo, int YearNo)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            if (YearNo == null)
            {
                return BadRequest("No dates received.");
            }

            // معالجة التواريخ هنا حسب المطلوب
            // على سبيل المثال: يمكنك عرض التواريخ في الـ console للتحقق

            //foreach (var date in dates)
            //{
            //    //Console.WriteLine(date);


            //}

            var dataweek = await _context.TmWeeksMaster
                .Where(x => x.CompanyID == _CompanyID && x.WeekNo == WeekNo && x.WeekYear == YearNo)
                .ToListAsync();

            if (dataweek == null || !dataweek.Any())
            {
                return Json(new { success = false, receivedData = "", message = "No data found for this week." });
            }


            //return Json(new { success = true,message = "Dates received successfully.", receivedData = dataweek });
            return Json(new { success = true, receivedData = dataweek });
        }
        catch (Exception)
        {
            throw;
        }
    }
    [HttpPost]
    public async Task<IActionResult> saveTask([FromBody] List<TaskViewModel> taskList)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            if (taskList == null || !taskList.Any())
            {
                return BadRequest("قائمة المهام فارغة.");
            }



            // تهيئة قائمة لتخزين المهام الجديدة
            //var tasks = taskList.Select(t => new Task
            //{
            //    RoomId = t.RoomId,
            //    RoomName = t.RoomName,
            //    ItemId = t.ItemId
            //}).ToList();

            //// إضافة المهام الجديدة إلى قاعدة البيانات
            //await _context.TmWeeksTasks.AddRange(tasks);
            //await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Dates received successfully." });
        }

        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }

    }
    public class TaskViewModel
    {
        public string RoomId { get; set; }
        public string RoomName { get; set; }
        public string ItemId { get; set; }
    }
    #endregion

    #region "New TM Task"
    public IActionResult PmsRoom(){

        try
        {

            return View();
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion
    #region "Overview Employees"
    public IActionResult OverviewEmployees()
    {
        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    #endregion

    #region "Report Tasks"
    public IActionResult ReportTasks()
    {
        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    #endregion

}
