using LodgeMasterWeb.Services;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using ClosedXML.Excel;
//using System.IO;
//using DocumentFormat.OpenXml.Spreadsheet;
//using OfficeOpenXml;
//using OfficeOpenXml.Style;
//using System.ComponentModel;
//using NPOI.SS.UserModel;
//using NPOI.XSSF.UserModel;
//using GemBox.Spreadsheet;
//using IronXL;

namespace LodgeMasterWeb.Controllers
{

    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult items()
        {

            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
        [HttpPost]
        public IActionResult Getitems(string department, int stock, string staff, string qtyFrom, string qtyTo)
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
                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];
                var mySQL = "";
                var depSQL = "";
                var stockSQL = "";
                var QtySQL = "";

                IQueryable<ItemsGridVM> itemsGrid;
                //if (department == "0")
                //{
                //    itemsGrid = _context.VMItems
                //               .Where(x => x.CompanyID == _CompanyID);
                //}
                //else
                //{
                //    itemsGrid = _context.VMItems
                //           .Where(x => x.CompanyID == _CompanyID && x.DepartmentID == department);
                //    _context.VMItems.FromSqlRaw(cc);
                //}
                mySQL = "SELECT * FROM VMItems WHERE isInspection=0 AND IsDELETED=0 AND CompanyID='" + _CompanyID + "' ";
                if (department != "0")
                {
                    depSQL = " AND DepartmentID='" + department + "'";
                }
                //Stock
                if (stock == 0)
                {
                    //stockSQL = " AND ItemStock=";
                }
                else if (stock == 1)
                {
                    stockSQL = " AND ItemStock=1";
                }
                else if (stock == 2)
                {
                    //stockSQL = " AND ItemStock=";
                }
                else if (stock == 3)
                {
                    stockSQL = " AND ItemStock=0";
                }
                //
                if (!string.IsNullOrEmpty(qtyFrom) && !string.IsNullOrEmpty(qtyTo))
                {
                    var qf = int.Parse(qtyFrom);
                    var qt = int.Parse(qtyTo);

                    if (qf < qt)
                    {
                        QtySQL = " AND (Qty >= " + qf + " AND Qty <= " + qt + ")";
                    }
                }
                mySQL += depSQL + stockSQL + QtySQL;

                itemsGrid = _context.VMItems.FromSqlRaw(mySQL);//.OrderBy($"{sortColumn} {sortColumnDirection}");

                var recordsTotal = itemsGrid.Count();
                var data = itemsGrid.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetitemsByService(int service)
        {

            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");

                var itemsService = await _context.Items
                        .Where(x => x.CompanyID == _CompanyID && x.isService == 0 && x.bActive == 1 && x.isInspection == 0)
                        .OrderBy(o => o.ItemName_E)
                        .Select(s => new SelectListItem { Text = s.ItemName_E, Value = s.ItemID })
                        .ToListAsync();


                var datajson = new { success = true, returnData = itemsService };

                return Json(datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }
        }
        //[Authorize(Permissions.Items.Edit)]
        [HttpPost]
        public async Task<IActionResult> EditItem(string itemid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var mySQL = "";

                mySQL = "SELECT * FROM VMItems WHERE CompanyID='" + _CompanyID + "' AND itemId='" + itemid + "'";

                Item itemData = await _context.Items.FromSqlRaw(mySQL).FirstOrDefaultAsync();

                if (itemData != null)
                {
                    var subItemsData = await _context.ItemServices
                        .Join(
                        _context.Items,
                        subItem => subItem.ItemIDSub,
                        item => item.ItemID,
                        (subItem, item) => new
                        {
                            ItemID = subItem.ItemID,
                            ItemIDSub = subItem.ItemIDSub,
                            subitemQty = subItem.Qty,
                            subitemName = item.ItemName_E,
                            CompanyID = subItem.CompanyID,
                            IsDeleted = subItem.IsDeleted,

                        }
                        )
                        .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0 && w.ItemID == itemid)
                        .ToListAsync();

                    string SubItemHtml = "";

                    if (subItemsData != null && subItemsData.Count > 0)
                    {


                        foreach (var subItem in subItemsData)
                        {
                            SubItemHtml += $"<span class='badge-item mx-2 mb-3' data-itemid='{subItem.ItemIDSub}' data-qty='{subItem.subitemQty}' data-itemname='{subItem.subitemName}'>";
                            SubItemHtml += subItem.subitemName;
                            SubItemHtml += $"<span class='badge-number'>{subItem.subitemQty}</span>";
                            SubItemHtml += "<span class='icon-exit'><i class='fa-solid fa-xmark fa-fw'></i></span></span>";
                        }
                    }
                    return Json(new { success = true, returnData = itemData, subItems = SubItemHtml });
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
        [HttpGet]
        public async Task<IActionResult> GetDepartment()
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

                var itemsDep = await _context.Departments
                     .AsNoTracking()
                     .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                     .OrderBy(x => x.DepName_E)
                     .Select(x => new SelectListItem { Text = x.DepName_E, Value = x.DepartmentID })
                     .ToListAsync();

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        public async Task<IActionResult> GetCountItems()
        {

            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var itemsCount = await _context.Items
                     .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.isInspection == 0)
                     .AsNoTracking()
                     .CountAsync();

                var datajson = new { success = true, returnData = itemsCount };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(string dataObj, string ServiceItems)
        {
            try
            {

                var isFound = false;
                Item dataOk = JsonConvert.DeserializeObject<Item>(dataObj);
                var itemServiceList = JsonConvert.DeserializeObject<List<ItemsServiceVM>>(ServiceItems);

                //List<ItemsServiceVM>dataservice = JsonConvert.DeserializeObject<List<ItemsServiceVM>>(dataObj);

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                if (string.IsNullOrEmpty(dataOk.ItemName_E) == true)
                {
                    return Json(new { success = false, returnData = "enter english name" });
                }
                else
                {
                    isFound = await _context.Items.AsNoTracking().AnyAsync(x => x.CompanyID == _CompanyID && x.ItemName_E == dataOk.ItemName_E);
                    if (isFound == true)
                    {
                        return Json(new { success = false, returnData = "English Name already exists." });
                    }
                }

                if (string.IsNullOrEmpty(dataOk.ItemName_A) == true)
                {
                    return Json(new { success = false, returnData = "enter arabic name" });
                }
                else
                {
                    isFound = await _context.Items.AsNoTracking().AnyAsync(x => x.CompanyID == _CompanyID && x.ItemName_A == dataOk.ItemName_A);
                    if (isFound == true)
                    {
                        return Json(new { success = false, returnData = "Arabic Name already exists." });
                    }
                }

                var maxIdService = new GenericService(_context);
                int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
                int maxSortedNumber = maxIdService.GetMaxSorted<Item>(e => e.CompanyID == _CompanyID, e => e.iSorted);

                //var item = new Item();
                dataOk.ItemID = Guid.NewGuid().ToString();
                dataOk.Item_cd = maxIdNumber + 1;
                dataOk.CompanyID = _CompanyID;
                dataOk.ItemType = 0;
                dataOk.bActive = 1;
                dataOk.iSorted = maxSortedNumber + 1;
                dataOk.IsDeleted = 0;
                dataOk.isDefault = 0;
                dataOk.isInspection = 0;
                //dataOk.ItemName_E = dataOk.ItemName_E;
                //dataOk.ItemName_A = dataOk.ItemName_A;
                //dataOk.DepartmentID = dataOk.DepartmentID;
                //dataOk.isService = dataOk.isService;
                //dataOk.ItemStock = dataOk.ItemStock;
                //dataOk.Qty = dataOk.Qty;
                //dataOk.minQty = dataOk.minQty;

                //dataOk.CreateEmpID
                //public string ItemIDDefault { get; set; } = string.Empty;

                //TODO:
                dataOk.UserCreate = _UserID;

                //AddSubItems if serviceitem true///

                await _context.Items.AddAsync(dataOk);
                await _context.SaveChangesAsync();

                if (dataOk.isService == 1)
                {
                    if (string.IsNullOrEmpty(ServiceItems) == false)
                    {
                        // Process service items if present
                        if (itemServiceList != null && itemServiceList.Count > 0)
                        {
                            foreach (var serviceItem in itemServiceList)
                            {
                                var serviceitemdata = new ItemService
                                {
                                    ItemID = dataOk.ItemID,
                                    CompanyID = _CompanyID,
                                    ItemIDSub = serviceItem.ItemIDSub,
                                    Qty = serviceItem.subitemQty,
                                    iSorted = serviceItem.iSorted
                                };
                                serviceItem.ItemID = dataOk.ItemID;
                                serviceItem.CompanyID = _CompanyID;

                                await _context.ItemServices.AddAsync(serviceitemdata);

                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }

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
        public async Task<IActionResult> SaveEditItem(string dataObj, string ServiceItems)
        {

            var isFound = false;
            Item dataOk = JsonConvert.DeserializeObject<Item>(dataObj);

            var itemServiceList = JsonConvert.DeserializeObject<List<ItemsServiceVM>>(ServiceItems);

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            if (string.IsNullOrEmpty(dataOk.ItemName_E) == true)
            {
                return Json(new { success = false, returnData = "enter english name" });
            }
            else
            {
                isFound = await _context.Items.AsNoTracking().AnyAsync(x => x.CompanyID == _CompanyID && x.ItemName_E == dataOk.ItemName_E && x.ItemID != dataOk.ItemID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "English Name already exists." });
                }
            }

            if (string.IsNullOrEmpty(dataOk.ItemName_A) == true)
            {
                return Json(new { success = false, returnData = "enter arabic name" });
            }
            else
            {
                isFound = _context.Items.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.ItemName_A == dataOk.ItemName_A && x.ItemID != dataOk.ItemID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Arabic Name already exists." });
                }
            }
            if (dataOk.isService == 1)
            {
                if (itemServiceList == null && itemServiceList.Count <= 0)
                {
                    return Json(new { success = false, returnData = "Please select sub items." });
                }
            }

            var itemUpdate = _context.Items.FirstOrDefault(i => i.CompanyID == _CompanyID && i.ItemID == dataOk.ItemID);

            if (itemUpdate != null)
            {
                itemUpdate.ItemName_E = dataOk.ItemName_E;
                itemUpdate.ItemName_A = dataOk.ItemName_A;
                itemUpdate.DepartmentID = dataOk.DepartmentID;
                //itemUpdate.isService = dataOk.isService;
                itemUpdate.ItemStock = dataOk.ItemStock;
                itemUpdate.Qty = dataOk.Qty;
                itemUpdate.minQty = dataOk.minQty;

                _context.Items.Update(itemUpdate);
                _context.SaveChanges();


                if (dataOk.isService == 1)
                {
                    if (string.IsNullOrEmpty(ServiceItems) == false)
                    {
                        // Process service items if present
                        if (itemServiceList != null && itemServiceList.Count > 0)
                        {

                            await DeleteSubItem(dataOk.ItemID);

                            foreach (var serviceItem in itemServiceList)
                            {
                                var serviceitemdata = new ItemService
                                {
                                    ItemID = dataOk.ItemID,
                                    CompanyID = _CompanyID,
                                    ItemIDSub = serviceItem.ItemIDSub,
                                    Qty = serviceItem.subitemQty,
                                    iSorted = serviceItem.iSorted
                                };
                                serviceItem.ItemID = dataOk.ItemID;
                                serviceItem.CompanyID = _CompanyID;

                                _context.ItemServices.Add(serviceitemdata);

                            }
                            _context.SaveChanges();
                        }
                        else if (itemServiceList != null && itemServiceList.Count == 0)
                        {
                            await DeleteSubItem(dataOk.ItemID);
                        }

                    }
                }

                var resJson = new { success = true, returnData = "Saved" };
                return Json(resJson);
            }
            else
            {
                var resJson = new { success = false, returnData = "not saved" };
                return Json(resJson);
            }

        }
        public async Task DeleteSubItem(string itemid)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            if (!string.IsNullOrEmpty(itemid))
            {
                var itemDelete = _context.ItemServices
                        .Where(i => i.CompanyID == _CompanyID && i.ItemID == itemid)
                        .ToList();

                if (itemDelete != null && itemDelete.Any())
                {
                    _context.ItemServices.RemoveRange(itemDelete);
                    await _context.SaveChangesAsync();
                }
            }

        }
        //[Authorize(Permissions.Items.Delete)]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(string itemid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var itemRemove = _context.Items.FirstOrDefault(i => i.CompanyID == _CompanyID && i.ItemID == itemid);

                if (itemRemove != null)
                {

                    if (itemRemove.isService == 1)
                    {
                        var SubitemRemove = await _context.ItemServices.Where(i => i.CompanyID == _CompanyID && i.ItemID == itemid).ToListAsync();
                        if (SubitemRemove != null && SubitemRemove.Count > 0)
                        {

                            foreach (var subitem in SubitemRemove)
                            {
                                subitem.IsDeleted = 1;
                            }
                            _context.ItemServices.UpdateRange(SubitemRemove);
                            //_context.SaveChanges();
                        }

                    }
                    itemRemove.IsDeleted = 1;

                    _context.Items.Update(itemRemove);
                    await _context.SaveChangesAsync();

                    var resJson = new { success = true, returnData = "Saved" };
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
        [HttpPost]
        public async Task<IActionResult> SearchOrders(string objData)
        {
            try
            {

                //FilterDashboardVM fddata = JsonConvert.DeserializeObject<FilterDashboardVM>(objData);

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                //var searchValue = Request.Form["search[value]"];

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<ItemsGridVM> Orders;

                if (string.IsNullOrEmpty(objData))
                {
                    return Json(new { success = false, returnData = "" });
                }
                var searchValue = objData;

                Orders = _context.VMItems
                    .Where(x => x.isInspection == 0
                            && x.CompanyID == _CompanyID
                            && x.ItemName_E.Contains(searchValue)
                            || x.ItemName_A.Contains(searchValue)
                            || x.DepartmentName.Contains(searchValue)
                            || x.ServiceName.Contains(searchValue)
                            || x.StockName.Contains(searchValue)
                            || x.Qty.ToString().Contains(searchValue));

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

                Orders = Orders.OrderBy($"{sortColumn} {sortColumnDirection}");
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
        public class ItemViewModel
        {
            public string ItemID { get; set; }
            public int Item_cd { get; set; }
            public string ItemName_E { get; set; }
            public string ItemName_A { get; set; }
            public string DepartmentID { get; set; }
            public string DepartmentName { get; set; }
            public int isService { get; set; }
            public int ItemStock { get; set; }
            public int Qty { get; set; }
            public int minQty { get; set; }
        }

        #region XSSFWorkbook
        [HttpGet]
        public async Task<IActionResult> ExportToExcel_old2(bool includeData = true)
        {
            return View();
            //try
            //{

            //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //    var _UserID = HttpContext.Session.GetString("UserID");

            //    // جلب البيانات بناءً على شرط includeData
            //    var items = includeData
            //        ? await (from item in _context.Items
            //                 join dept in _context.Departments on item.DepartmentID equals dept.DepartmentID
            //                 where item.CompanyID == _CompanyID
            //                    && item.IsDeleted == 0
            //                    && item.isInspection == 0
            //                 orderby item.ItemName_E
            //                 select new ItemViewModel
            //                 {
            //                     ItemID = item.ItemID,
            //                     Item_cd = item.Item_cd,
            //                     ItemName_E = item.ItemName_E,
            //                     ItemName_A = item.ItemName_A,
            //                     DepartmentID = item.DepartmentID,
            //                     DepartmentName = dept.DepName_E,
            //                     isService = item.isService,
            //                     ItemStock = item.ItemStock,
            //                     Qty = item.Qty,
            //                     minQty = item.minQty
            //                 }).ToListAsync()
            //        : new List<ItemViewModel>();

            //    IWorkbook workbook = new XSSFWorkbook();
            //    ISheet sheet = workbook.CreateSheet("Items");

            //    // Add column headers
            //    var headerRow = sheet.CreateRow(0);
            //    headerRow.CreateCell(0).SetCellValue("ItemID");
            //    headerRow.CreateCell(1).SetCellValue("Item No.");
            //    headerRow.CreateCell(2).SetCellValue("itemid"); // سيكون العمود مخفياً لاحقاً
            //    headerRow.CreateCell(3).SetCellValue("Item No.");
            //    headerRow.CreateCell(4).SetCellValue("Item English name");
            //    headerRow.CreateCell(5).SetCellValue("Item Arabic name");
            //    headerRow.CreateCell(6).SetCellValue("Department");
            //    headerRow.CreateCell(7).SetCellValue("is Service");
            //    headerRow.CreateCell(8).SetCellValue("in Stock");
            //    headerRow.CreateCell(9).SetCellValue("Quantity");
            //    headerRow.CreateCell(10).SetCellValue("min Quantity");
            //    // Add other headers...

            //    if (includeData)
            //    {

            //        int row = 1;
            //        foreach (var item in items)
            //        {
            //            var dataRow = sheet.CreateRow(row);
            //            dataRow.CreateCell(0).SetCellValue(item.ItemID); // ItemID
            //            dataRow.CreateCell(1).SetCellValue(item.Item_cd); // Item No.
            //            dataRow.CreateCell(2).SetCellValue(item.ItemID); // itemid
            //            dataRow.CreateCell(3).SetCellValue(item.Item_cd); // Item No.
            //            dataRow.CreateCell(4).SetCellValue(item.ItemName_E); // Item English name
            //            dataRow.CreateCell(5).SetCellValue(item.ItemName_A); // Item Arabic name
            //            dataRow.CreateCell(6).SetCellValue(item.DepartmentName); // Department
            //            dataRow.CreateCell(7).SetCellValue(item.isService == 1 ? "True" : "False"); // is Service
            //            dataRow.CreateCell(8).SetCellValue(item.ItemStock == 1 ? "True" : "False"); // in Stock
            //            dataRow.CreateCell(9).SetCellValue(item.Qty); // Quantity
            //            dataRow.CreateCell(10).SetCellValue(item.minQty); // min Quantity                                           // Add other fields...
            //            row++;
            //        }
            //    }

            //    for (int r = 1; r <= sheet.LastRowNum; r++)
            //    {
            //        var cell = sheet.GetRow(r).GetCell(5);
            //        cell.SetCellType(CellType.String);

            //        cell = sheet.GetRow(r).GetCell(6);
            //        cell.SetCellType(CellType.String);
            //    }

            //    // ضبط عرض الأعمدة لتناسب المحتوى
            //    for (int i = 0; i < headerRow.LastCellNum; i++)
            //    {
            //        sheet.AutoSizeColumn(i);
            //    }

            //    // حفظ الملف إلى MemoryStream
            //    using (var stream = new MemoryStream())
            //    {
            //        //workbook.SaveAs(stream);
            //        stream.Position = 0;

            //        string excelName = $"Items-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            //        // إرجاع الملف للتحميل باستخدام MemoryStream
            //        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            //    }


            //    //using (var stream = new MemoryStream())
            //    //{
            //    //    workbook.Write(stream); // حفظ البيانات إلى `MemoryStream`
            //    //    string excelName = $"Items-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            //    //    byte[] byteArray = stream.ToArray(); // تحويل `MemoryStream` إلى `byte[]`
            //    //    stream.Close(); // إغلاق `MemoryStream`

            //    //    // إرسال الملف إلى المتصفح لعرض مربع "حفظ باسم"
            //    //    return File(byteArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }
        #endregion

        #region EPPlus
        [HttpGet]
        public async Task<IActionResult> ExportToExcel11(bool includeData = true)
        {
            return View();
            //try
            //{
            //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //    var _UserID = HttpContext.Session.GetString("UserID");

            //   // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //    // جلب البيانات بناءً على شرط includeData
            //    var items = includeData
            //        ? await (from item in _context.Items
            //                 join dept in _context.Departments on item.DepartmentID equals dept.DepartmentID
            //                 where item.CompanyID == _CompanyID
            //                    && item.IsDeleted == 0
            //                    && item.isInspection == 0
            //                 orderby item.ItemName_E
            //                 select new ItemViewModel
            //                 {
            //                     ItemID = item.ItemID,
            //                     Item_cd = item.Item_cd,
            //                     ItemName_E = item.ItemName_E,
            //                     ItemName_A = item.ItemName_A,
            //                     DepartmentID = item.DepartmentID,
            //                     DepartmentName = dept.DepName_E,
            //                     isService = item.isService,
            //                     ItemStock = item.ItemStock,
            //                     Qty = item.Qty,
            //                     minQty = item.minQty
            //                 }).ToListAsync()
            //        : new List<ItemViewModel>();

            //    // إنشاء ملف Excel
            //    var fileName = $"Items-{System.DateTime.Now:yyyyMMddHHmmss}.xlsx";
            //    using (var package = new ExcelPackage())
            //    {
            //        var worksheet = package.Workbook.Worksheets.Add("Items");

            //        // رؤوس الأعمدة
            //        worksheet.Cells[1, 1].Value = "itemid"; // سيكون العمود مخفياً لاحقاً
            //        worksheet.Cells[1, 2].Value = "Item No.";
            //        worksheet.Cells[1, 3].Value = "Item English name";
            //        worksheet.Cells[1, 4].Value = "Item Arabic name";
            //        worksheet.Cells[1, 5].Value = "Department";
            //        worksheet.Cells[1, 6].Value = "is Service";
            //        worksheet.Cells[1, 7].Value = "in Stock";
            //        worksheet.Cells[1, 8].Value = "Quantity";
            //        worksheet.Cells[1, 9].Value = "min Quantity";

            //        // إضافة البيانات إذا كان includeData = true
            //        if (includeData)
            //        {
            //            int row = 2;
            //            foreach (var item in items)
            //            {
            //                worksheet.Cells[row, 1].Value = item.ItemID; // itemid
            //                worksheet.Cells[row, 2].Value = item.Item_cd; // Item No.
            //                worksheet.Cells[row, 3].Value = item.ItemName_E; // Item English name
            //                worksheet.Cells[row, 4].Value = item.ItemName_A; // Item Arabic name
            //                worksheet.Cells[row, 5].Value = item.DepartmentName; // Department
            //                worksheet.Cells[row, 6].Value = item.isService == 1 ? "True" : "False"; // is Service
            //                worksheet.Cells[row, 7].Value = item.ItemStock == 1 ? "True" : "False"; // in Stock
            //                worksheet.Cells[row, 8].Value = item.Qty; // Quantity
            //                worksheet.Cells[row, 9].Value = item.minQty; // min Quantity

            //                row++;
            //            }
            //        }

            //        // إخفاء عمود "itemid"
            //        worksheet.Column(1).Hidden = true;

            //        // تنسيق خلايا العمود "is Service" و "in Stock" كـ True/False
            //        worksheet.Column(6).Style.Numberformat.Format = "General";
            //        worksheet.Column(7).Style.Numberformat.Format = "General";

            //        // تنسيق الأعمدة لجعلها ملائمة للمحتوى
            //        worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            //        // إعداد الملف للتصدير
            //        //var stream = new MemoryStream();
            //        //package.SaveAs(stream);
            //        // stream.Position = 0;

            //        //string excelName = $"Items-{System.DateTime.Now:yyyyMMddHHmmss}.xlsx";
            //        //return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);


            //        // حفظ ملف Excel في الذاكرة
            //        var excelBytes = package.GetAsByteArray();
            //        //var fileName = $"Items-{System.DateTime.Now:yyyyMMddHHmmss}.xlsx";

            //        // إرسال الملف كملف قابل للتنزيل للعميل
            //        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            //    }

            //}
            //catch (Exception ex)
            //{

            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }

        // معالجة ملف الرفع
        [HttpPost]
        public async Task<IActionResult> UploadExcel_old(IFormFile file)
        {
            return View();
            //try
            //{
            //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //    var _UserID = HttpContext.Session.GetString("UserID");

            //    if (file == null || file.Length == 0)
            //    {
            //        ModelState.AddModelError("", "Please upload a valid Excel file.");
            //        return View();
            //    }
            //    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;// LicenseContext.Commercial;
            //    using (var stream = new MemoryStream())
            //    {
            //        await file.CopyToAsync(stream);
            //        using (var package = new ExcelPackage(stream))
            //        {
            //            var worksheet = package.Workbook.Worksheets[0];
            //            var rowCount = worksheet.Dimension.Rows;

            //            var newItems = new List<Item>();
            //            var updateItems = new List<Item>();

            //            var maxIdService = new GenericService(_context);
            //            int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
            //            //int maxSortedNumber = maxIdService.GetMaxSorted<Item>(e => e.CompanyID == _CompanyID, e => e.iSorted);

            //            for (int row = 2; row <= rowCount; row++) // يبدأ من الصف 2 لتجاهل الرؤوس
            //            {
            //                var itemId = worksheet.Cells[row, 1].Value?.ToString();
            //                var itemNo = worksheet.Cells[row, 2].Value?.ToString();
            //                var itemEnglishName = worksheet.Cells[row, 3].Value?.ToString();
            //                var itemArabicName = worksheet.Cells[row, 4].Value?.ToString();
            //                var department = worksheet.Cells[row, 5].Value?.ToString();
            //                var isService = worksheet.Cells[row, 6].Value?.ToString() == "True";
            //                var inStock = worksheet.Cells[row, 7].Value?.ToString() == "True";
            //                var quantity = worksheet.Cells[row, 8].Value?.ToString();
            //                var minQuantity = worksheet.Cells[row, 9].Value?.ToString();

            //                // تحقق من وجود بيانات كاملة
            //                if (string.IsNullOrEmpty(itemNo) || string.IsNullOrEmpty(itemEnglishName) ||
            //                    string.IsNullOrEmpty(itemArabicName) || string.IsNullOrEmpty(department) ||
            //                    string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(minQuantity))
            //                {
            //                    ModelState.AddModelError("", $"Row {row} has incomplete data. Please fill all fields.");
            //                    return View();
            //                }

            //                // إذا كان الصنف موجود يتم التحديث، وإذا لم يكن موجود يتم الإضافة
            //                var existingItem = await _context.Items.FirstOrDefaultAsync(x => x.ItemID.ToString() == itemId);

            //                if (existingItem != null)
            //                {
            //                    // تحديث الصنف الحالي
            //                    //existingItem.ItemNo = itemNo;
            //                    existingItem.ItemName_E = itemEnglishName;
            //                    existingItem.ItemName_A = itemArabicName;
            //                    existingItem.DepartmentID = department;
            //                    existingItem.isService = isService == true ? 1 : 0;
            //                    existingItem.ItemStock = inStock == true ? 1 : 0;
            //                    existingItem.Qty = int.Parse(quantity);
            //                    existingItem.minQty = int.Parse(minQuantity);
            //                    updateItems.Add(existingItem);
            //                }
            //                else
            //                {
            //                    maxIdNumber++;
            //                    // إنشاء صنف جديد
            //                    var newItem = new Item
            //                    {
            //                        ItemID = Guid.NewGuid().ToString(),
            //                        Item_cd = maxIdNumber,
            //                        ItemName_E = itemEnglishName,
            //                        ItemName_A = itemArabicName,
            //                        DepartmentID = department,
            //                        isService = isService == true ? 1 : 0,
            //                        ItemStock = inStock == true ? 1 : 0,
            //                        Qty = int.Parse(quantity),
            //                        minQty = int.Parse(minQuantity)
            //                    };
            //                    newItems.Add(newItem);
            //                }
            //            }

            //            // تحديث أو إدخال الأصناف الجديدة
            //            if (updateItems.Any())
            //            {
            //                _context.Items.UpdateRange(updateItems);
            //            }

            //            if (newItems.Any())
            //            {
            //                await _context.Items.AddRangeAsync(newItems);
            //            }

            //            await _context.SaveChangesAsync();

            //            TempData["Success"] = "Items have been uploaded and processed successfully.";
            //            return RedirectToAction("UploadExcel");
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

        }
        #endregion

        #region XLWork
        [HttpGet]
        public async Task<IActionResult> ExportToExcel(bool includeData = true)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                // جلب البيانات بناءً على شرط includeData
                var items = includeData
                    ? await (from item in _context.Items
                             join dept in _context.Departments on item.DepartmentID equals dept.DepartmentID
                             where item.CompanyID == _CompanyID
                                && item.IsDeleted == 0
                                && item.isInspection == 0
                             orderby item.ItemName_E
                             select new ItemViewModel
                             {
                                 ItemID = item.ItemID,
                                 Item_cd = item.Item_cd,
                                 ItemName_E = item.ItemName_E,
                                 ItemName_A = item.ItemName_A,
                                 DepartmentID = item.DepartmentID,
                                 DepartmentName = dept.DepName_E,
                                 isService = item.isService,
                                 ItemStock = item.ItemStock,
                                 Qty = item.Qty,
                                 minQty = item.minQty
                             }).ToListAsync() // الحصول على أول 100 صنف فقط
                    : new List<ItemViewModel>();

                // Export DataTable to Excel
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Items");

                    // Add header row

                    worksheet.Cell(1, 1).Value = "Item Id.";
                    worksheet.Cell(1, 2).Value = "Item No.";
                    worksheet.Cell(1, 3).Value = "Item English name";
                    worksheet.Cell(1, 4).Value = "Item Arabic name";
                    worksheet.Cell(1, 5).Value = "Department";
                    worksheet.Cell(1, 6).Value = "is Service";
                    worksheet.Cell(1, 7).Value = "in Stock";
                    worksheet.Cell(1, 8).Value = "Quantity";
                    worksheet.Cell(1, 9).Value = "Minimum Quantity";

                    int countitem = 2;
                    // Add data rows
                    foreach (var itm in items)
                    {
                        //worksheet.Cell(i + 2, j + 1).Value = dataTable.Rows[i][j];

                        worksheet.Cell(countitem, 1).Value = itm.ItemID;
                        worksheet.Cell(countitem, 2).Value = itm.Item_cd.ToString();
                        worksheet.Cell(countitem, 3).Value = itm.ItemName_E;
                        worksheet.Cell(countitem, 4).Value = itm.ItemName_A;
                        worksheet.Cell(countitem, 5).Value = itm.DepartmentName;
                        worksheet.Cell(countitem, 6).Value = itm.isService == 0 ? "Not Service" : "Service";
                        worksheet.Cell(countitem, 7).Value = itm.ItemStock == 0 ? "Out Stock" : "In Stock";
                        worksheet.Cell(countitem, 8).Value = itm.Qty;
                        worksheet.Cell(countitem, 9).Value = itm.minQty;
                        countitem++;
                    }

                    // تنسيق الجدول لجعله أكثر جمالية
                    worksheet.Columns().AdjustToContents();
                    worksheet.Columns("1").Hide();

                    string excelName = $"Items-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    // Create a memory stream to save the workbook
                    ////using (var stream = new MemoryStream())
                    ////{
                    ////    // Save the workbook to the memory stream
                    ////    workbook.SaveAs(stream);

                    ////    // Convert the memory stream to a byte array
                    ////    var fileBytes = stream.ToArray();
                        

                    ////    // Set the Content-Disposition header to prompt the browser to save the file
                    ////    var contentDisposition = new System.Net.Mime.ContentDisposition
                    ////    {
                    ////        FileName = excelName,//"generated_excel.xlsx",
                    ////        Inline = false // Prompt the browser to save as attachment
                    ////    };
                    ////    Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

                    ////    // Return the file content as a file result
                    ////    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    ////}


                    // حفظ الملف في الذاكرة
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                        // إضافة ترويسة Content-Disposition للتأكد من عرض مربع التنزيل
                        HttpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={excelName}");
                        // إعادة الملف كتنزيل للمستخدم
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                }

            }

            catch (Exception ex)
            {
                return BadRequest(new { FilePath = ex.Message });
            }
        }

        // معالجة ملف الرفع
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (file == null || file.Length == 0)
                {
                    ModelState.AddModelError("", "Please upload a valid Excel file.");
                    return View();
                }

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheet(1); // تحديد الورقة الأولى
                        var rowCount = worksheet.LastRowUsed().RowNumber();

                        var newItems = new List<Item>();
                        var updateItems = new List<Item>();

                        var maxIdService = new GenericService(_context);
                        int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);

                        string SameDepartment = "";
                        string  DepID = "";

                        for (int row = 2; row <= rowCount; row++) // يبدأ من الصف 2 لتجاهل الرؤوس
                        {
                            var itemId = worksheet.Cell(row, 1).GetValue<string>();
                            var itemNo = worksheet.Cell(row, 2).GetValue<string>();
                            var itemEnglishName = worksheet.Cell(row, 3).GetValue<string>();
                            var itemArabicName = worksheet.Cell(row, 4).GetValue<string>();
                            var department = worksheet.Cell(row, 5).GetValue<string>();
                            var isService = worksheet.Cell(row, 6).GetValue<bool>();
                            var inStock = worksheet.Cell(row, 7).GetValue<bool>();
                            var quantity = worksheet.Cell(row, 8).GetValue<int>();
                            var minQuantity = worksheet.Cell(row, 9).GetValue<int>();

                            // تحقق من وجود بيانات كاملة
                            if (string.IsNullOrEmpty(itemEnglishName) 
                                || string.IsNullOrEmpty(department))
                                //string.IsNullOrEmpty(itemNo) 
                                //|| string.IsNullOrEmpty(itemArabicName) 
                            {
                                ModelState.AddModelError("", $"Row {row} has incomplete data. Please fill all fields.");
                                return View();
                            }

                            // إذا كان الصنف موجود يتم التحديث، وإذا لم يكن موجود يتم الإضافة
                            var existingItem = await _context.Items.FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.ItemID == itemId);

                            if(department!= SameDepartment)
                            {
                                 SameDepartment= department;
                                DepID = GeneralFun.GetDepartmentIdByName(department, _CompanyID, _context);
                            }


                            if (existingItem != null)
                            {
                                if (!string.IsNullOrEmpty(DepID))
                                {
                                    existingItem.ItemName_E = itemEnglishName;
                                    existingItem.ItemName_A = itemArabicName;
                                    existingItem.DepartmentID = DepID;
                                    existingItem.isService = isService ? 1 : 0;
                                    existingItem.ItemStock = inStock ? 1 : 0;
                                    existingItem.Qty = quantity;
                                    existingItem.minQty = minQuantity;
                                    updateItems.Add(existingItem);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(DepID))
                                {
                                    maxIdNumber++;

                                    var newItem = new Item
                                    {
                                        ItemID = Guid.NewGuid().ToString(),
                                        Item_cd = maxIdNumber,
                                        ItemName_E = itemEnglishName,
                                        ItemName_A = itemArabicName,
                                        DepartmentID = DepID,
                                        isService = isService ? 1 : 0,
                                        ItemStock = inStock ? 1 : 0,
                                        Qty = quantity,
                                        minQty = minQuantity,
                                        CompanyID = _CompanyID
                                    };
                                    newItems.Add(newItem); 
                                }
                            }
                        }

                        if (updateItems.Any())
                        {
                            _context.Items.UpdateRange(updateItems);
                        }

                        if (newItems.Any())
                        {
                            await _context.Items.AddRangeAsync(newItems);
                        }

                        await _context.SaveChangesAsync();

                        TempData["Success"] = "Items have been uploaded and processed successfully.";
                        return View();// RedirectToAction("UploadExcel");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing the file: " + ex.Message);
                return View();
            }

        }
        #endregion
    }
}

