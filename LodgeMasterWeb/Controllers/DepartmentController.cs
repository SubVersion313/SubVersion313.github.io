using LodgeMasterWeb.Services;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private string _CompanyID = "2525cc";// string.Empty;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Department()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }

        [HttpPost]
        public IActionResult GetDepartment()
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");


            try
            {

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<Department> itemsDep = _context.Departments
                                                .AsNoTracking()
                                                .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0);
                //var itemsDep = _context.Departments
                //    .AsNoTracking()
                //    .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                //    .Select(x => new
                //    {
                //        x.DepartmentID,
                //        x.DepName_E,
                //        x.DepName_A,
                //        x.bActive
                //    });

                //"DepartmentID","DepName_E","DepName_A","bActive"

                //if (bActive == 1)
                //{
                //    itemsDep = itemsDep.Where(x => x.bActive == bActive);
                //}
                //itemsDep = itemsDep.OrderBy($"{sortColumn} {sortColumnDirection}");

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
        public IActionResult CreateDepartment(string dataObj)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            Department dataOk = JsonConvert.DeserializeObject<Department>(dataObj);

            if (string.IsNullOrEmpty(dataOk.DepName_E) == true)
            {
                return Json(new { success = false, returnData = "enter english name" });
            }
            else
            {
                isFound = _context.Items.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.ItemName_E == dataOk.DepName_E);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "English Name already exists." });
                }
            }

            if (string.IsNullOrEmpty(dataOk.DepName_A) == true)
            {
               // return Json(new { success = false, returnData = "enter arabic name" });
            }
            else
            {
                isFound = _context.Items.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.ItemName_A == dataOk.DepName_A);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Arabic Name already exists." });
                }
            }

            var maxIdService = new GenericService(_context);
            //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
            int maxSortedNumber = maxIdService.GetMaxSorted<Department>(e => e.CompanyID == _CompanyID, e => e.iSorted);

            //var item = new Item();
            dataOk.DepartmentID = Guid.NewGuid().ToString();
            dataOk.CompanyID = _CompanyID;
            dataOk.bActive = dataOk.bActive;
            dataOk.iSorted = maxSortedNumber + 1;
            dataOk.IsDeleted = 0;
            dataOk.isDefault = 0;
            dataOk.CreateEmpID = _UserID;

            _context.Departments.Add(dataOk);
            _context.SaveChanges();
            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        
        [HttpPost]
        public IActionResult EditDep(string depid)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                //var mySQL = "";

                // mySQL = "SELECT * FROM VMItems WHERE CompanyID='" + _CompanyID + "' AND itemId='" + itemid + "'";

                //Item itemData = _context.Items.FromSqlRaw(mySQL).FirstOrDefault();
                Department DepData = _context.Departments.FirstOrDefault(x => x.CompanyID == _CompanyID && x.DepartmentID == depid);

                if (DepData != null)
                {
                    return Json(new { success = true, returnData = DepData });
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
        public IActionResult SaveEditDep(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            Department dataOk = JsonConvert.DeserializeObject<Department>(dataObj);

            if (string.IsNullOrEmpty(dataOk.DepName_E) == true)
            {
                return Json(new { success = false, returnData = "enter english name" });
            }
            else
            {
                isFound = _context.Departments.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.DepName_E == dataOk.DepName_E && x.DepartmentID != dataOk.DepartmentID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "English Name already exists." });
                }
            }

            if (string.IsNullOrEmpty(dataOk.DepName_A) == true)
            {
               // return Json(new { success = false, returnData = "enter arabic name" });
            }
            else
            {
                isFound = _context.Departments.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.DepName_A == dataOk.DepName_A && x.DepartmentID != dataOk.DepartmentID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Arabic Name already exists." });
                }
            }

            var depUpdate = _context.Departments.FirstOrDefault(i => i.CompanyID == _CompanyID && i.DepartmentID == dataOk.DepartmentID);

            if (depUpdate != null)
            {
                depUpdate.DepName_E = dataOk.DepName_E;
                depUpdate.DepName_A = dataOk.DepName_A;
                depUpdate.bActive = dataOk.bActive;

                _context.Departments.Update(depUpdate);
                _context.SaveChanges();
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
        public IActionResult DeleteDep(string depid)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var itemRemove = _context.Departments.FirstOrDefault(i => i.CompanyID == _CompanyID && i.DepartmentID == depid);

                if (itemRemove != null)
                {
                    itemRemove.IsDeleted = 1;
                    _context.Departments.Update(itemRemove);
                    _context.SaveChanges();
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
    }
}
