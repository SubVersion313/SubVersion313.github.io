//using DotVVM.Framework.Controls;
//using Humanizer;
//using DocumentFormat.OpenXml.Bibliography;
using LodgeMasterWeb.Contants;
using LodgeMasterWeb.Core.Models;
using LodgeMasterWeb.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using SQLitePCL;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;
        private readonly string _empAVATAR = @"Images/avatar/bg-man.jpg";

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = UserManager;
            _webHostEnvironment = webHostEnvironment;
            _empImagePath = _webHostEnvironment.WebRootPath;
        }

        #region "Dashboard Page"
        [Authorize(Permissions.Dashboard.View)]
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
        public async Task<IActionResult> GetItemsQty()
        {
            try
            {
                return Json("");
            }
            catch (Exception)
            {
                return Json("");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCountOrders()
        {
            // يرجع بعدد العمليات لكل حالة
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                // var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                // var _UserID = HttpContext.Session.GetString("UserID");

                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                var CountOrders = _context.OrdersMaster
                    .Where(x => x.CompanyID == _CompanyID)
                    .GroupBy(x => x.Status)
                    .Select(group => new
                    {
                        statusid = group.Key,
                        total = group.Count()
                    });
                //.ToList();

                return Json(new { success = true, returnData = CountOrders });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrdersByStatus(int stid)
        {
            try
            {
                var companyId = HttpContext.Session.GetString("CompanyID");
                var companyFolder = HttpContext.Session.GetString("CompanyFolder");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var searchValue = Request.Form["search[value]"];

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                var sortColumnIndex2 = Request.Form["order[1][column]"];
                var sortColumn2 = Request.Form[$"columns[{sortColumnIndex2}][name]"];
                var sortColumnDirection2 = Request.Form["order[1][dir]"];
                
                var sortColumnIndex3 = Request.Form["order[1][column]"];
                var sortColumn3 = Request.Form[$"columns[{sortColumnIndex3}][name]"];
                var sortColumnDirection3 = Request.Form["order[1][dir]"];

                //var query = from order in _context.OrdersMaster
                //            join status in _context.SysStatus on order.Status equals status.StatusID into statusJoin
                //            from status in statusJoin.DefaultIfEmpty()
                //            join location in _context.CompanyUnits on order.LocationID equals location.LocationID into locationJoin
                //            from location in locationJoin.DefaultIfEmpty()
                //            join department in _context.Departments on order.DepartmentID equals department.DepartmentID into departmentJoin
                //            from department in departmentJoin.DefaultIfEmpty()
                //            select new
                //            {
                //                order.OrderID,
                //                order.Order_cd,
                //                order.dtCraeteStamp,
                //                order.LocationID,
                //                order.DepartmentID,
                //                order.sNotes,
                //                StatusId = order.Status,
                //                LocationName = location.LocationName,
                //                StatusName = status.Status_E,
                //                DepName = department.DepName_E,
                //                DaysDifference = GeneralFun.GetTotalDiffByType(order.dtCraeteStamp, 'D'),
                //                RemainingHours =GeneralFun.GetTotalDiffByType(order.dtCraeteStamp, 'H'),
                //                Mintues = GeneralFun.GetTotalDiffByType(order.dtCraeteStamp, 'M'),
                //                CurrentDate = order.dtCraete.ToString("dd/MM/yyyy"), // Assuming dtCraete is of type DateTime
                //                order.DelayTime,
                //                order.bDelay,
                //                status.StatusSortShow
                //            };

                //var result = await query.ToListAsync();
                var ordersQuery = _context.Vmorders.Where(x => x.CompanyID == companyId);

                if (!string.IsNullOrEmpty(searchValue))
                {
                    ordersQuery = ordersQuery.Where(x =>
                        x.DepName.Contains(searchValue) ||
                        x.LocationName.Contains(searchValue) ||
                        x.CreateName.Contains(searchValue) ||
                        x.AssignName.Contains(searchValue) ||
                        x.CurrentDate.Contains(searchValue)
                    );
                }

                if (stid != 0)
                {
                    ordersQuery = ordersQuery.Where(x => x.StatusId == stid);
                }

                //var orders = ordersQuery.Select(x => new OrderDisplayVM
                //{
                //    OrderID = x.OrderID,
                //    Order_cd = x.Order_cd,
                //    StatusId = x.StatusId,
                //    StatusName = x.StatusName,
                //    DaysDifference = x.DaysDifference,
                //    RemainingHours = x.RemainingHours,
                //    CurrentDate = x.CurrentDate,
                //    LocationName = x.LocationName,
                //    DepName = x.DepName,
                //    CreateName = x.CreateName,
                //    AssignName = x.AssignName,
                //    StatusSortShow = x.StatusSortShow,
                //    dtCraeteStamp = x.dtCraeteStamp
                //});

                //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

                // Apply sorting
                //في حالة هناك طلب defer يتم عرضه في المقدمة

                //var anyDefer = false;// await ordersQuery.AnyAsync(x => x.StatusId == 13);
                //if (anyDefer==true)//defer
                
                if (!string.IsNullOrEmpty(sortColumnIndex3))
                {
                    ordersQuery = ordersQuery.OrderBy($"{sortColumn3} {sortColumnDirection3}")
                        .ThenBy($"{sortColumn} {sortColumnDirection}")
                        .ThenBy($"{sortColumn2} {sortColumnDirection2}");
                }
                else if (!string.IsNullOrEmpty(sortColumnIndex2))
                {
                    ordersQuery = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}")
                                             .ThenBy($"{sortColumn2} {sortColumnDirection2}");
                }
                else
                {
                    ordersQuery = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}");
                }

                 
                // Apply sorting
                //if (!string.IsNullOrEmpty(sortColumnIndex2))
                //{
                //    ordersQuery = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}")
                //                             .ThenBy($"{sortColumn2} {sortColumnDirection2}");
                //}
                //else
                //{
                //ordersQuery = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}");
                //}


                //var recordsTotal = await orderedOrders.CountAsync();
                //var data = await orderedOrders.Skip(start).Take(pageSize).ToListAsync();
                var recordsTotal = await ordersQuery.CountAsync();
                var data = await ordersQuery.Skip(Skip).Take(pageSize).ToListAsync();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, returnData = ex.Message });
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
                    OrdDetVM.bgClass = GeneralFun.GetbgColorClass(OrderMst.StatusId);
                    OrdDetVM.StatusName = OrderMst.StatusName;

                    OrdDetVM.sNotes = OrderMst.sNotes;

                    OrdDetVM.CountCurrent = 0;
                    OrdDetVM.CountItems = 0;
                    OrdDetVM.Percent = 0;
                    //edit aziz
                    OrdDetVM.SupervisorUserAssign = OrderMst.SupervisorUserAssign;
                    OrdDetVM.ForSuperviser = OrderMst.ForSuperviser;

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
                                ItemStock = itemDet.ItemStock,
                                CompanyFolder = _CompanyFolder,
                                SubFolder = itemDet.isInspection == 1 ? "inspection" : "orders",
                                PicName = string.IsNullOrEmpty(ItemOrd.PhotoName) ? "mistik.png" : ItemOrd.PhotoName
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
        public IActionResult TakeOrder(string OrderId)
        {
            try
            {
                //Aziz
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");
                // bool DoneAction = false;

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
                        empDep.ForSuperviser = 0;

                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();
                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Inprocess, "", _UserID, $"Take employee order > {UserName}");
                        //DoneAction = true;
                    }
                }
                //OrderDetailsVM updata = null;
                //if (DoneAction==true)
                //{
                //updata = UpdateDetails(OrderId);
                //}

                var datajson = new { success = true, returnData = "Saved" };
                return Json(datajson);

                //return RedirectToAction(nameof(Details),"Dashboard", new { Id = OrderId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult CompleteOrder(string OrderId, string Notes)
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
                        empDep.ForSuperviser = 0;
                        _context.OrdersMaster.Update(empDep);
                        _context.SaveChanges();

                        var UserName = "";
                        var UserData = _userManager.Users.FirstOrDefault(u => u.CompanyID == _CompanyID && u.Id == _UserID);

                        if (UserData != null)
                        {
                            UserName = UserData.FirstName + " " + UserData.LastName;
                        }
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Completed, "", _UserID, $"Completed order > {UserName}");
                    }
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Completed, "", "", Notes);
                    }
                    var datajson = new { success = true, returnData = "Saved" };
                    return Json(datajson);
                }
                else
                {
                    var datajson = new { success = false, returnData = "" };
                    return Json(datajson);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult CloseOrder(string OrderId, string Notes)
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
                        empDep.ForSuperviser = 0;
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
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Closed, "", "", Notes);
                    }
                    var datajson = new { success = true, returnData = "Saved" };
                    return Json(datajson);
                }
                else
                {
                    var datajson = new { success = false, returnData = "" };
                    return Json(datajson);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult ReopenOrder(string OrderId, string Notes)
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
                        empDep.ForSuperviser = 1;
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
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.ReOpen, "", "", Notes);
                    }
                    var datajson = new { success = true, returnData = "Saved" };
                    return Json(datajson);
                }
                else
                {
                    var datajson = new { success = false, returnData = "" };
                    return Json(datajson);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        [HttpPost]
        public IActionResult HoldOrder(string OrderId, string Notes)
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
                        OrderData.ForSuperviser = 0;
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
                    if (!string.IsNullOrEmpty(Notes))
                    {
                        GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, (int)enumStatus.Hold, "", "", Notes);
                    }
                    var datajson = new { success = true, returnData = "Saved" };
                    return Json(datajson);
                }
                else
                {
                    var datajson = new { success = false, returnData = "" };
                    return Json(datajson);
                }
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
                        empDep.ForSuperviser = 0;

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
                        empDep.ForSuperviser = 1;
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
                            deferOrder.Status = 13;

                            _context.OrdersMaster.Update(deferOrder);
                            _context.SaveChanges();

                            GeneralFun.RecordLogOrdersAction(OrderId, _UserID, _CompanyID, _context, 13, "", "", $"defer to {delayTime}");
                        }

                        return Json(new { success = true, returnData = "change Defered" });
                    }

                }
                return Json(new { success = false, returnData = "" });
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

        public IActionResult newdashboard()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> HousekeepingChart(int startDate, int endDate, string typePeriod)
        {
            try
            {
                var companyId = HttpContext.Session.GetString("CompanyID");

                // تأكد من أن companyId ليس null
                if (string.IsNullOrEmpty(companyId))
                {
                    return Json(new { success = false, returnData = "Company ID is not available in session." });
                }

                string MaintenanceID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "Maintenance").DepartmentID;
                string HouseKeepingID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "HouseKeeping").DepartmentID;
                                
                string  sqlStatment = "";
                string format = "yyyy-MM-dd"; // الصيغة المطلوبة

                if (typePeriod == "0") // Today
                {

                // الفترات الزمنية الثابتة
                var timePeriods = new List<string> { "00-04", "04-08", "08-12", "12-16", "16-20", "20-24" };

                 sqlStatment = @"SELECT 
                                CASE 
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 0 AND DATEPART(HOUR, O.dtCraeteStamp) < 4 THEN '00-04'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 4 AND DATEPART(HOUR, O.dtCraeteStamp) < 8 THEN '04-08'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 8 AND DATEPART(HOUR, O.dtCraeteStamp) < 12 THEN '08-12'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 12 AND DATEPART(HOUR, O.dtCraeteStamp) < 16 THEN '12-16'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 16 AND DATEPART(HOUR, O.dtCraeteStamp) < 20 THEN '16-20'
                                    ELSE '20-24'
                                END AS TimePeriod,
                                COUNT(*) AS CountData
                                FROM OrdersMaster O
                                WHERE o.dtCraete Between @StartDate And @EndDate
                                    AND CompanyID=@CompanyID
                                    AND O.DepartmentID = @DepartmentID
                                GROUP BY CASE 
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 0 AND DATEPART(HOUR, O.dtCraeteStamp) < 4 THEN '00-04'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 4 AND DATEPART(HOUR, O.dtCraeteStamp) < 8 THEN '04-08'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 8 AND DATEPART(HOUR, O.dtCraeteStamp) < 12 THEN '08-12'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 12 AND DATEPART(HOUR, O.dtCraeteStamp) < 16 THEN '12-16'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 16 AND DATEPART(HOUR, O.dtCraeteStamp) < 20 THEN '16-20'
                                    ELSE '20-24'
                                END
                                ORDER BY TimePeriod";

                // بيانات قسم HouseKeeping
                var dataForHousKeeping = await _context.ChartLinesVM
                    .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                            new SqlParameter("@DepartmentID", HouseKeepingID),
                                            new SqlParameter("@StartDate", startDate),
                                            new SqlParameter("@EndDate", endDate))
                    .ToListAsync();

                // ملء الفترات الزمنية المفقودة بالقيمة 0
                var Arrhouskeeping = timePeriods.Select(tp => new
                {
                    x = tp,
                    y = dataForHousKeeping.FirstOrDefault(d => d.TimePeriod == tp)?.CountData ?? 0
                }).ToList();

                // بيانات قسم Maintenance
                var dataForMaintince = await _context.ChartLinesVM
                    .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                            new SqlParameter("@DepartmentID", MaintenanceID),
                                            new SqlParameter("@StartDate", startDate),
                                            new SqlParameter("@EndDate", endDate))
                    .ToListAsync();

                var ArrMaintince = timePeriods.Select(tp => new
                {
                    x = tp,
                    y = dataForMaintince.FirstOrDefault(d => d.TimePeriod == tp)?.CountData ?? 0
                }).ToList();

                // بيانات الطلبات المكتملة (Status = 2)
                 sqlStatment = @"SELECT 
                                CASE 
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 0 AND DATEPART(HOUR, O.dtCraeteStamp) < 4 THEN '00-04'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 4 AND DATEPART(HOUR, O.dtCraeteStamp) < 8 THEN '04-08'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 8 AND DATEPART(HOUR, O.dtCraeteStamp) < 12 THEN '08-12'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 12 AND DATEPART(HOUR, O.dtCraeteStamp) < 16 THEN '12-16'
                                    WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 16 AND DATEPART(HOUR, O.dtCraeteStamp) < 20 THEN '16-20'
                                    ELSE '20-24'
                                END AS TimePeriod,
                                COUNT(*) AS CountData
                                FROM OrdersMaster O
                                WHERE o.dtCraete Between @StartDate And @EndDate
                                    AND o.CompanyID=@CompanyID
                                    AND O.Status = @Status
                                GROUP BY CASE 
                                        WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 0 AND DATEPART(HOUR, O.dtCraeteStamp) < 4 THEN '00-04'
                                        WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 4 AND DATEPART(HOUR, O.dtCraeteStamp) < 8 THEN '04-08'
                                        WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 8 AND DATEPART(HOUR, O.dtCraeteStamp) < 12 THEN '08-12'
                                        WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 12 AND DATEPART(HOUR, O.dtCraeteStamp) < 16 THEN '12-16'
                                        WHEN DATEPART(HOUR, O.dtCraeteStamp) >= 16 AND DATEPART(HOUR, O.dtCraeteStamp) < 20 THEN '16-20'
                                        ELSE '20-24'
                                    END
                                ORDER BY TimePeriod";
                    var dataForComplete = await _context.ChartLinesVM
                        .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                                new SqlParameter("@Status", 2),
                                                new SqlParameter("@StartDate", startDate),
                                                new SqlParameter("@EndDate", endDate))
                        .ToListAsync();

                    var ArrComplete = timePeriods.Select(tp => new
                    {
                        x = tp,
                        y = dataForComplete.FirstOrDefault(d => d.TimePeriod == tp)?.CountData ?? 0
                    }).ToList();

                return Json(new { success = true, dataForHousKeeping = Arrhouskeeping, dataForMaintince = ArrMaintince, dataForComplete = ArrComplete });

                }
                else if (typePeriod == "1") // This week
                {
                    
                    // حساب النطاق الزمني من startDate إلى endDate
                    string ConvertDateTemp = startDate.ToString();
                    ConvertDateTemp = ConvertDateTemp.Substring(0, 4) + "-" + ConvertDateTemp.Substring(4, 2) + "-" + ConvertDateTemp.Substring(6, 2);

                    DateTime currentStartDate = DateTime.ParseExact(ConvertDateTemp, format, System.Globalization.CultureInfo.InvariantCulture);

                    ConvertDateTemp = endDate.ToString();
                    ConvertDateTemp = ConvertDateTemp.Substring(0, 4) + "-" + ConvertDateTemp.Substring(4, 2) + "-" + ConvertDateTemp.Substring(6, 2);
                    DateTime currentEndDate = DateTime.ParseExact(ConvertDateTemp, format, System.Globalization.CultureInfo.InvariantCulture);


                    // تحويل startDate و endDate إلى صيغة التاريخ
                    //DateTime currentStartDate = DateTime.ParseExact(startDate.ToString("yyyyMMdd"), format, System.Globalization.CultureInfo.InvariantCulture);
                    //DateTime currentEndDate = DateTime.ParseExact(endDate.ToString("yyyyMMdd"), format, System.Globalization.CultureInfo.InvariantCulture);

                    // SQL statement لطلب بيانات النظافة
                     sqlStatment = @"SELECT
                                     DATENAME(WEEKDAY, O.dtCraeteStamp) AS WeekDay,
                                     CONVERT(VARCHAR(10), O.dtCraeteStamp, 120) AS FormattedDate,
                                     COUNT(*) AS CountData 
                                 FROM
                                     OrdersMaster O
                                 WHERE
                                     O.dtCraete Between @StartDate And @EndDate
                                     AND O.CompanyID=@CompanyID
                                     AND O.DepartmentID = @DepartmentID
                                 GROUP BY
                                     DATENAME(WEEKDAY, O.dtCraeteStamp), 
                                     CONVERT(VARCHAR(10), O.dtCraeteStamp, 120)
                                 ORDER BY
                                     FormattedDate";

                    // جلب بيانات النظافة
                    var dataForHousKeeping = await _context.ChartLinesWeekVM
                                 .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                                         new SqlParameter("@DepartmentID", HouseKeepingID),
                                                         new SqlParameter("@StartDate", startDate),
                                                         new SqlParameter("@EndDate", endDate))
                                 .ToListAsync();

                    // إعداد قائمة الأيام بين التاريخين
                    var weekDays = new List<dynamic>();
                    DateTime tempDate = currentStartDate;
                    while (tempDate <= currentEndDate)
                    {
                        weekDays.Add(new
                        {
                            WeekDay = tempDate.DayOfWeek,
                            FormattedDate = tempDate.ToString("yyyy-MM-dd"),
                            CountData = 0 // القيمة الافتراضية
                        });
                        tempDate = tempDate.AddDays(1);
                    }

                    // دمج بيانات النظافة مع قائمة الأيام
                    var dataHouskeepingAllDays = (from day in weekDays
                                                  join data in dataForHousKeeping
                                                  on day.FormattedDate equals data.FormattedDate into gj
                                                  from subdata in gj.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      WeekDay = day.WeekDay,
                                                      FormattedDate = day.FormattedDate,
                                                      CountData = subdata?.CountData ?? 0
                                                  })
                                                  .OrderBy(x => x.FormattedDate)
                                                  .ToList();

                    // جلب بيانات الصيانة
                    var dataForMaintenance = await _context.ChartLinesWeekVM
                                 .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                                         new SqlParameter("@DepartmentID", MaintenanceID),
                                                         new SqlParameter("@StartDate", startDate),
                                                         new SqlParameter("@EndDate", endDate))
                                 .ToListAsync();

                    // دمج بيانات الصيانة مع قائمة الأيام
                    var dataMaintenanceAllDays = (from day in weekDays
                                                  join data in dataForMaintenance
                                                  on day.FormattedDate equals data.FormattedDate into gj
                                                  from subdata in gj.DefaultIfEmpty()
                                                  select new
                                                  {
                                                      WeekDay = day.WeekDay,
                                                      FormattedDate = day.FormattedDate,
                                                      CountData = subdata?.CountData ?? 0
                                                  })
                                                  .OrderBy(x => x.FormattedDate)
                                                  .ToList();

                    // جلب بيانات الطلبات المكتملة
                    sqlStatment = @"SELECT
                                         DATENAME(WEEKDAY, O.dtCraeteStamp) AS WeekDay,
                                         CONVERT(VARCHAR(10), O.dtCraeteStamp, 120) AS FormattedDate,
                                         COUNT(*) AS CountData 
                                     FROM
                                         OrdersMaster O
                                     WHERE
                                         O.dtCraete Between @StartDate And @EndDate
                                         AND O.CompanyID=@CompanyID
                                         AND O.Status = @Status
                                     GROUP BY
                                         DATENAME(WEEKDAY, O.dtCraeteStamp), 
                                         CONVERT(VARCHAR(10), O.dtCraeteStamp, 120)
                                     ORDER BY
                                         FormattedDate";

                    // جلب بيانات الطلبات المكتملة
                    var dataForComplete = await _context.ChartLinesWeekVM
                                 .FromSqlRaw(sqlStatment, new SqlParameter("@CompanyID", companyId),
                                                         new SqlParameter("@Status", 2),
                                                         new SqlParameter("@StartDate", startDate),
                                                         new SqlParameter("@EndDate", endDate))
                                 .ToListAsync();

                    // دمج بيانات الطلبات المكتملة مع قائمة الأيام
                    var dataCompleteAllDays = (from day in weekDays
                                               join data in dataForComplete
                                               on day.FormattedDate equals data.FormattedDate into gj
                                               from subdata in gj.DefaultIfEmpty()
                                               select new
                                               {
                                                   WeekDay = day.WeekDay,
                                                   FormattedDate = day.FormattedDate,
                                                   CountData = subdata?.CountData ?? 0
                                               })
                                               .OrderBy(x => x.FormattedDate)
                                               .ToList();

                    // إرجاع النتائج في صيغة JSON
                    return Json(new { success = true, dataForHousKeeping = dataHouskeepingAllDays, dataForMaintenance = dataMaintenanceAllDays, dataForComplete = dataCompleteAllDays });


                }
                else if (typePeriod == "2") // This Month
                    {

                    // حساب النطاق الزمني من startDate إلى endDate
                    string ConvertDateTemp = startDate.ToString();
                    ConvertDateTemp = ConvertDateTemp.Substring(0, 4) + "-" + ConvertDateTemp.Substring(4, 2) + "-" + ConvertDateTemp.Substring(6, 2);

                    DateTime currentStartDate = DateTime.ParseExact(ConvertDateTemp, format, System.Globalization.CultureInfo.InvariantCulture);

                    ConvertDateTemp = endDate.ToString();
                    ConvertDateTemp = ConvertDateTemp.Substring(0, 4) + "-" + ConvertDateTemp.Substring(4, 2) + "-" + ConvertDateTemp.Substring(6, 2);
                    DateTime currentEndDate = DateTime.ParseExact(ConvertDateTemp, format, System.Globalization.CultureInfo.InvariantCulture);


                    // SQL statement لطلب بيانات النظافة
                     sqlStatment = @"SELECT
                                        YEAR(O.dtCraeteStamp) AS YearDate,
                                        MONTH(O.dtCraeteStamp) AS MonthDate,
                                        ((DAY(O.dtCraeteStamp) - 1) / 7 + 1) AS WeekNumber,
                                        COUNT(*) AS CountData 
                                     FROM
                                         OrdersMaster O
                                     WHERE
                                         O.dtCraete Between @StartDate And @EndDate
                                         AND O.CompanyID=@CompanyID
                                         AND O.DepartmentID = @DepartmentID
                                     GROUP BY
                                        YEAR(O.dtCraeteStamp),
                                        MONTH(O.dtCraeteStamp),
                                        ((DAY(O.dtCraeteStamp) - 1) / 7 + 1)
                                     ORDER BY
                                        YearDate,
                                        MonthDate,
                                        WeekNumber";

                    // جلب بيانات النظافة
                    var dataForHousKeeping = await _context.ChartLinesMonthVM
                                 .FromSqlRaw(sqlStatment,
                                              new SqlParameter("@CompanyID", companyId),
                                              new SqlParameter("@DepartmentID", HouseKeepingID),
                                              new SqlParameter("@StartDate", currentStartDate),
                                              new SqlParameter("@EndDate", currentEndDate))
                                 .ToListAsync();

                    // تهيئة قائمة للأسابيع بين التاريخين
                    //var weekList = new List<dynamic>();
                    var weekList = new List<WeekData>();

                    // التأكد من أن currentStartDate و currentEndDate تم تهيئتهم بشكل صحيح
                    if (currentStartDate <= currentEndDate)
                    {
                        DateTime currentDate = currentStartDate;

                        // إنشاء قائمة للأسابيع بين التاريخين
                        while (currentDate <= currentEndDate)
                        {
                            // حساب رقم الأسبوع الحالي
                            int weekNumber = (int)((currentDate.Day - 1) / 7) + 1;

                            // إضافة بيانات الأسبوع إلى القائمة
                            weekList.Add(new WeekData
                            {
                                YearDate = currentDate.Year,
                                MonthDate = currentDate.Month,
                                WeekNumber = weekNumber,
                                CountData = 0 // القيمة الافتراضية إذا لم يكن هناك بيانات
                            });

                            // التقدم للأسبوع التالي
                            currentDate = currentDate.AddDays(7);
                        }
                    }
                    else
                    {
                        // إذا كانت التواريخ غير مهيأة بشكل صحيح
                        return Json(new { success = false, message = "Invalid date range." });
                    }

                    // تأكد من أن weekList و dataForHousKeeping ليست فارغة
                    if (weekList.Count == 0)
                    {
                        return Json(new { success = false, message = "No weeks to display." });
                    }

                    if (dataForHousKeeping == null || dataForHousKeeping.Count == 0)
                    {
                        // في حال عدم وجود بيانات مسترجعة من الاستعلام
                        return Json(new { success = false, message = "No data available from the database." });
                    }

                    // دمج البيانات المسترجعة مع قائمة الأسابيع لإضافة الأسابيع التي لم تُرجع من الاستعلام
                    var finalData = (from week in weekList
                                     join data in dataForHousKeeping
                                     on new { week.YearDate, week.MonthDate, week.WeekNumber }
                                     equals new { data.YearDate, data.MonthDate, data.WeekNumber }
                                     into gj
                                     from subdata in gj.DefaultIfEmpty() // إذا لم توجد بيانات
                                     select new
                                     {
                                         YearDate = week.YearDate,
                                         MonthDate = week.MonthDate,
                                         WeekNumber = week.WeekNumber,
                                         CountData = subdata?.CountData ?? 0 // إذا لم تكن موجودة، يتم إرجاع 0
                                     })
                                    .OrderBy(x => x.YearDate)
                                    .ThenBy(x => x.MonthDate)
                                    .ThenBy(x => x.WeekNumber)
                                    .ToList();

                    // إرجاع البيانات للـ Frontend (الجبهة الأمامية)
                    return Json(new { success = true, dataForHousKeeping = finalData });

                    //    return Json(new { success = true, dataForHousKeeping = dataForHousKeeping, dataForMaintince = dataForMaintince, dataForComplete = dataForComplete });

                }
                    return Json(new { success = false });
                }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DrawStatusDounat(int startDate, int endDate)
        {
            try
            {
                var companyId = HttpContext.Session.GetString("CompanyID");


                // تأكد من أن companyId ليس null
                if (string.IsNullOrEmpty(companyId))
                {
                    return Json(new { success = false, returnData = "Company ID is not available in session." });
                }

                string MaintenanceID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "Maintenance").DepartmentID;
                string HouseKeepingID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "HouseKeeping").DepartmentID;


                // احضر البيانات من قاعدة البيانات بدون GroupBy و ToString
                var orders = await _context.OrdersMaster
                    .Where(o => o.CompanyID == companyId &&
                                o.dtCraete >= startDate &&
                                o.dtCraete <= endDate)
                    .ToListAsync();


                // قم بالتجميع في الذاكرة بعد جلب البيانات

                //houskeeping
                var pendingCount = orders.Count(o => o.DepartmentID == HouseKeepingID && o.Status == 1);
                var inProcessCount = orders.Count(o => o.DepartmentID == HouseKeepingID && o.Status == 3);
                var completeCount = orders.Count(o => o.DepartmentID == HouseKeepingID && o.Status == 2);
                // Prepare data for the chart
                var datahouskeeping = new
                {
                    series = new[] { pendingCount, inProcessCount, completeCount },
                    labels = new[]
                    {
                            $"Pending ({pendingCount})",
                            $"In-process ({inProcessCount})",
                            $"Complete ({completeCount})"
                        }
                };

                // Guest & employee
                var GuestCount = orders.Count(o => o.FromGuest == 1 && o.Status == 1);
                var EmployeeCount = orders.Count(o => o.FromGuest == 0 && o.Status == 1);
                // Prepare data for the chart
                var dataGuestEmployee = new
                {
                    series = new[] { GuestCount, EmployeeCount },
                    labels = new[]
                    {
                            $"Guest ({GuestCount})",
                            $"Employee ({EmployeeCount})",

                        }
                };

                // Maintenance
                var PendingCountMaintenance = orders.Count(o => o.DepartmentID == MaintenanceID && o.Status == 1);
                var CompleteCountMaintenance = orders.Count(o => o.DepartmentID == MaintenanceID && o.Status == 2);
                // Prepare data for the chart
                var dataMaintenance = new[] { PendingCountMaintenance, CompleteCountMaintenance };


                // Inspection Rooms

                var PendingCountInspection = orders.Count(o => o.isInspection == 1 && o.Status == 1);
                var CompleteCountInspection = orders.Count(o => o.isInspection == 1 && o.Status == 2);
                // Prepare data for the chart
                var dataInspection = new[] { PendingCountInspection, CompleteCountInspection };

                return Json(new { success = true, datahouskeeping = datahouskeeping, dataGuestEmployee = dataGuestEmployee, dataMaintenance = dataMaintenance, dataInspection = dataInspection });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetOrdersByStatus_Test(int stid)
        {
            try
            {
                var companyId = HttpContext.Session.GetString("CompanyID");
                var companyFolder = HttpContext.Session.GetString("CompanyFolder");

                var start = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var sortColumnIndex2 = Request.Form["order[1][column]"];
                var sortColumn2 = Request.Form[$"columns[{sortColumnIndex2}][name]"];
                var sortColumnDirection2 = Request.Form["order[1][dir]"];

                var query = from order in _context.OrdersMaster
                            join status in _context.SysStatus on order.Status equals status.StatusID
                            join user in _context.Users on order.UserIDCreate equals user.Id into users
                            from user in users.DefaultIfEmpty()
                            join unit in _context.CompanyUnits on order.LocationID equals unit.LocationID into units
                            from unit in units.DefaultIfEmpty()
                            join department in _context.Departments on order.DepartmentID equals department.DepartmentID into departments
                            from department in departments.DefaultIfEmpty()
                            where order.CompanyID == companyId //&& order.Status == stid
                            select new
                            {
                                order.OrderID,
                                order.Order_cd,
                                order.dtCraeteStamp,
                                StatusId = order.Status,
                                LocationName = unit.LocationName,
                                StatusName = status.Status_E,
                                DepName = department.DepName_E,
                                CreateName = user.FirstName + " " + user.LastName,
                                DaysDifference = "0",//GetTotalDiffByType(order.dtCraeteStamp, "D"),
                                RemainingHours = "0",//GetTotalDiffByType(order.dtCraeteStamp, "H"),
                                Minutes = 0,// GetTotalDiffByType(order.dtCraeteStamp, "M"),
                                CurrentDate = order.dtCraete.ToString().Substring(6, 2) + "/" + order.dtCraete.ToString().Substring(4, 2) + "/" + order.dtCraete.ToString().Substring(0, 4),
                                status.StatusSortShow,
                                order.isInspection,
                                assignName = ""
                            };

                //var result = await query.ToListAsync();
                //var ordersQuery = _context.Vmorders.Where(x => x.CompanyID == companyId);

                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    ordersQuery = ordersQuery.Where(x =>
                //        x.DepName.Contains(searchValue) ||
                //        x.LocationName.Contains(searchValue) ||
                //        x.CreateName.Contains(searchValue) ||
                //        x.AssignName.Contains(searchValue) ||
                //        x.CurrentDate.Contains(searchValue)
                //    );
                //}

                //if (stid != 0)
                //{
                //    ordersQuery = ordersQuery.Where(x => x.StatusId == stid);
                //}



                //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

                var recordsTotal = await query.CountAsync();
                var data = await query.Skip(start).Take(pageSize).ToListAsync();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }

        }
        public static int GetTotalDiffByType(DateTime startDateTime, string typeValue)
        {
            // Get current date and time in local time
            DateTime currentDateTime = ConvertToLocalTime(DateTime.UtcNow);

            // Calculate total minutes difference
            int totalMinutes = (int)(currentDateTime - startDateTime).TotalMinutes;

            // Calculate days, hours, and minutes
            int days = totalMinutes / (24 * 60);
            totalMinutes %= (24 * 60);
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            // Determine result based on typeValue
            return typeValue switch
            {
                "D" => days,
                "H" => hours,
                "M" => minutes,
                _ => 0,
            };
        }
        private static DateTime ConvertToLocalTime1(DateTime utcDateTime)
        {
            // Assuming your local time zone is "Arabic Standard Time"
            TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, localZone);
        }
        public static DateTime ConvertToLocalTime(DateTime pstTime)
        {
            // Define the source and destination time zones
            TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            TimeZoneInfo arabZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");

            // Convert the PST time to UTC
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(pstTime, pstZone);

            // Convert UTC time to the destination time zone
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, arabZone);

            return localTime;
        }
        public class WeekData
        {
            public int YearDate { get; set; }
            public int MonthDate { get; set; }
            public int WeekNumber { get; set; }
            public int CountData { get; set; }
        }
        ///Temp Code/////
        //[HttpPost]
        //public async Task<IActionResult>  Rebuilddata()
        //{
        //    try
        //    {
        //        var random = new Random();
        //        var records = _context.OrdersMaster
        //            .Where(o=>o.dtCraete==20240522)
        //            .ToList();
        //        if (records!=null)
        //        {

        //            foreach (var record in records)
        //            {
        //                // Generate a random DateTime within a range (e.g., last year to now)
        //                DateTime start = new DateTime(2024, 1, 1);
        //                DateTime end = DateTime.Now;
        //                int range = (end - start).Days;
        //                DateTime randomDate = start.AddDays(random.Next(range))
        //                                           .AddHours(random.Next(0, 24))
        //                                           .AddMinutes(random.Next(0, 60))
        //                                           .AddSeconds(random.Next(0, 60));

        //                // Update the DateTime field
        //                record.dtCraeteStamp = randomDate;

        //                // Convert to yyyyMMdd format and store as an integer
        //                record.dtCraete = int.Parse(randomDate.ToString("yyyyMMdd"));
        //            }
        //        }
        //        _context.SaveChanges();
        //        return Json(new { success = true, msg = "Ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, msg = ex.Message });

        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> HousekeepingChart_old(int startDate, int endDate, string typePeriod)
        //{
        //    try
        //    {
        //        var companyId = HttpContext.Session.GetString("CompanyID");


        //        // تأكد من أن companyId ليس null
        //        if (string.IsNullOrEmpty(companyId))
        //        {
        //            return Json(new { success = false, returnData = "Company ID is not available in session." });
        //        }

        //        string MaintenanceID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "Maintenance").DepartmentID;
        //        string HouseKeepingID = _context.Departments.FirstOrDefault(d => d.CompanyID == companyId && d.MasterNameDefault == "HouseKeeping").DepartmentID;

        //        List<dynamic> dataForHousKeeping = new List<dynamic>();
        //        List<dynamic> dataForMaintince = new List<dynamic>();
        //        List<dynamic> dataForComplete = new List<dynamic>();

        //        // احضر البيانات من قاعدة البيانات بدون GroupBy و ToString
        //        var orders = await _context.OrdersMaster
        //            .Where(o => o.CompanyID == companyId &&
        //                        o.dtCraete >= startDate &&
        //                        o.dtCraete <= endDate)
        //            .ToListAsync();

        //        if (typePeriod == "0") // Today
        //        {

        //            // قم بالتجميع في الذاكرة بعد جلب البيانات
        //            dataForHousKeeping = orders
        //               .Where(o => o.DepartmentID == HouseKeepingID) // إضافة شرط DepartmentID
        //               .GroupBy(o => new
        //               {
        //                   FormattedDate = o.dtCraeteStamp.ToString("yyyyMMdd"),
        //                   TimePeriod = o.dtCraeteStamp.Hour >= 0 && o.dtCraeteStamp.Hour < 4 ? "00-04" :
        //                                o.dtCraeteStamp.Hour >= 4 && o.dtCraeteStamp.Hour < 8 ? "04-08" :
        //                                o.dtCraeteStamp.Hour >= 8 && o.dtCraeteStamp.Hour < 12 ? "08-12" :
        //                                o.dtCraeteStamp.Hour >= 12 && o.dtCraeteStamp.Hour < 16 ? "12-16" :
        //                                o.dtCraeteStamp.Hour >= 16 && o.dtCraeteStamp.Hour < 20 ? "16-20" :
        //                                "20-24"
        //               })
        //               .Select(g => new
        //               {
        //                   TimePeriod = g.Key.TimePeriod,
        //                   CountData = g.Count()
        //               })
        //               .OrderBy(x => x.TimePeriod)
        //               .ToList<dynamic>();



        //            dataForMaintince = orders
        //                           .Where(o => o.DepartmentID == MaintenanceID)
        //                           .GroupBy(o => new
        //                           {
        //                               FormattedDate = o.dtCraeteStamp.ToString("yyyyMMdd"),
        //                               TimePeriod = o.dtCraeteStamp.Hour >= 0 && o.dtCraeteStamp.Hour < 4 ? "00-04" :
        //                                            o.dtCraeteStamp.Hour >= 4 && o.dtCraeteStamp.Hour < 8 ? "04-08" :
        //                                            o.dtCraeteStamp.Hour >= 8 && o.dtCraeteStamp.Hour < 12 ? "08-12" :
        //                                            o.dtCraeteStamp.Hour >= 12 && o.dtCraeteStamp.Hour < 16 ? "12-16" :
        //                                            o.dtCraeteStamp.Hour >= 16 && o.dtCraeteStamp.Hour < 20 ? "16-20" :
        //                                            "20-24"
        //                           })
        //                           .Select(g => new
        //                           {
        //                               TimePeriod = g.Key.TimePeriod,
        //                               CountData = g.Count()
        //                           })
        //                           .OrderBy(x => x.TimePeriod)
        //                           .ToList<dynamic>();

        //            dataForComplete = orders
        //                   .Where(o => o.Status == 2)//Complete
        //                   .GroupBy(o => new
        //                   {
        //                       FormattedDate = o.dtCraeteStamp.ToString("yyyyMMdd"),
        //                       TimePeriod = o.dtCraeteStamp.Hour >= 0 && o.dtCraeteStamp.Hour < 4 ? "00-04" :
        //                                    o.dtCraeteStamp.Hour >= 4 && o.dtCraeteStamp.Hour < 8 ? "04-08" :
        //                                    o.dtCraeteStamp.Hour >= 8 && o.dtCraeteStamp.Hour < 12 ? "08-12" :
        //                                    o.dtCraeteStamp.Hour >= 12 && o.dtCraeteStamp.Hour < 16 ? "12-16" :
        //                                    o.dtCraeteStamp.Hour >= 16 && o.dtCraeteStamp.Hour < 20 ? "16-20" :
        //                                    "20-24"
        //                   })
        //                   .Select(g => new
        //                   {
        //                       TimePeriod = g.Key.TimePeriod,
        //                       CountData = g.Count()
        //                   })
        //                   .OrderBy(x => x.TimePeriod)
        //                   .ToList<dynamic>();

        //            return Json(new { success = true, dataForHousKeeping = dataForHousKeeping, dataForMaintince = dataForMaintince, dataForComplete = dataForComplete });
        //        }
        //        else if (typePeriod == "1") // This week
        //        {

        //            // قم بالتجميع في الذاكرة بعد جلب البيانات

        //            //dataForHousKeeping = orders
        //            //    .Where(o => o.DepartmentID == HouseKeepingID) // تصفية حسب DepartmentID والفترة الزمنية
        //            //    .GroupBy(o => new
        //            //    {
        //            //        WeekDay = o.dtCraeteStamp.DayOfWeek, // تجميع على أساس اليوم من الأسبوع
        //            //        FormattedDate = o.dtCraeteStamp.Date // تجميع على أساس التاريخ بدون وقت
        //            //    })
        //            //    .Select(g => new
        //            //    {
        //            //        WeekDay = g.Key.WeekDay, // اليوم من الأسبوع
        //            //        FormattedDate = g.Key.FormattedDate.ToString("yyyy-MM-dd"), // التاريخ بدون وقت
        //            //        CountData = g.Count() // عدد البيانات
        //            //    })
        //            //    .OrderBy(x => x.FormattedDate) // ترتيب حسب التاريخ
        //            //    .ToList<dynamic>();

        //            // قائمة بأيام الأسبوع (0 = الأحد، 6 = السبت)
        //            var weekDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>()
        //                .Select(d => new { WeekDay = d, FormattedDate = (DateTime?)null, CountData = 0 })
        //                .ToList();

        //            dataForHousKeeping = orders
        //                   .Where(o => o.DepartmentID == HouseKeepingID)
        //                   .GroupBy(o => new
        //                   {
        //                       WeekDay = o.dtCraeteStamp.DayOfWeek, // التجميع على أساس اليوم من الأسبوع
        //                       FormattedDate = o.dtCraeteStamp.Date // التجميع على أساس التاريخ بدون وقت
        //                   })
        //                    .Select(g => new
        //                    {
        //                        WeekDay = g.Key.WeekDay, // اليوم من الأسبوع
        //                        FormattedDate = g.Key.FormattedDate.ToString("yyyy-MM-dd"), // التاريخ بدون وقت بصيغة yyyy-MM-dd
        //                        CountData = g.Count() // عدد البيانات
        //                    })
        //                    .OrderBy(x => x.FormattedDate) // ترتيب حسب التاريخ
        //                    .ToList<dynamic>();

        //            // دمج الطلبات مع أيام الأسبوع
        //            var dataWithAllDays = (from day in weekDays
        //                                   join data in dataForHousKeeping
        //                                   on day.WeekDay equals data.WeekDay into gj
        //                                   from subdata in gj.DefaultIfEmpty()
        //                                   select new
        //                                   {
        //                                       WeekDay = day.WeekDay,
        //                                       FormattedDate = subdata?.FormattedDate ?? day.FormattedDate?.ToString("yyyy-MM-dd"),
        //                                       CountData = subdata?.CountData ?? 0
        //                                   })
        //                                  .OrderBy(x => x.WeekDay)
        //                                  .ToList();



        //            dataForMaintince = orders
        //                 .Where(o => o.DepartmentID == MaintenanceID)
        //                 .GroupBy(o => new
        //                 {
        //                     WeekDay = o.dtCraeteStamp.DayOfWeek, // تجميع على أساس اليوم من الأسبوع
        //                     FormattedDate = o.dtCraeteStamp.Date // تجميع على أساس التاريخ بدون وقت
        //                 })
        //                .Select(g => new
        //                {
        //                    WeekDay = g.Key.WeekDay, // اليوم من الأسبوع
        //                    FormattedDate = g.Key.FormattedDate.ToString("yyyy-MM-dd"), // التاريخ بدون وقت
        //                    CountData = g.Count() // عدد البيانات
        //                })
        //                .OrderBy(x => x.FormattedDate) // ترتيب حسب التاريخ
        //                .ToList<dynamic>();

        //            dataForComplete = orders
        //                   .Where(o => o.Status == 2)//Complete
        //                  .GroupBy(o => new
        //                  {
        //                      WeekDay = o.dtCraeteStamp.DayOfWeek, // تجميع على أساس اليوم من الأسبوع
        //                      FormattedDate = o.dtCraeteStamp.Date // تجميع على أساس التاريخ بدون وقت
        //                  })
        //                .Select(g => new
        //                {
        //                    WeekDay = g.Key.WeekDay, // اليوم من الأسبوع
        //                    FormattedDate = g.Key.FormattedDate.ToString("yyyy-MM-dd"), // التاريخ بدون وقت
        //                    CountData = g.Count() // عدد البيانات
        //                })
        //                .OrderBy(x => x.FormattedDate) // ترتيب حسب التاريخ
        //                .ToList<dynamic>();

        //            return Json(new { success = true, dataForHousKeeping = dataForHousKeeping, dataForMaintince = dataWithAllDays, dataForComplete = dataForComplete });


        //        }
        //        else if (typePeriod == "2") // This Month
        //        {

        //            // قم بالتجميع في الذاكرة بعد جلب البيانات

        //            dataForHousKeeping = orders
        //                .Where(o => o.DepartmentID == HouseKeepingID) // تصفية حسب DepartmentID والفترة الزمنية
        //                .GroupBy(o => new
        //                {
        //                    Year = o.dtCraeteStamp.Year, // سنة التاريخ
        //                    Month = o.dtCraeteStamp.Month, // شهر التاريخ
        //                    WeekNumber = (int)(Math.Floor((o.dtCraeteStamp.Day - 1) / 7.0) + 1) // حساب رقم الأسبوع
        //                })
        //                .Select(g => new
        //                {
        //                    Year = g.Key.Year, // السنة
        //                    Month = g.Key.Month, // الشهر
        //                    WeekNumber = g.Key.WeekNumber, // رقم الأسبوع في الشهر
        //                    CountData = g.Count() // عدد البيانات
        //                })
        //                .OrderBy(x => x.Year)
        //                .ThenBy(x => x.Month)
        //                .ThenBy(x => x.WeekNumber) // ترتيب حسب السنة والشهر ورقم الأسبوع
        //                .ToList<dynamic>();


        //            dataForMaintince = orders
        //                .Where(o => o.DepartmentID == MaintenanceID)
        //                .GroupBy(o => new
        //                {
        //                    Year = o.dtCraeteStamp.Year, // سنة التاريخ
        //                    Month = o.dtCraeteStamp.Month, // شهر التاريخ
        //                    WeekNumber = (int)(Math.Floor((o.dtCraeteStamp.Day - 1) / 7.0) + 1) // حساب رقم الأسبوع
        //                })
        //            .Select(g => new
        //            {
        //                Year = g.Key.Year, // السنة
        //                Month = g.Key.Month, // الشهر
        //                WeekNumber = g.Key.WeekNumber, // رقم الأسبوع في الشهر
        //                CountData = g.Count() // عدد البيانات
        //            })
        //            .OrderBy(x => x.Year)
        //            .ThenBy(x => x.Month)
        //            .ThenBy(x => x.WeekNumber) // ترتيب حسب السنة والشهر ورقم الأسبوع
        //            .ToList<dynamic>();

        //            dataForComplete = orders
        //               .Where(o => o.Status == 2)//Complete
        //               .GroupBy(o => new
        //               {
        //                   Year = o.dtCraeteStamp.Year, // سنة التاريخ
        //                   Month = o.dtCraeteStamp.Month, // شهر التاريخ
        //                   WeekNumber = (int)(Math.Floor((o.dtCraeteStamp.Day - 1) / 7.0) + 1) // حساب رقم الأسبوع
        //               })
        //               .Select(g => new
        //               {
        //                   Year = g.Key.Year, // السنة
        //                   Month = g.Key.Month, // الشهر
        //                   WeekNumber = g.Key.WeekNumber, // رقم الأسبوع في الشهر
        //                   CountData = g.Count() // عدد البيانات
        //               })
        //               .OrderBy(x => x.Year)
        //               .ThenBy(x => x.Month)
        //               .ThenBy(x => x.WeekNumber) // ترتيب حسب السنة والشهر ورقم الأسبوع
        //               .ToList<dynamic>();

        //            return Json(new { success = true, dataForHousKeeping = dataForHousKeeping, dataForMaintince = dataForMaintince, dataForComplete = dataForComplete });

        //        }
        //        return Json(new { success = false });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, returnData = ex.Message });
        //    }
        //}
    }

}

