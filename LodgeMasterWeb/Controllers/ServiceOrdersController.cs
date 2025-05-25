//using DotVVM.Framework.Controls;
//using Humanizer;
using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    public class ServiceOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;
        private readonly string _empAVATAR = @"Images/avatar/bg-man.jpg";

        public ServiceOrdersController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = UserManager;
            _webHostEnvironment = webHostEnvironment;
            _empImagePath = _webHostEnvironment.WebRootPath;
        }


        #region "Dashboard Page"
        [Authorize(Permissions.ServiceOrders.View)]
        public IActionResult Index()
        {

            if (!GeneralFun.CheckLoginUser(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();// DateTime.Now.Date.ToString("dddd, dd MMMM yyyy", cultureInfo);
                                                              //دخول الشاشة

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCountOrders()
        {
            // يرجع بعدد العمليات لكل حالة
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");
                var UserInfo = _userManager.GetUserAsync(User).Result;

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                var CountOrders = _context.OrdersMaster
                            .Where(x => x.CompanyID == _CompanyID && (x.DepartmentID == UserInfo.DepartmentID
                            || x.UserIDAssign == _UserID))
                            .GroupBy(x => x.Status)
                            .Select(item => new
                            {
                                statusid = item.Key,
                                total = item.Count()
                            })
                            .ToList();

                if (CountOrders != null && CountOrders.Count>0)
                {
                    var OpenCount = CountOrders.FirstOrDefault(c => c.statusid == 1).total;
                    TempData["ServiceCount"] = OpenCount;
                }
                else
                {
                    TempData["ServiceCount"] = 0;
                }
                return Json(new { success = true, returnData = CountOrders });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = "" });
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> GetOrdersByStatus(int stid)
        //{
        //    try
        //    {
        //        //Aziz
        //        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //        var _UserID = HttpContext.Session.GetString("UserID");
        //        var UserInfo = _userManager.GetUserAsync(User).Result;

        //        var Skip = int.Parse(Request.Form["start"]);
        //        var pageSize = int.Parse(Request.Form["length"]);

        //        var searchValue = Request.Form["search[value]"];

        //        var sortColumnIndex = Request.Form["order[0][column]"];
        //        var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
        //        var sortColumnDirection = Request.Form["order[0][dir]"];

        //        var sortColumnIndex2 = Request.Form["order[1][column]"];
        //        var sortColumn2 = Request.Form[$"columns[{sortColumnIndex2}][name]"];
        //        var sortColumnDirection2 = Request.Form["order[1][dir]"];

        //        // IQueryable<VMOrders> Orders;

        //        var excludedStatusIds = new List<int> { 9, 12, 13 };
        //        var Orders =await _context.Vmorders
        //            .Where(x => x.CompanyID == _CompanyID)
        //            .Select(x => new OrderDisplayVM
        //            {
        //                OrderID = x.OrderID,
        //                Order_cd = x.Order_cd,
        //                StatusId = x.StatusId,
        //                StatusName = x.StatusName,
        //                DaysDifference = x.DaysDifference,
        //                RemainingHours = x.RemainingHours,
        //                CurrentDate = x.CurrentDate,
        //                LocationName = x.LocationName,
        //                DepName = x.DepName,
        //                CreateName = x.CreateName,
        //                AssignName = x.AssignName,
        //                StatusSortShow = x.StatusSortShow,
        //                dtCraeteStamp = x.dtCraeteStamp,
        //                DepartmentID =x.DepartmentID,
        //                UserIDAssign=x.UserIDAssign
        //            }).ToListAsync();


        //        if (stid == 0)
        //        {
        //            if (!string.IsNullOrEmpty(searchValue))
        //            {
        //                //Orders = _context.Vmorders
        //                //        .Where(x => x.CompanyID == _CompanyID
        //                //                && !excludedStatusIds.Contains(x.StatusId)
        //                //                && x.DepName.Contains(searchValue)
        //                //                || x.LocationName.Contains(searchValue)
        //                //                || x.CreateName.Contains(searchValue)
        //                //                || x.AssignName.Contains(searchValue)
        //                //                || x.CurrentDate.Contains(searchValue));
        //                Orders =await Orders
        //                        .Where(x => !excludedStatusIds.Contains(x.StatusId)
        //                                && x.DepName.Contains(searchValue)
        //                                || x.LocationName.Contains(searchValue)
        //                                || x.CreateName.Contains(searchValue)
        //                                || x.AssignName.Contains(searchValue)
        //                                || x.CurrentDate.Contains(searchValue));
        //            }
        //            else
        //            {
        //            //    Orders = _context.Vmorders
        //            //               .Where(x => x.CompanyID == _CompanyID
        //            //               && !excludedStatusIds.Contains(x.StatusId)
        //            //               && (x.DepartmentID == UserInfo.DepartmentID
        //            //|| x.UserIDAssign == _UserID)); 
        //                Orders = Orders
        //                           .Where(x =>!excludedStatusIds.Contains(x.StatusId)
        //                           && (x.DepartmentID == UserInfo.DepartmentID || x.UserIDAssign == _UserID));
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(searchValue))
        //            {
        //                //Orders = _context.Vmorders
        //                //        .Where(x => x.CompanyID == _CompanyID && x.StatusId == stid
        //                //                && x.DepName.Contains(searchValue)
        //                //                || x.LocationName.Contains(searchValue)
        //                //                || x.CreateName.Contains(searchValue)
        //                //                || x.AssignName.Contains(searchValue)
        //                //                || x.CurrentDate.Contains(searchValue));
        //                Orders = Orders
        //                        .Where(x => x.StatusId == stid
        //                                && x.DepName.Contains(searchValue)
        //                                || x.LocationName.Contains(searchValue)
        //                                || x.CreateName.Contains(searchValue)
        //                                || x.AssignName.Contains(searchValue)
        //                                || x.CurrentDate.Contains(searchValue));
        //            }
        //            else
        //            {
        //            //    Orders = _context.Vmorders
        //            //       .Where(x => x.CompanyID == _CompanyID && x.StatusId == stid
        //            //       && (x.DepartmentID == UserInfo.DepartmentID
        //            //|| x.UserIDAssign == _UserID));    
        //                Orders = Orders
        //                   .Where(x => x.StatusId == stid
        //                   && (x.DepartmentID == UserInfo.DepartmentID
        //                    || x.UserIDAssign == _UserID));
        //            }
        //        }

        //        //IQueryable<OrderMaster> Orders = _context.OrdersMaster
        //        //                .Where(x => x.CompanyID == _CompanyID && x != null);

        //        //Orders = Orders.OrderBy(x => x.dtCraeteStamp);

        //        Orders = Orders.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");
        //        // Apply sorting based on user's request

        //        //if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        //        //{
        //        //    // Orders = sortColumnDirection == "asc" ? Orders.OrderBy(sortColumn)
        //        //    //     : Orders.OrderBy(sortColumn);
        //        //    Orders = Orders.OrderBy($"{sortColumn} {sortColumnDirection}");
        //        //}
        //        var recordsTotal = Orders.Count();
        //        var data = Orders.Skip(Skip).Take(pageSize).ToList();

        //        var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

        //        return Json(datajson);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = true, returnData = ex.Message });
        //    }

        //}
        [HttpPost]
        public async Task<IActionResult> GetOrdersByStatus(int stid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");
                var UserInfo = await _userManager.GetUserAsync(User);

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var searchValue = Request.Form["search[value]"];

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                var sortColumnIndex2 = Request.Form["order[1][column]"];
                var sortColumn2 = Request.Form[$"columns[{sortColumnIndex2}][name]"];
                var sortColumnDirection2 = Request.Form["order[1][dir]"];

                var excludedStatusIds = new List<int> { 9, 12, 13 };

                // Base query
                var query = _context.Vmorders
                    .Where(x => x.CompanyID == _CompanyID)
                    .AsQueryable();

                // Apply filters
                if (stid != 0)
                {
                    query = query.Where(x => x.StatusId == stid);
                }

                if (!string.IsNullOrEmpty(searchValue))
                {
                    query = query.Where(x => !excludedStatusIds.Contains(x.StatusId)
                        && (x.DepName.Contains(searchValue)
                        || x.LocationName.Contains(searchValue)
                        || x.CreateName.Contains(searchValue)
                        || x.AssignName.Contains(searchValue)
                        || x.CurrentDate.Contains(searchValue)));
                }
                else
                {
                    query = query.Where(x => !excludedStatusIds.Contains(x.StatusId)
                        && (x.DepartmentID == UserInfo.DepartmentID || x.UserIDAssign == _UserID));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(sortColumnIndex2))
                {
                    query = query.OrderBy($"{sortColumn} {sortColumnDirection}")
                                 .ThenBy($"{sortColumn2} {sortColumnDirection2}");
                }
                else
                {
                    query = query.OrderBy($"{sortColumn} {sortColumnDirection}");
                }
                // Get total records count
                var recordsTotal = await query.CountAsync();

                // Fetch paginated data
                var data = await query.Skip(Skip).Take(pageSize).ToListAsync();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        #endregion

        #region "Details Page"
        //[Authorize(Permissions.Dashboard.Edit)]
        public IActionResult Details(string Id)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                CultureInfo cultureInfo = new CultureInfo("en-US");
                ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();


                OrderDetailsVM OrdDetVM = new OrderDetailsVM();

                var OrderMst = _context.Vmorders
                                .AsNoTracking()
                                //.Where(x => x.OrderID == Id && x.CompanyID == _CompanyID)
                                .FirstOrDefault(x => x.OrderID == Id && x.CompanyID == _CompanyID);

                if (OrderMst != null)
                {

                    var pathImage = Path.Combine(_empImagePath, "Companies", _CompanyFolder, "images", "employee");


                    OrdDetVM.OrderID = Id;
                    OrdDetVM.CompanyID = _CompanyID;
                    OrdDetVM.Order_cd = OrderMst.Order_cd;
                    OrdDetVM.dtCraeteDisplay = OrderMst.dtCraeteStamp?.ToString("dd MMMM, yyyy", cultureInfo);// "dd/MM/yyyy");
                    OrdDetVM.AtTime = OrderMst.dtCraeteStamp?.ToString("HH:mm");
                    OrdDetVM.LocationID = OrderMst.LocationID;
                    OrdDetVM.LocationName = OrderMst.LocationName;
                    OrdDetVM.DepartmentID = OrderMst.DepartmentID;
                    OrdDetVM.DepName = OrderMst.DepName;

                    OrdDetVM.UserIDCreate = OrderMst.UserIDCreate;
                    OrdDetVM.CreateName = OrderMst.CreateName;

                    OrdDetVM.DaysDifference = OrderMst.DaysDifference;
                    OrdDetVM.RemainingHours = OrderMst.RemainingHours;

                    if (string.IsNullOrEmpty(OrderMst.CreatePic))
                    {
                        //OrdDetVM.CreatePic = OrderMst.CreatePic ? Path.Combine(pathImage, OrderMst.CreatePic) : Path.Combine(pathImage, OrderMst.CreatePic);
                        //OrdDetVM.CreatePic = Path.Combine(pathImage, "8b7706b4-c412-4a72-b224-8b3e0d09a122.png");//_empImagePath, _empAVATAR);
                        //OrdDetVM.CreatePic = Path.Combine("~", "Companies", _CompanyFolder, "images", "employee", "8b7706b4-c412-4a72-b224-8b3e0d09a122.png");//_empImagePath, _empAVATAR);
                        OrdDetVM.CreatePic = $@"../../{_empAVATAR}";//, _empAVATAR);// _CompanyFolder, "/images/employee/");//_empImagePath, _empAVATAR);
                    }
                    else
                    {
                        //OrdDetVM.CreatePic = Path.Combine(_empImagePath, OrderMst.CreatePic);
                        OrdDetVM.CreatePic = Path.Combine(@"../../companies", _CompanyFolder, @"images/employee/", OrderMst.CreatePic);
                    }
                    //OrdDetVM.CreatePic = Path.Combine("../../companies/hjcont/images/employee/aa578b6a-fe70-410e-8966-489807368670.png");

                    OrdDetVM.DepartmentAssignUserId = OrderMst.DepartmentAssignUserId;
                    OrdDetVM.DepartmentAssignName = OrderMst.DepartmentAssignName;
                    if (string.IsNullOrEmpty(OrderMst.DepartmentAssignPic))
                    {
                        //OrdDetVM.CreatePic = OrderMst.CreatePic ? Path.Combine(pathImage, OrderMst.CreatePic) : Path.Combine(pathImage, OrderMst.CreatePic);
                        OrdDetVM.DepartmentAssignPic = $@"../../{_empAVATAR}";// Path.Combine(_empImagePath, _empAVATAR);
                    }
                    else
                    {
                        //OrdDetVM.DepartmentAssignPic = Path.Combine(pathImage, OrderMst.DepartmentAssignPic);
                        OrdDetVM.DepartmentAssignPic = Path.Combine(@"../../companies", _CompanyFolder, @"images/employee/", OrderMst.DepartmentAssignPic);
                    }

                    OrdDetVM.UserIDAssign = OrderMst.UserIDAssign;
                    OrdDetVM.AssignName = OrderMst.AssignName;
                    if (string.IsNullOrEmpty(OrderMst.AssignPic))
                    {
                        //OrdDetVM.CreatePic = OrderMst.CreatePic ? Path.Combine(pathImage, OrderMst.CreatePic) : Path.Combine(pathImage, OrderMst.CreatePic);
                        OrdDetVM.AssignPic = $@"../../{_empAVATAR}";// Path.Combine(_empImagePath, _empAVATAR);
                    }
                    else
                    {
                        // OrdDetVM.AssignPic = Path.Combine(pathImage, OrderMst.AssignPic);
                        OrdDetVM.AssignPic = Path.Combine(@"../../companies", _CompanyFolder, @"images/employee/", OrderMst.AssignPic);
                    }

                    //OrdDetVM.AssigneesName = OrderMst.AssignName;
                    //OrdDetVM.AssigneesPic = OrderMst.AssignPic;

                    //OrdDetVM.UserIDAction = OrderMs.UserIDAction;
                    //OrdDetVM.AssigneesActionName = OrderMs.UserIDAction;
                    //OrdDetVM.AssigneesActionPic = OrderMs.UserIDAction;

                    //OrdDetVM.RunTime
                    OrdDetVM.StatusId = OrderMst.StatusId;
                    //OrdDetVM.bgClass = GeneralFun.GetbgColorClass(OrderMst.StatusId);
                    OrdDetVM.StatusName = OrderMst.StatusName;
                    OrdDetVM.sNotes = OrderMst.sNotes;

                    OrdDetVM.CountCurrent = 0;
                    OrdDetVM.CountItems = 0;
                    OrdDetVM.Percent = 0;
                    // edit aziz
                   OrdDetVM.SupervisorUserAssign = OrderMst.SupervisorUserAssign;

                    //var OrderDet = _context.OrdersDet
                    //                .AsNoTracking()
                    //                .Where(x => x.OrderID == Id && x.CompanyID == _CompanyID)
                    //                .OrderBy(x => x.Order_Det)
                    //                .ToList();

                    var OrderDetitem = _context.OrdersDet
                            .Where(x => x.OrderID == Id && x.CompanyID == _CompanyID)
                            .OrderBy(x => x.Order_Det)
                            .Join(
                            _context.Items,
                            Itemord => Itemord.ItemID,
                            itemDet => itemDet.ItemID,
                            (ItemOrd, itemDet) => new OrdersDetailItems
                            {
                                ItemID = itemDet.ItemID,
                                ItemName_E = itemDet.ItemName_E,
                                ItemName_A = itemDet.ItemName_A,
                                Order_Det = ItemOrd.Order_Det,
                                OrderID = ItemOrd.OrderID,
                                CompanyID = ItemOrd.CompanyID,
                                Qty = ItemOrd.Qty,
                                sItemNotes = ItemOrd.sItemNotes,
                                isClosed = ItemOrd.isClosed,
                                dtColsed = ItemOrd.dtColsed,
                                UserClosed = ItemOrd.UserClosed,
                                DepartmentID = itemDet.DepartmentID,
                                DepartmentItemName = itemDet.DepartmentID,
                                ItemType = itemDet.ItemType,
                                ItemStock = itemDet.ItemStock
                            }).ToList();

                    if (OrderDetitem != null)
                    {
                        var itemCountClosed = _context.OrdersDet
                                .AsNoTracking()
                                .Where(x => x.OrderID == Id && x.CompanyID == _CompanyID && x.isClosed == 1)
                                .Count();

                        OrdDetVM.OrderDetItem = OrderDetitem;

                        OrdDetVM.CountCurrent = itemCountClosed;
                        OrdDetVM.CountItems = OrderDetitem.Count;

                        if (OrderDetitem.Count == 0)
                        {
                            OrdDetVM.Percent = 0; // Handle the case where OrderDet.Count is zero to prevent division by zero.
                        }
                        else
                        {
                            OrdDetVM.Percent = (int)((double)itemCountClosed / OrderDetitem.Count * 100);
                        }

                    }

                    ///Order Action
                    ///
                    //var defaultValue = "";
                    var OrderActionDet = _context.OrderActionVM
                            .Where(x => x.CompanyID == _CompanyID && x.OrderID == Id)
                            .OrderBy(x => x.SerialID)
                            //.Select(act => new OrderActionViewModel
                            //{
                            //    SerGUID = act.SerGUID ?? defaultValue,
                            //    OrderID = act.OrderID,
                            //    CompanyID = act.CompanyID,
                            //    dtAction = act.dtAction,
                            //    sNotes = act.sNotes ?? defaultValue,
                            //    Satatus = act.Satatus,
                            //    UserIDAction = act.UserIDAction ?? defaultValue,
                            //    ToEmp = act.ToEmp ?? defaultValue,
                            //    ToDepartment = act.ToDepartment ?? defaultValue,
                            //    DepName_E = act.DepName_E ?? defaultValue,
                            //    DepName_A = act.DepName_A ?? defaultValue,
                            //    //isDeleted = act.isDeleted ,
                            //    CreatedName = act.CreatedName ?? defaultValue,
                            //    CreatedDep = act.CreatedDep ?? defaultValue,
                            //    AssignName = act.AssignName ?? defaultValue,
                            //    AssignDep = act.AssignDep ?? defaultValue,
                            //    DaysDifference = act.DaysDifference,
                            //    RemainingHours = act.RemainingHours,
                            //    CurrentDate = act.CurrentDate ?? defaultValue
                            //})
                            .ToList();
                    if (OrderActionDet != null)
                    {
                        OrdDetVM.OrderAction = OrderActionDet;
                    }

                    ///Order Images mistake

                    var OrderMistake = _context.AttachmentFiles
                            .Where(x => x.CompanyID == _CompanyID && x.OrderID == Id)
                            .ToList();

                    if (OrderMistake != null)
                    {
                        OrdDetVM.MistakePics = OrderMistake;
                    }
                }

                return View(OrdDetVM);
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }
        public IActionResult GetEmpDepartment(string dep = "0")
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                // var empDep;
                List<SelectListItem> empDep;

                if (dep == "0")
                {
                    //empDep = _context.Employees
                    //    .AsNoTracking()
                    //    .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0 && x.bActive == 1)
                    //    .OrderBy(x => x.FirstName)
                    //    .Select(x => new SelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.EmpID })
                    //    .ToList(); 

                    empDep = _userManager.Users
                        .AsNoTracking()
                        .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.bActive == 1)
                        .OrderBy(x => x.FirstName)
                        .Select(x => new SelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.Id })
                        .ToList();
                }
                else
                {
                    //empDep = _context.Employees
                    //        .AsNoTracking()
                    //        .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0 && x.bActive == 1 && x.DepartmentID == dep)
                    //        .OrderBy(x => x.FirstName)
                    //        .Select(x => new SelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.EmpID })
                    //        .ToList(); 
                    empDep = _userManager.Users
                           .AsNoTracking()
                           .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.bActive == 1 && x.DepartmentID == dep)
                           .OrderBy(x => x.FirstName)
                           .Select(x => new SelectListItem { Text = (x.FirstName + " " + x.LastName), Value = x.Id })
                           .ToList();
                }


                var datajson = new { success = true, returnData = empDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        //Details View Action
        [HttpPost]
        public IActionResult TakeAction(string OrderId)
        {

            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (empDep != null)
                    {
                        empDep.UserIDAssign = _UserID;
                        empDep.Status = (int)enumStatus.Inprocess;
                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();
                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Inprocess, "", _UserID, "Take employee order");
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
            return View();
        }
        [HttpPost]
        public IActionResult CompleteOrder(string OrderId)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (empDep != null)
                    {
                        empDep.UserIDAssign = _UserID;
                        empDep.Status = (int)enumStatus.Completed;
                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Completed, "", _UserID, $"Completed order > {UserName}");
                        GeneralFun.UpdateOrderQuantity(OrderId, _CompanyID, _context);
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult CloseOrder(string OrderId)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (empDep != null)
                    {
                        empDep.UserIDAssign = _UserID;
                        empDep.Status = (int)enumStatus.Closed;
                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Closed, "", _UserID, $"Closed order > {UserName}");
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult ReopenOrder(string OrderId)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (empDep != null)
                    {
                        empDep.UserIDAssign = _UserID;
                        empDep.Status = (int)enumStatus.Open;
                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.ReOpen, "", _UserID, $"Reopen order > {UserName}");
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult HoldOrder(string OrderId)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId))//&& !string.IsNullOrEmpty(EmpId))
                {
                    var OrderData = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (OrderData != null)
                    {
                        OrderData.UserIDAssign = _UserID;
                        OrderData.Status = (int)enumStatus.Hold;
                        _context.OrdersMaster.Update(OrderData);
                        _context.SaveChanges();

                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Hold, "", _UserID, $"Hold order > {UserName}");
                    }
                }

                var datajson = new { success = true, returnData = "save Hold" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        //[Authorize(Permissions.CreateOrders.Edit)]
        public IActionResult ChangeEmployee(string OrderId, string EmpId)
        {

            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(EmpId))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);

                    if (empDep != null)
                    {
                        empDep.UserIDAssign = EmpId;

                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        // [SerialID]
                        //,[SerGUID]
                        //,[OrderID]
                        //,[CompanyID]
                        //,[dtAction]
                        //,[sNotes]
                        //,[Satatus]
                        //,[UserIDAction]
                        //,[ToEmp]
                        //,[ToDepartment]

                        //AddActionRecord(OrderId, _CompanyID,Note,statusId,_UserId, EmpId, ToDepartment)

                        //OrderAction orderAction = new OrderAction
                        //{
                        //    SerGUID = Guid.NewGuid().ToString(),
                        //    OrderID = OrderId,
                        //    CompanyID = _CompanyID,
                        //    dtAction = DateTime.Now,
                        //    sNotes = "",
                        //    Satatus = 1,
                        //    UserIDAction = _UserID,
                        //    ToDepartment = "",
                        //    ToEmp = EmpId
                        //};
                        //_context.OrdersAction.Add(orderAction);
                        //_context.SaveChanges();
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, 1, "", EmpId, "Change employee");
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Permissions.CreateOrders.Edit)]
        public IActionResult ChangeDepartment(string OrderId, string DepartmentID)
        {

            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(DepartmentID))
                {
                    var empDep = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                    if (empDep != null)
                    {
                        empDep.DepartmentID = DepartmentID;

                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        // [SerialID]
                        //,[SerGUID]
                        //,[OrderID]
                        //,[CompanyID]
                        //,[dtAction]
                        //,[sNotes]
                        //,[Satatus]
                        //,[UserIDAction]
                        //,[ToEmp]
                        //,[ToDepartment]
                        //AddActionRecord(OrderId, _CompanyID,Note,statusId,_UserId, EmpId, ToDepartment)
                        //OrderAction orderAction = new OrderAction
                        //{
                        //    SerGUID = Guid.NewGuid().ToString(),
                        //    OrderID = OrderId,
                        //    CompanyID = _CompanyID,
                        //    dtAction = DateTime.Now,
                        //    sNotes = "",
                        //    Satatus = 1,
                        //    UserIDAction = _UserID,
                        //    ToDepartment = DepartmentID,
                        //    ToEmp = ""
                        //};

                        //_context.OrdersAction.Add(orderAction);
                        //_context.SaveChanges();
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, 1, DepartmentID, "", "Change Department");
                    }
                }

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Permissions.CreateOrders.Edit)]
        public IActionResult ChangeDefer(string OrderId, string inputdate, string inputtime)
        {

            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (!string.IsNullOrEmpty(OrderId) || !string.IsNullOrEmpty(inputdate) || !string.IsNullOrEmpty(inputtime))
                {
                    // Combine date and time strings into a single string in "yyyy-MM-dd HH:mm:ss" format
                    var combinedDateTimeString = $"{inputdate} {inputtime}";
                    // Parse the combined string into a DateTime object
                    if (DateTime.TryParseExact(combinedDateTimeString, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime delayTime))

                    {


                        var deferOrder = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                        if (deferOrder != null)
                        {

                            deferOrder.bDelay = 1;
                            deferOrder.DelayTime = delayTime;
                            deferOrder.Status = 9;

                            _context.OrdersMaster.Update(deferOrder);
                            _context.SaveChanges();

                            GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, 1, "", "", $"defered to {delayTime}");
                        }
                    }
                }

                var datajson = new { success = true, returnData = "change Defered" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Permissions.CreateOrders.Edit)]
        public IActionResult addNoteforOrder(string OrderId, string NoteOrder)
        {

            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                if (!string.IsNullOrEmpty(OrderId) && !string.IsNullOrEmpty(NoteOrder))
                {
                    var orderNote = _context.OrdersMaster
                         .FirstOrDefault(x => x.CompanyID == _CompanyID && x.OrderID == OrderId);
                    if (orderNote != null)
                    {
                        orderNote.sNotes = NoteOrder;

                        _context.OrdersMaster.Update(orderNote);
                        _context.SaveChanges();
                        //OrderAction orderAction = new OrderAction
                        //{
                        //    SerGUID = Guid.NewGuid().ToString(),
                        //    OrderID = OrderId,
                        //    CompanyID = _CompanyID,
                        //    dtAction = DateTime.Now,
                        //    sNotes = "",
                        //    Satatus = 1,
                        //    UserIDAction = _UserID,
                        //    ToDepartment = DepartmentID,
                        //    ToEmp = ""
                        //};
                        //_context.OrdersAction.Add(orderAction);

                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, 1, "", "", NoteOrder);

                    }
                }
                var htmlActivty = DrawAction(OrderId);
                var datajson = new { success = true, returnData = "Saved", listActivity = htmlActivty };
                // var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult DrawActivity(string OrderId)
        {

            try
            {
                var htmlActivty = DrawAction(OrderId);
                var datajson = new { success = true, listActivity = htmlActivty };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        public string DrawAction(string Id)
        {

            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                string resulthtml = string.Empty;

                var OrderActionDet = _context.OrderActionVM
                    .Where(x => x.CompanyID == _CompanyID && x.OrderID == Id)
                    .OrderByDescending(x => x.SerialID)
                    .ToList();

                if (OrderActionDet != null && OrderActionDet.Any())
                {
                    foreach (var item in OrderActionDet)
                    {
                        var timeAgo = "";
                        if (item.DaysDifference > 0)
                        {
                            timeAgo = item.DaysDifference == 1 ? $"{item.DaysDifference} Day " : $"{item.DaysDifference} Days ";

                            //if (item.DaysDifference == 1)
                            //{
                            //    timeAgo = $"{item.DaysDifference} Day ";
                            //}
                            //else
                            //{
                            //    timeAgo = $"{item.DaysDifference} Days ";
                            //}
                        }
                        timeAgo += $"{item.RemainingHours} hours ";

                        var tit = " => " + item.CreatedName;

                        resulthtml += $"<div class='list-group-item list-group-item-action'><div class='d-flex w-100 justify-content-between'>";
                        resulthtml += $"<h5 class='mb-1 title'>{item.sNotes}</h5>";
                        resulthtml += $"<small>{timeAgo}</small></div>";
                        resulthtml += $"<small class='note'>{Convert.ToDateTime(item.dtAction).ToString("dd/MM/yyyy HH:mm")}</small>";
                        resulthtml += $"<small class='note'>{tit}</small></div>";


                    }
                }
                return resulthtml;
            }
            catch (Exception)
            {

                return "";
            }

        }
        #endregion
        #region "Public Page"
        [HttpPost]
        public IActionResult FilterDailogApply(string objData)
        {
            try
            {
                //string OrdernumberFrom = "";
                //string OrdernumberTo = "";
                //string RuntimeFrom = "";
                //string RuntimeTo = "";
                string DateFrom = "";
                string DateTo = "";
                //string LocationId = "";
                //string LocationName = "";

                string StatusCondation = "";

                FilterDashboardVM fddata = JsonConvert.DeserializeObject<FilterDashboardVM>(objData);

                if (fddata.StatusOpen == 1)
                {
                    StatusCondation = $"{(int)enumStatus.Open},";
                }
                if (fddata.StatusHold == 1)
                {
                    StatusCondation += $"{(int)enumStatus.Hold},";
                }
                if (fddata.StatusClosed == 1)
                {
                    StatusCondation += $"{(int)enumStatus.Closed},";
                }
                if (fddata.StatusInprocess == 1)
                {
                    StatusCondation += $"{(int)enumStatus.Inprocess},";
                }
                if (fddata.StatusCompleted == 1)
                {
                    StatusCondation += $"{(int)enumStatus.Completed}";
                }
                else
                {
                    if (!string.IsNullOrEmpty(StatusCondation))
                    {
                        StatusCondation = StatusCondation.Substring(0, StatusCondation.Length - 1);
                    }
                }


                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var searchValue = Request.Form["search[value]"];

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<VMOrders> Orders;

                string mySQL = "";


                mySQL = $"SELECT * FROM Vmorders WHERE CompanyID=N'{_CompanyID}'";
                if (!string.IsNullOrEmpty(StatusCondation))
                {
                    mySQL += $" AND statusid in ({StatusCondation}) ";
                }
                if (!string.IsNullOrEmpty(fddata.OrdernumberFrom) && !string.IsNullOrEmpty(fddata.OrdernumberTo))
                {
                    mySQL += $" AND order_cd between {fddata.OrdernumberFrom} and  {fddata.OrdernumberTo}";
                }
                if (!string.IsNullOrEmpty(fddata.DateFrom) && !string.IsNullOrEmpty(fddata.DateTo))
                {
                    var df = Convert.ToDateTime(fddata.DateFrom);
                    var dt = Convert.ToDateTime(fddata.DateTo);
                    DateFrom = df.Year.ToString() + (df.Month > 9 ? df.Month.ToString() : "0" + df.Month.ToString()) + (df.Day > 9 ? df.Day.ToString() : "0" + df.Day.ToString());
                    DateTo = dt.Year.ToString() + (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString()) + (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString());

                    mySQL += $" AND dtCraete between {DateFrom} and  {DateTo}";
                }

                //Orders = _context.Vmorders.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}");
                Orders = _context.Vmorders
                    .FromSqlRaw(mySQL)
                    .OrderBy(o => o.StatusSortShow)
                    .ThenByDescending(o => o.dtCraeteStamp)
                    .ThenBy(o => o.DepName);

                //Orders = Orders.OrderBy($"{sortColumn} {sortColumnDirection}");
                // Apply sorting based on user's request

                var recordsTotal = Orders.Count();
                var data = Orders.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }

            catch (Exception ex)
            {

                return Json(new { success = true, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult SearchOrders(string objData)
        {
            try
            {

                FilterDashboardVM fddata = JsonConvert.DeserializeObject<FilterDashboardVM>(objData);


                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                //var searchValue = Request.Form["search[value]"];

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<VMOrders> Orders;

                if (string.IsNullOrEmpty(fddata.textSearch))
                {
                    return Json(new { success = false, returnData = "" });
                }
                var searchValue = fddata.textSearch;

                Orders = _context.Vmorders
                    .Where(x => x.CompanyID == _CompanyID
                            && (x.DepName.Contains(searchValue)
                            || x.LocationName.Contains(searchValue)
                            || x.CreateName.Contains(searchValue)
                            || x.AssignName.Contains(searchValue)
                            || x.CurrentDate.Contains(searchValue)
                            || x.Order_cd.ToString().Contains(searchValue)))
                    .OrderBy(o => o.StatusSortShow)
                    .ThenByDescending(o => o.dtCraeteStamp)
                    .ThenBy(o => o.DepName);

                //string mySQL = "";
                //mySQL = $"SELECT * FROM Vmorders WHERE CompanyID=N'{_CompanyID}'";
                //if (!string.IsNullOrEmpty(StatusCondation))
                //{
                //    mySQL += $" AND statusid in ({StatusCondation}) ";
                //}
                //if (!string.IsNullOrEmpty(fddata.OrdernumberFrom) && !string.IsNullOrEmpty(fddata.OrdernumberTo))
                //{
                //    mySQL += $" AND order_cd between {fddata.OrdernumberFrom} and  {fddata.OrdernumberTo}";
                //}
                //if (!string.IsNullOrEmpty(fddata.DateFrom) && !string.IsNullOrEmpty(fddata.DateTo))
                //{
                //    var df = Convert.ToDateTime(fddata.DateFrom);
                //    var dt = Convert.ToDateTime(fddata.DateTo);
                //    DateFrom = df.Year.ToString() + (df.Month > 9 ? df.Month.ToString() : "0" + df.Month.ToString()) + (df.Day > 9 ? df.Day.ToString() : "0" + df.Day.ToString());
                //    DateTo = dt.Year.ToString() + (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString()) + (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString());

                //    mySQL += $" AND dtCraete between {DateFrom} and  {DateTo}";
                //}

                //Orders = _context.Vmorders.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}");

                //Orders = Orders.OrderBy($"{sortColumn} {sortColumnDirection}");
                // Apply sorting based on user's request

                var recordsTotal = Orders.Count();
                var data = Orders.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }

            catch (Exception ex)
            {

                return Json(new { success = true, returnData = ex.Message });
            }
        }
        #endregion
    }
}
