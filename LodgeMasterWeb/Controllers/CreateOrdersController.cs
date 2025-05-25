//using DocumentFormat.OpenXml.Drawing.Charts;
using LodgeMasterWeb.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;

namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    public class CreateOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
     
        // private static TimeZoneInfo Arabia_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
        public CreateOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        [HttpGet]
        public async Task<IActionResult> Index(OrderStepOneViewModel VMStep2, String Test)
        {
            ////////////////////
            ///تجهيز صفحة انشاء الطلبات
            ///////////////////
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }


                ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();


                if (VMStep2.BasketID == null)
                {

                    OrderStepOneViewModel ViewModel = new()
                    {
                        LstLocations = _context.CompanyUnits
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                            .OrderBy(x => x.LocationName)
                            .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                            .ToList(),
                        LstItems = _context.Items
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .OrderBy(x => x.ItemName_E)
                            .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                            .ToList(),
                        LstDepartment = _context.Departments
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .OrderBy(x => x.DepName_E)
                            .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                            .ToList(),

                        LstEmployees = _userManager.Users
                                        .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                        .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName })

                    };
                    return View(ViewModel);
                }
                else
                {

                    return View(VMStep2);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(OrderStepOneViewModel ViewModel)
        {
            ////////////////////
            /// تحميل بيانت صفحة انشاء الطلبات والانتقال الى الصفحة التالية
            ///////////////////
            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");
                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                var OrderGuid = "";

                if (!ModelState.IsValid)
                {
                    ViewModel.LstLocations = _context.CompanyUnits
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                            .OrderBy(x => x.LocationName)
                            .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                            .ToList();
                    ViewModel.LstItems = _context.Items
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .OrderBy(x => x.ItemName_E)
                            .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                            .ToList();
                    ViewModel.LstDepartment = _context.Departments
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .OrderBy(x => x.DepName_E)
                            .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                            .ToList();
                    ViewModel.LstEmployees = _userManager.Users
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });

                    return View(ViewModel);
                }



                if (ViewModel.BasketID != null)
                {
                    OrderGuid = ViewModel.BasketID;
                    var BasketData = _context.BasketTemps
                                .AsNoTracking()
                                .Where(x => x.EmpID == _UserID && x.BasketID == OrderGuid)
                                .OrderBy(x => x.OrderId)
                                .ToList();

                    if (BasketData.Any())
                    {
                        ViewModel.OrderID = string.Empty;
                        //ViewModel.BasketID = "";
                        //ViewModel.LocationID = string.Empty;
                        ViewModel.LocationName = GeneralFun.GetLocationNameById(ViewModel.LocationID, _CompanyID, _context);
                        ViewModel.LstLocations = _context.CompanyUnits
                                     .AsNoTracking()
                                     .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                                     .OrderBy(x => x.LocationName)
                                     .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                                     .ToList();
                        ViewModel.LstItems = _context.Items
                                    .AsNoTracking()
                                    .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                    .OrderBy(x => x.ItemName_E)
                                    .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                                    .ToList();
                        ViewModel.LstDepartment = _context.Departments
                                    .AsNoTracking()
                                    .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                    .OrderBy(x => x.DepName_E)
                                    .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                                    .ToList();
                        ViewModel.LstEmployees = _userManager.Users
                                    .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                    .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });

                        ViewModel.OrderItems.Clear();


                        foreach (var item in BasketData)
                        {

                            ViewModel.OrderItems.Add(new OrderItemQtyAssign
                            {
                                OrderId = item.OrderId,
                                BasketID = item.BasketID,
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                qty = item.Qty,
                                EmpAssignID = item.EmpAssignID,
                                EmpAssignName = "",
                                DepartmentID = item.DepartmentID,
                                DepartmentName = GeneralFun.GetDepartmentNameById(item.DepartmentID, _CompanyID, _context),
                                LstEmpDepartment = await GeneralFun.GetEmployeesByDepartmentId(item.DepartmentID, _CompanyID, _userManager),
                                EmployeeID = "",
                                //EmployeeName = "",
                                Notes = item.Notes

                            }); ;

                        }

                        //TempData["StepOneVM"] = ViewModel;
                        return View(nameof(step2), ViewModel);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    ViewModel.LstLocations = _context.CompanyUnits
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                            .OrderBy(x => x.LocationName)
                            .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                            .ToList();
                    ViewModel.LstItems = _context.Items
                            .AsNoTracking()
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                            .OrderBy(x => x.ItemName_E)
                            .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                            .ToList();
                    ViewModel.LstDepartment = _context.Departments
                             .AsNoTracking()
                             .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                             .OrderBy(x => x.DepName_E)
                             .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                             .ToList();

                    ViewModel.LstEmployees = _userManager.Users
                             .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                             .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });
                    return View(ViewModel);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //التاكد من استخدمها
        public async Task<IActionResult> ShowBasketList(string BasketID, string LocationID, string LocationName)
        {
            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (BasketID != null)
                {
                    OrderStepOneViewModel ViewModel = new OrderStepOneViewModel();

                    var BasketData = _context.BasketTemps
                        .AsNoTracking()
                        .Where(x => x.EmpID == _UserID && x.BasketID == BasketID)
                        .OrderBy(x => x.OrderId)
                        .ToList();

                    if (BasketData.Any())
                    {
                        ViewModel.OrderID = string.Empty;
                        ViewModel.BasketID = BasketID;
                        ViewModel.LocationID = LocationID;
                        ViewModel.LocationName = LocationName;

                        ViewModel.LstLocations = _context.CompanyUnits
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                                .OrderBy(x => x.LocationName)
                                .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                                .ToList();
                        ViewModel.LstItems = _context.Items
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                .OrderBy(x => x.ItemName_E)
                                .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                                .ToList();
                        ViewModel.LstDepartment = _context.Departments
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                .OrderBy(x => x.DepName_E)
                                .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                                .ToList();
                        //ViewModel.LstEmployees = _context.Employees
                        //        .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                        //        .OrderBy(x => x.FirstName)
                        //        .Select(s => new SelectListItem { Value = s.EmpID, Text = s.FirstName + " " + s.LastName })
                        //        .ToList();
                        ViewModel.LstEmployees = _userManager.Users
                                   .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                   .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });

                        ViewModel.OrderItems.Clear();

                        foreach (var item in BasketData)
                        {
                            ViewModel.OrderItems.Add(new OrderItemQtyAssign
                            {
                                OrderId = item.OrderId,
                                BasketID = item.BasketID,
                                ItemID = item.ItemID,
                                ItemName = item.ItemName,
                                qty = item.Qty,
                                EmpAssignID = item.EmpAssignID,
                                EmpAssignName = "",
                                DepartmentID = item.DepartmentID,
                                DepartmentName = GeneralFun.GetDepartmentNameById(item.DepartmentID, _CompanyID, _context),
                                LstEmpDepartment = await GeneralFun.GetEmployeesByDepartmentId(item.DepartmentID, _CompanyID, _userManager),
                                EmployeeID = "",
                                //EmployeeName = "",
                                Notes = item.Notes
                            });
                        }

                        //TempData["StepOneVM"] = ViewModel;
                        return View(nameof(step2), ViewModel);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return View(nameof(step2), null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> step2(OrderStepOneViewModel ViewModel)
        {
            //تجهيز الصفحة الثانية عرض كل الاصناف
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();


                ViewModel.LstDepartment = _context.Departments
                           .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                           .OrderBy(x => x.DepName_E)
                           .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                           .ToList();

                ViewModel.LstEmployees = _userManager.Users
                   .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                   .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult FinishOrder(OrderStepOneViewModel ViewModel)
        {

            return RedirectToAction(nameof(Index), "Dashboard");
        }
        [HttpPost]
        public JsonResult fillitems([FromBody] List<OrderItemQtyAssign> term, string Locationid)
        {

            var OrderGuid = Guid.NewGuid().ToString();
            TempData["OrderGuid"] = OrderGuid;

            OrderStepOneViewModel OrderOneViewModel = new()
            {
                OrderID = OrderGuid,
                LocationID = Locationid,
                // LocationName = _context.CompanyUnits.FirstOrDefault(l => l.LocationID == Locationid).LocationName,
                OrderItems = term,
            };

            return Json(OrderOneViewModel);
        }

        [HttpPost]
        public IActionResult AddItemBasket([FromBody] BasketTemp model)
        {
            ////////////////////
            ///اضافة الصنف الى جدول السلة والتاكد بنه غير موجود
            ///////////////////
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var OrderGuid = "";// TempData["OrderGuid"];
                //var _EmpID = "1";// HttpContext.Session.GetString("EmpID");
                var _DepartmentID = "";
                var _EmpAssignID = "";
                int _isService = 0;

                if (model.BasketID == null || model.BasketID == "")
                {
                    OrderGuid = Guid.NewGuid().ToString();
                }
                else
                {
                    OrderGuid = model.BasketID;
                }
                // احضار معلومات الصنف
                var ItemData = _context.Items
                            .AsNoTracking()
                            .FirstOrDefault(x => x.ItemID == model.ItemID && x.CompanyID == _CompanyID);
                if (ItemData != null)
                {

                    _DepartmentID = ItemData.DepartmentID;
                    _isService = ItemData.isService;

                    if (_isService == 1)
                    {
                        bool isItemSubQtyOk = false;

                        var itemServiceCount = _context.ItemServices
                                .Count(i => i.CompanyID == _CompanyID && i.ItemID == model.ItemID);

                        var ItemSubQtyOk = _context.ItemServices
                            .Where(t1 => t1.CompanyID == _CompanyID && t1.ItemID == model.ItemID)
                            .Join(_context.Items,
                                  t1 => t1.ItemIDSub,
                                  t2 => t2.ItemID,
                                  (t1, t2) => new { ItemService = t1, Item = t2 })
                            .Where(joinResult => joinResult.Item.Qty >= joinResult.ItemService.Qty)
                            .Select(joinResult => new
                            {
                                // select the properties you need from both tables
                                ItemService = joinResult.ItemService,
                                Item = joinResult.Item
                            })
                            .ToList();

                        if (ItemSubQtyOk != null && ItemSubQtyOk.Count > 0)
                        {
                            if (itemServiceCount == ItemSubQtyOk.Count)
                            {
                                isItemSubQtyOk = true;
                            }
                        }

                        if (isItemSubQtyOk == false)
                        {

                        }
                    }
                }


                //احضار بيانات الصنف من جدول السلة
                var result = _context.BasketTemps
                        .AsNoTracking()
                        .FirstOrDefault(x => x.ItemID == model.ItemID && x.EmpID == _UserID && x.BasketID == OrderGuid);
                //اذا  لم يكن موجود في جدول السلة يتم اضافته مع الكمية والتعليق والقسم والسوبرفيزر 
                if (result == null)
                {
                    //احضار رقم القسم  الخاص بالصنف 
                    //var departments = _context.Items
                    //        .AsNoTracking()
                    //        .Where(x => x.ItemID == model.ItemID && x.CompanyID == _CompanyID)
                    //        .Select(x => new
                    //        {
                    //            DepartmentID = x.DepartmentID,
                    //        })
                    //        .ToList(); 

                    if (ItemData != null)
                    {
                        //foreach (var Dep in departments)
                        //{
                        //    _DepartmentID = Dep.DepartmentID;
                        //}
                        //_DepartmentID = ItemData.DepartmentID;
                        //_isService = ItemData.isService;
                        //احضار السوبر فايز لهذا القسم لاسنادة الى الطلب
                        if (!string.IsNullOrEmpty(_DepartmentID))
                        {

                            var EmpAssign = _userManager.Users
                                           .FirstOrDefaultAsync(c => c.CompanyID == _CompanyID
                                           && c.DepartmentID == _DepartmentID
                                           && c.bActive == 1
                                           && c.IsDeleted == 0
                                           && c.supervisor == 1).Result;
                            //.FirstOrDefaultAsync().Result;
                            //.Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName });

                            if (EmpAssign != null)
                            {

                                _EmpAssignID = EmpAssign.Id;
                            }
                        }

                        if (_isService == 1)
                        {
                            model.Qty = 1;
                        }

                    }

                    model.BasketID = OrderGuid;
                    model.CompanyID = _CompanyID;
                    model.DepartmentID = _DepartmentID;
                    model.EmpAssignID = _EmpAssignID;
                    model.EmpID = _UserID;

                    _context.BasketTemps.Add(model);
                }
                else
                {
                    //اذا  كان موجود في جدول السلة يتم تحديث الكمية والتعليق فقط 
                    result.Qty = _isService == 1 ? 1 : (result.Qty + model.Qty);
                    result.Notes = model.Notes;
                    _context.BasketTemps.Update(result);
                }
                _context.SaveChanges();
                return Ok(OrderGuid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public async Task<IActionResult> UpdateItemBasketOrder(int OrderId, string Basketid, string newitemid, string olditemid,
                                            string newitemname, int newQty, string departmentid, string empAssigneeid)

        {
            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                var FindItemBasket = false;

                FindItemBasket = _context.BasketTemps.Any(x => x.BasketID == Basketid && x.ItemID == newitemid);

                var result = _context.BasketTemps
                            .FirstOrDefault(x => x.OrderId == OrderId
                                            && x.BasketID == Basketid
                                            && x.ItemID == olditemid);
                if (result != null)
                {

                    result.ItemID = newitemid;
                    result.ItemName = newitemname;
                    result.Qty = newQty;
                    result.DepartmentID = departmentid;
                    result.EmpAssignID = empAssigneeid;

                    _context.BasketTemps.Update(result);
                    _context.SaveChanges();

                    var empdata = await GeneralFun.GetEmployeesByDepartmentId(departmentid, _CompanyID, _userManager);
                    return Json(new { success = true, empdata = empdata, message = "تم الحفظ بنجاح" });
                    //}

                }
                else
                {

                    return Json(new { success = false, message = "توجد مشكلة  في الحفظ" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult UpdateEmpAssigneeDepartment(int OrderId, string basketid, string itemid, string empAssignID)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                var result = _context.BasketTemps
                            .FirstOrDefault(x => x.OrderId == OrderId
                                            && x.BasketID == basketid
                                            && x.ItemID == itemid
                                            && x.EmpID == _UserID
                                            && x.CompanyID == _CompanyID);
                if (result != null)
                {

                    result.EmpAssignID = empAssignID;

                    _context.BasketTemps.Update(result);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "تم الحفظ بنجاح" });

                }
                else
                {
                    return Json(new { success = false, message = "توجد مشكلة  في الحفظ" });
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult DeleteItemBasket(int OrderId, int checkcount = 0)
        {
            try
            {

                var result = _context.BasketTemps
                    .AsNoTracking()
                    .FirstOrDefault(x => x.OrderId == OrderId);// x => x.ItemID == model.ItemID && x.EmpID == _EmpID);// && x.BasketID == OrderGuid);
                if (result != null)
                {
                    _context.BasketTemps.Remove(result);
                    _context.SaveChanges();
                    return Ok();
                }
                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        public IActionResult RemoveItemBasket(int OrderId, string basketid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                // var OrderGuid = TempData["OrderGuid"];

                var result = _context.BasketTemps.AsNoTracking().Count(x => x.BasketID == basketid);// x => x.ItemID == model.ItemID && x.EmpID == _EmpID);// && x.BasketID == OrderGuid);
                if (result > 1)
                {
                    var resultRemove = _context.BasketTemps
                        .AsNoTracking()
                        .FirstOrDefault(x => x.OrderId == OrderId);// x => x.ItemID == model.ItemID && x.EmpID == _EmpID);// && x.BasketID == OrderGuid);
                    if (resultRemove != null)
                    {
                       _context.BasketTemps.Remove(resultRemove);
                        _context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public IActionResult GetAllItems(string orderid)
        {
            try
            {
                //var _CompanyID = HttpContext.Session.GetString("CompanyID");
                //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var OrderGuid = orderid;// TempData["OrderGuid"];

                var result = _context.BasketTemps
                    .AsNoTracking()
                    .Where(x => x.BasketID == OrderGuid && x.EmpID == _UserID)
                    .OrderBy(x => x.OrderId)
                    .ToList();

                string contentItem = "";
                // For example, if 'data' is an array of items, you can loop through them

                foreach (var item in result)
                {

                    contentItem += $"<span class='badge-item mx-2 mb-3' data-itemid='{item.ItemID}'  data-orderId='{item.OrderId}'";
                    contentItem += $"data-qty='{item.Qty}' data-notes='{item.Notes}' data-itemname='{item.ItemName}' data-ddLocation='{item.ItemName}'>";
                    contentItem += $"{item.ItemName}";
                    contentItem += $"<span class='badge-number'>{item.Qty}</span>";
                    contentItem += $"<span class='icon-exit'><i class='fa-solid fa-xmark fa-fw'></i></span>";
                    contentItem += $"</span>";

                }

                return Json(new { success = true, contentItems = contentItem });
                // return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult GetItemDetails([FromBody] ItemViewModel ItemDetails)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                var EditItemViewModel = new ItemViewModel
                {
                    BasketOrderId = ItemDetails.BasketOrderId,
                    BasketID = ItemDetails.BasketID,
                    OldItemID = ItemDetails.OldItemID,
                    OldItemName = ItemDetails.OldItemName,
                    OldItemQty = ItemDetails.OldItemQty,
                    NewItemQty = 0,
                    OldDepartmentID = ItemDetails.OldDepartmentID,
                    OldDepartmentName = ItemDetails.OldDepartmentName,

                    LstItems = _context.Items
                                    .AsNoTracking()
                                    .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                    .OrderBy(x => x.ItemName_E)
                                    .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                                    .ToList(),
                    LstDepartment = _context.Departments
                                    .AsNoTracking()
                                    .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                    .OrderBy(x => x.DepName_E)
                                    .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
                                    .ToList(),
                    //LstEmpAssignee = _context.Employees
                    //                .AsNoTracking()
                    //                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1
                    //                       && c.isDeleted == 0 && c.DepartmentID == ItemDetails.OldDepartmentID)
                    //                .OrderBy(x => x.FirstName)
                    //                .ThenBy(x => x.LastName)
                    //                .Select(s => new SelectListItem { Value = s.EmpID, Text = (s.FirstName + " " + s.LastName) })
                    //                .ToList(),
                    LstEmpAssignee = _userManager.Users
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0
                                       && c.DepartmentID == ItemDetails.OldDepartmentID)
                                .Select(s => new SelectListItem { Value = s.Id, Text = s.FirstName + " " + s.LastName }),

                    OldEmpAssigneeID = ItemDetails.OldEmpAssigneeID,
                    OldEmpAssigneeName = ItemDetails.OldEmpAssigneeName
                };

                //return PartialView("_Modal_Item", EditItemViewModel); _Edit_Item
                return PartialView("_Edit_Item", EditItemViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult AutocompleteLocation(string prefix)
        {
            //if (HttpContext.Session.GetString("CompanyID") != null)
            //{
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");

            var results = _context.CompanyUnits
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0 && c.LocationName.Contains(prefix))
                            .OrderBy(x => x.LocationName)
                            .Select(item => new { label = item.LocationName, Val = item.LocationID })
                            .ToList();  // Replace with your actual data service logic.
            return Json(results);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Userlogin");
            //}

        }
        [HttpGet]
        public async Task<IActionResult> GetAllLocation()
        {
            //if (HttpContext.Session.GetString("CompanyID") != null)
            //{
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");

            var results = await _context.CompanyUnits
                            .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                            .OrderBy(x => x.LocationName)
                            .Select(item => new { label = item.LocationName, Val = item.LocationID })
                            .ToListAsync();  // Replace with your actual data service logic.
            return Json(results);
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Userlogin");
            //}

        }
        [HttpPost]
        public JsonResult AutocompleteItems(string prefix)
        {
            if (HttpContext.Session.GetString("CompanyID") != null)
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                //var _UserID = HttpContext.Session.GetString("UserID");

                try
                {
                    // Query the database for autocomplete items
                    var results = _context.Items
                        .Where(c => c.CompanyID == _CompanyID
                                && c.bActive == 1
                                && c.IsDeleted == 0
                                && c.isInspection == 0
                                && c.ItemName_E.Contains(prefix))
                        .OrderByDescending(x => x.priorityOrder)
                        .ThenBy(x => x.ItemName_E)
                        .Select(item => new { Val = item.ItemID, Label = item.ItemName_E })
                        .ToList();

                    return Json(results);
                }
                catch (Exception ex)
                {
                    // Handle exceptions, log, or return an error response
                    return Json(new { error = "An error occurred while retrieving autocomplete items." });
                }
            }
            return Json(new { error = "An error occurred while retrieving autocomplete items." });
        }
        [HttpGet]
        public IActionResult BackstepOne()
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            OrderStepOneViewModel ViewModel = new()
            {
                LstLocations = _context.CompanyUnits
                                .AsNoTracking()
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.isDeleted == 0)
                                .OrderBy(x => x.LocationName)
                                .Select(s => new SelectListItem { Value = s.LocationID, Text = s.LocationName })
                                .ToList(),
                LstItems = _context.Items
                                .AsNoTracking()
                                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
                                .OrderBy(x => x.ItemName_E)
                                .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
                                .ToList()

            };

            // return RedirectToAction(nameof(Index), ViewModel);
            return View(nameof(Index), ViewModel);

        }
        [HttpPost]
        public async Task<IActionResult> SavedOrder(string Locationid, string Basketid, int btntype, OrderStepOneViewModel model)
        {
            try
            {
                ////////////////////
                /// مرحلة انشاء الطلبات حسب القسم وحفظها في الجدول والالانتقال الى الداش بورد
                ///////////////////

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var OrderGUIDBasket = Basketid;

                        if (!_context.BasketTemps.Any(x => x.EmpID == _UserID && x.BasketID == OrderGUIDBasket))
                        {
                            transaction.Rollback();
                            return Json(new { success = false, orderguid = "", message = "Basket is empty" });
                        }

                        //bool unClosed = _context.WhatsappBotActions.Any(w => w.UserId == _UserID && !w.Close);

                        //if (unClosed)
                        //{
                        //    transaction.Rollback();
                        //    return Json(new { success = false, orderguid = "", message = "You can't add items to the order while the bot is still active." });
                        //}

                        var BasketDataTotalDepartment = await _context.BasketTemps
                                                   .AsNoTracking()
                                                   .Where(x => x.EmpID == _UserID && x.BasketID == OrderGUIDBasket)
                                                   .GroupBy(x => x.DepartmentID)
                                                   .Select(group => new
                                                   {
                                                       DepartmentID = group.Key,
                                                       Count = group.Count()
                                                   })
                                                   .ToListAsync();
                        string OrderGUID = "",
                            OrderCd = string.Empty;
                        string DepartmentForItems = string.Empty;

                        if (BasketDataTotalDepartment.Any())
                        {
                            try
                            {
                                foreach (var Dep in BasketDataTotalDepartment)
                                {
                                    //var DepartmentForItems = Dep.DepartmentID;
                                    DepartmentForItems = "";

                                    DepartmentForItems = Dep.DepartmentID;

                                    OrderMaster OrderMS = new OrderMaster();

                                    OrderGUID = Guid.NewGuid().ToString();

                                    var maxOrderCd = await _context.OrdersMaster
                                                    .Where(c => c.CompanyID == _CompanyID) // Apply your condition here
                                                    .Select(o => o.Order_cd)
                                                    .DefaultIfEmpty()
                                                    .MaxAsync();

                                    // var dtCraeteformatted = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "dd/MM/yyyy HH:mm:ss",CultureInfo.InvariantCulture);
                                    if (maxOrderCd != null)
                                    {
                                        maxOrderCd += 1;
                                    }
                                    else
                                    {
                                        maxOrderCd = 1;
                                    }

                                    OrderMS.OrderID = OrderGUID;
                                    OrderMS.CompanyID = _CompanyID;
                                    OrderMS.Order_cd = maxOrderCd;
                                    OrderMS.dtCraete = Int32.Parse(GeneralFun.GetCurrentTime().ToString("yyyyMMdd"));
                                    OrderMS.dtCraeteStamp = GeneralFun.GetCurrentTime();// DateTime.Now;// DateTime.ParseExact(DateTime.Now,"dd/MM/yyyy HH:mm:ss",);

                                    OrderMS.LocationID = Locationid;// OrderSaveVM.LocationID;
                                                                    //OrderMS.DepartmentID = OrderSaveVM.;
                                    OrderMS.UserIDCreate = _UserID;
                                    OrderMS.sNotes = string.Empty;
                                    OrderMS.Status = 1;
                                    OrderMS.CatID = 0;
                                    OrderMS.DelayTime = GeneralFun.GetCurrentTime();// DateTime.Now;
                                    OrderMS.StampAssign = GeneralFun.GetCurrentTime();// DateTime.Now;

                                    //Change value
                                    OrderMS.DepartmentID = DepartmentForItems;

                                    var DepartmentDataSuper = await GeneralFun.GetSupervisorDep(DepartmentForItems, _CompanyID, _userManager);
                                    //if (string.IsNullOrEmpty(DepartmentDataSuper))
                                    //{
                                    //    transaction.Rollback();
                                    //    return Json(new { success = false, orderguid = "", message = "There is a problem with saving! No supervisor employee in the Department " });
                                    //}

                                    OrderMS.DepartmentAssignUserId = DepartmentDataSuper;
                                    OrderMS.UserIDAssign = string.Empty;
                                    OrderMS.DeptIDAssign = string.Empty;
                                    OrderMS.ForSuperviser = 1;// 0;

                                    OrderMS.LinkData = 0;
                                    OrderMS.bDelay = 0;
                                    OrderMS.isInspection = 0;
                                    OrderMS.FromGuest = model.FromGuest;// btntype; //0=from employee  1=from guest
                                    OrderMS.Priority = 2;
                                    
                                    _context.OrdersMaster.Add(OrderMS);

                                    OrderCd = OrderMS.Order_cd.ToString();

                                    var BasketItemByDep = await _context.BasketTemps
                                                        .AsNoTracking()
                                                        .Where(x => x.EmpID == _UserID && x.BasketID == OrderGUIDBasket && x.DepartmentID == DepartmentForItems)
                                                        .OrderBy(x => x.OrderId)
                                                        .ToListAsync();

                                    foreach (var item in BasketItemByDep)
                                    {
                                        var OrderDT = new OrderDet
                                        {
                                            OrderID = OrderGUID,
                                            CompanyID = _CompanyID,
                                            ItemID = item.ItemID,
                                            Qty = item.Qty,
                                            sItemNotes = item.Notes,
                                            isClosed = 0
                                        };
                                        _context.OrdersDet.Add(OrderDT);
                                    };
                                   await _context.SaveChangesAsync();

                                    var OrderAct = new OrderAction
                                    {
                                        //public long SerialID { get; set; }
                                        SerGUID = Guid.NewGuid().ToString(),
                                        OrderID = OrderGUID,
                                        CompanyID = _CompanyID,
                                        dtAction =  GeneralFun.GetCurrentTime(),// DateTime.Now,
                                        sNotes = "Created",//string.Empty,
                                        Satatus = 1,
                                        UserIDAction = _UserID,
                                        ToEmp = string.Empty,
                                        ToDepartment = DepartmentForItems,

                                    };

                                    _context.OrdersAction.Add(OrderAct);

                                    await _context.SaveChangesAsync();
                                    //STOP VELEVT
                                    var sendwhats = await StartWhatsapp(_CompanyID, _UserID, DepartmentForItems, OrderGUID, OrderCd, _context, _userManager);
                                }
                                transaction.Commit();

                                var BasketDataDelete = await _context.BasketTemps
                                                   .AsNoTracking()
                                                   .Where(x => x.EmpID == _UserID && x.BasketID == OrderGUIDBasket).ToListAsync();
                                if (BasketDataDelete != null)
                                {
                                     _context.BasketTemps.RemoveRange(BasketDataDelete);
                                    await _context.SaveChangesAsync();
                                }

                                //
                                //var sendwhats = await StartWhatsapp(_CompanyID, _UserID, DepartmentForItems, OrderGUID, OrderCd, _context, _userManager);
                                //createOrder"4ff2ab55-977b-4cea-9d56-6641572ab40b"

                                //إرسال رسالة واتس مع تخزين السؤال

                                //var flowChartMaster = await _context.FlowChartMasters.FirstAsync(x => x.CompanyID == _CompanyID);

                                //var questions = await _context.FlowChartDetails
                                //    .Where(fd => fd.FlowchartID == flowChartMaster.FlowchartID && fd.bActive == 1)
                                //    .OrderBy(fd => fd.Sorted)
                                //    .ToListAsync();

                                //if (questions.Any())
                                //{
                                //    var firstQuestion = questions.First();

                                //    var message = $"{firstQuestion.HeaderMessage_E}\n" +
                                //                  $"{firstQuestion.BodyMessage_E}\n" +
                                //                  $"{firstQuestion.FooterMessage_E}\n";

                                //    var answers = await _context.FlowChartActions
                                //        .Where(fa => fa.FCDetailsID == firstQuestion.FCDetailsID && fa.bActive == 1)
                                //        .OrderBy(fa => fa.Sorted)
                                //        .ToListAsync();

                                //    foreach (var answer in answers)
                                //    {
                                //        message += $"{answer.NoAnswer} - {answer.DisplayAnswer}\n";
                                //    }

                                //    var user = await _userManager.FindByIdAsync(_UserID);

                                //    if (user == null)
                                //        return NotFound();
                                //    //هذه موقتا لين ما بجيب رقم السوبر فايز ورقم جواله
                                //    var mobileNum = user.PhoneNumber;

                                //    if (message.Contains("{order_no}"))
                                //    {
                                //        message = message.Replace("{order_no}", OrderCd);
                                //    }
                                //    //تفاصيل الطلب
                                //    if (message.Contains("{item_order}"))
                                //    {
                                //        string DetailItems = "";
                                //        var ItemsOrder = await _context.VMOrderDetItems
                                //            .Where(c => c.OrderID == OrderGUID)
                                //            .ToListAsync();
                                //        if (ItemsOrder != null && ItemsOrder.Count > 0)
                                //        {
                                //            foreach (var Item in ItemsOrder)
                                //            {
                                //                DetailItems += Environment.NewLine + Item.ItemName_E + $" ({Item.Qty})";
                                //            }
                                //        }
                                //        message = message.Replace("{item_order}", DetailItems);
                                //    }

                                //    var interMobile = GeneralFun.FormatPhoneNumber(mobileNum);

                                //    //await Sender.SendWhatsapp("966" + mobileNum, message, 3);
                                //    if (interMobile == null)
                                //        return NotFound();

                                //    await Sender.SendWhatsapp(interMobile, message, 3);

                                //    var whByUser = await _context.WhatsappBotActions.Where(w => w.UserId == user.Id).ToListAsync();

                                //    int maxSorted = whByUser.Count > 0 ? whByUser.Max(m => m.Sorted) + 1 : 1;
                                //    //تخزين السؤال
                                //    var whatsappBotAction = new WhatsappBotAction()
                                //    {
                                //        Id = Guid.NewGuid().ToString(),
                                //        FCDetailsID = firstQuestion.FCDetailsID,
                                //        OrderId = OrderGUID,
                                //        UserId = user.Id,
                                //        Sorted = maxSorted
                                //    };
                                //    await _context.WhatsappBotActions.AddAsync(whatsappBotAction);
                                //    await _context.SaveChangesAsync();
                                //    //_logger.LogInformation("Sending message: \n{Message}", message);
                                //}
                                //else
                                //{
                                //    //_logger.LogWarning("No active questions found for the specified flowchart.");
                                //}


                                return Json(new { success = true, orderguid = "", message = "Order saved successfully" });
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                return Json(new { success = false, orderguid = "", message = "There is a problem with saving" });
                            }

                        }
                        else
                        {
                            return Json(new { success = false, orderguid = "", message = "There is a problem with saving" });
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        //return BadRequest(ex.Message);
                        return Json(new { success = false, orderguid = "", message = "Error" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, orderguid = "", message = ex.Message });

            }
        }
        public async Task<bool> StartWhatsapp(string _CompanyID, string UserId, string DepartmentForItems, string OrderGUID, string OrderCd
                                            , ApplicationDbContext _dbContext, UserManager<ApplicationUser> _userMg)
        {
            try
            {
                //createOrder"4ff2ab55-977b-4cea-9d56-6641572ab40b"

                //إرسال رسالة واتس مع تخزين السؤال
                {
                    var flowChartMaster = await _dbContext.FlowChartMasters.FirstAsync(x => x.CompanyID == _CompanyID);

                    var questions = await _dbContext.FlowChartDetails
                        .Where(fd => fd.FlowchartID == flowChartMaster.FlowchartID && fd.bActive == 1)
                        .OrderBy(fd => fd.Sorted)
                        .ToListAsync();

                    if (questions.Any())
                    {
                        var firstQuestion = questions.First();

                        var message = $"{firstQuestion.HeaderMessage_E}\n" +
                                      $"{firstQuestion.BodyMessage_E}\n" +
                                      $"{firstQuestion.FooterMessage_E}\n";

                        var answers = await _dbContext.FlowChartActions
                            .Where(fa => fa.FCDetailsID == firstQuestion.FCDetailsID && fa.bActive == 1)
                            .OrderBy(fa => fa.Sorted)
                            .ToListAsync();

                        foreach (var answer in answers)
                        {
                            message += $"{answer.NoAnswer} - {answer.DisplayAnswer}\n";
                        }

                        //var supervisorUser = await _context.Users.
                        //                    FirstOrDefaultAsync(c => c.CompanyID == _CompanyID
                        //                                        && c.supervisor == 1
                        //                                        && c.DepartmentID == DepartmentForItems);

                        var supervisorUser = await _context.Users.
                               FirstOrDefaultAsync(c => c.CompanyID == _CompanyID && c.Id == UserId);
                        
                        if (supervisorUser is not null)
                        {


                            var user = await _userMg.FindByIdAsync(supervisorUser.Id);// _UserID);

                            //if (user == null)
                            //    return false;// NotFound();
                            //هذه موقتا لين ما بجيب رقم السوبر فايز ورقم جواله
                            var mobileNum = supervisorUser.PhoneNumber;// user.PhoneNumber;

                            if (message.Contains("{order_no}"))
                            {
                                message = message.Replace("{order_no}", OrderCd);
                            }
                            //تفاصيل الطلب
                            if (message.Contains("{item_order}"))
                            {
                                string DetailItems = "";
                                var ItemsOrder = await _dbContext.VMOrderDetItems
                                    .Where(c => c.OrderID == OrderGUID)
                                    .ToListAsync();
                                if (ItemsOrder != null && ItemsOrder.Count > 0)
                                {
                                    foreach (var Item in ItemsOrder)
                                    {
                                        DetailItems += Environment.NewLine + Item.ItemName_E + $" ({Item.Qty})";
                                    }
                                }
                                message = message.Replace("{item_order}", DetailItems);
                            }

                            var interMobile = GeneralFun.FormatPhoneNumber(mobileNum);

                            //await Sender.SendWhatsapp("966" + mobileNum, message, 3);
                            if (interMobile == null)
                                return false;// return NotFound();

                            await Sender.SendWhatsapp(interMobile, message, 3);

                            //var whByUser = await _dbContext.WhatsappBotActions.Where(w => w.UserId == user.Id).ToListAsync();
                            var whByUser = await _dbContext.WhatsappBotActions.Where(w => w.UserId == supervisorUser.Id).ToListAsync();

                            int maxSorted = whByUser.Count > 0 ? whByUser.Max(m => m.Sorted) + 1 : 1;
                            //تخزين السؤال
                            var whatsappBotAction = new WhatsappBotAction()
                            {
                                Id = Guid.NewGuid().ToString(),
                                FCDetailsID = firstQuestion.FCDetailsID,
                                OrderId = OrderGUID,
                                UserId = supervisorUser.Id,//user.Id,
                                Sorted = maxSorted,
                                CompanyId = _CompanyID
                            };
                            await _dbContext.WhatsappBotActions.AddAsync(whatsappBotAction);
                            await _dbContext.SaveChangesAsync();
                            //_logger.LogInformation("Sending message: \n{Message}", message);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //_logger.LogWarning("No active questions found for the specified flowchart.");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #region "Omro"
        [HttpPost]
        public IActionResult OderDetails(DataItems dataObj)
        {
            return View();

        }
        public class DataItems
        {
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
        }
        [HttpPost]
        public ActionResult TestSaved(ClassData1 Obj1, ClassData2 Obj2, List<ClassData1> ListOfObj1)
        {
            if (ListOfObj1.Count > 0)
            {
                try
                {
                    ClassData3 ResData = new ClassData3();
                    ResData.Address = "KSA";
                    ResData.Name = "Ahmed Ali";
                    ResData.Age = 25;
                    ResData.CurrentDate = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    return Json(new { success = true, returnData = ResData, message = "تم الحفظ بنجاح" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "حدث خطأ اثناء العملية" });
                }
            }

            else
            {
                return Json(new { success = false, message = "عفوا بعض البيانات المرسلة ناقصة" });
            }
        }

        public class ClassData1
        {
            public int x { get; set; }
            public int y { get; set; }
        }
        public class ClassData2
        {
            public int z { get; set; }
            public int e { get; set; }
        }
        public class ClassData3
        {
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? CurrentDate { get; set; }
            public int Age { get; set; }
        }
        #endregion

    }
}
