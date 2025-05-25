namespace LodgeMasterWeb.Controllers
{
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IActionResult Role()
        //{

        //    return View();
        //}
        //public IActionResult GetRole()
        //{
        //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //    var _UserID = HttpContext.Session.GetString("UserID");

        //    try
        //    {

        //        var Skip = int.Parse(Request.Form["start"]);
        //        var pageSize = int.Parse(Request.Form["length"]);

        //        var sortColumnIndex = Request.Form["order[0][column]"];
        //        var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
        //        var sortColumnDirection = Request.Form["order[0][dir]"];

        //        IQueryable<userRole> itemsDep = _context.UserRoles
        //                                          .AsNoTracking()
        //                                          .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0);




        //        //if (bActive == 1)
        //        //{
        //        //    itemsDep = itemsDep.Where(x => x.bActive == bActive);
        //        //}
        //        //itemsDep = itemsDep.OrderBy($"{sortColumn} {sortColumnDirection}");

        //        var recordsTotal = itemsDep.Count();
        //        var data = itemsDep.Skip(Skip).Take(pageSize).ToList();

        //        var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

        //        return Json(datajson);
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = false, returnData = ex.Message });
        //    }
        //}


        //public IActionResult EditRole(string depid)
        //{
        //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //    var _UserID = HttpContext.Session.GetString("UserID");

        //    try
        //    {
        //        //var mySQL = "";

        //        // mySQL = "SELECT * FROM VMItems WHERE CompanyID='" + _CompanyID + "' AND itemId='" + itemid + "'";

        //        //Item itemData = _context.Items.FromSqlRaw(mySQL).FirstOrDefault();
        //        userRole DepData = _context.UserRoles.FirstOrDefault(x => x.CompanyID == _CompanyID && x.RoleID == depid);

        //        if (DepData != null)
        //        {
        //            return Json(new { success = true, returnData = DepData });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, returnData = "" });
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = false, returnData = ex.Message });
        //    }
        //}
        //[HttpPost]
        //public IActionResult CreateRole(string dataObj)
        //{
        //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //    var _UserID = HttpContext.Session.GetString("UserID");

        //    var isFound = false;
        //    userRole dataOk = JsonConvert.DeserializeObject<userRole>(dataObj);

        //    if (string.IsNullOrEmpty(dataOk.RoleName_E) == true)
        //    {
        //        return Json(new { success = false, returnData = "enter Role name" });
        //    }
        //    else
        //    {
        //        isFound = _context.UserRoles.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.RoleName_E == dataOk.RoleName_E);
        //        if (isFound == true)
        //        {
        //            return Json(new { success = false, returnData = "Role Name already exists." });
        //        }
        //    }

        //    //if (string.IsNullOrEmpty(dataOk.DepName_A) == true)
        //    //{
        //    //    return Json(new { success = false, returnData = "enter arabic name" });
        //    //}
        //    //else
        //    //{
        //    //    isFound = _context.Items.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.ItemName_A == dataOk.DepName_A);
        //    //    if (isFound == true)
        //    //    {
        //    //        return Json(new { success = false, returnData = "Arabic Name already exists." });
        //    //    }
        //    //}

        //    //var maxIdService = new GenericService(_context);
        //    //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
        //    //int maxSortedNumber = maxIdService.GetMaxSorted<Department>(e => e.CompanyID == _CompanyID, e => e.iSorted);

        //    //var item = new Item();
        //    dataOk.RoleID = Guid.NewGuid().ToString();
        //    dataOk.CompanyID = _CompanyID;
        //    //dataOk.bActive = dataOk.bActive;
        //    dataOk.iSorted = 1;//maxSortedNumber + 1;
        //    dataOk.isDeleted = 0;

        //    //dataOk.CreateEmpID = "2514cc";

        //    _context.UserRoles.Add(dataOk);
        //    _context.SaveChanges();
        //    var resJson = new { success = true, returnData = "Saved" };

        //    return Json(resJson);
        //}
        //[HttpPost]
        //public IActionResult SaveEditRole(string dataObj)
        //{
        //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //    var _UserID = HttpContext.Session.GetString("UserID");

        //    var isFound = false;
        //    userRole dataOk = JsonConvert.DeserializeObject<userRole>(dataObj);

        //    if (string.IsNullOrEmpty(dataOk.RoleName_E) == true)
        //    {
        //        return Json(new { success = false, returnData = "enter Role name" });
        //    }
        //    else
        //    {
        //        isFound = _context.UserRoles.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.RoleName_E == dataOk.RoleName_E && x.RoleID != dataOk.RoleID);
        //        if (isFound == true)
        //        {
        //            return Json(new { success = false, returnData = "Role Name already exists." });
        //        }
        //    }

        //    //if (string.IsNullOrEmpty(dataOk.DepName_A) == true)
        //    //{
        //    //    return Json(new { success = false, returnData = "enter arabic name" });
        //    //}
        //    //else
        //    //{
        //    //    isFound = _context.Departments.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.DepName_A == dataOk.DepName_A && x.DepartmentID != dataOk.DepartmentID);
        //    //    if (isFound == true)
        //    //    {
        //    //        return Json(new { success = false, returnData = "Arabic Name already exists." });
        //    //    }
        //    //}

        //    var depUpdate = _context.UserRoles.FirstOrDefault(i => i.CompanyID == _CompanyID && i.RoleID == dataOk.RoleID);

        //    if (depUpdate != null)
        //    {
        //        depUpdate.RoleName_E = dataOk.RoleName_E;
        //        depUpdate.RoleDesc_E = dataOk.RoleDesc_E;
        //        depUpdate.bActive = dataOk.bActive;

        //        _context.UserRoles.Update(depUpdate);
        //        _context.SaveChanges();
        //        var resJson = new { success = true, returnData = "Saved" };
        //        return Json(resJson);
        //    }
        //    else
        //    {
        //        var resJson = new { success = false, returnData = "not saved" };
        //        return Json(resJson);
        //    }

        //}
        //[HttpPost]
        //public IActionResult DeleteRole(string depid)
        //{
        //    var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //    var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //    var _UserID = HttpContext.Session.GetString("UserID");
        //    try
        //    {
        //        var itemRemove = _context.UserRoles.FirstOrDefault(i => i.CompanyID == _CompanyID && i.RoleID == depid);

        //        if (itemRemove != null)
        //        {
        //            itemRemove.isDeleted = 1;
        //            _context.UserRoles.Update(itemRemove);
        //            _context.SaveChanges();

        //            var resJson = new { success = true, returnData = "Saved" };
        //            return Json(resJson);
        //        }
        //        else
        //        {
        //            var resJson = new { success = false, returnData = "not delete" };
        //            return Json(resJson);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var resJson = new { success = false, returnData = ex.Message };
        //        return Json(resJson);
        //    }

        //}
    }
}
