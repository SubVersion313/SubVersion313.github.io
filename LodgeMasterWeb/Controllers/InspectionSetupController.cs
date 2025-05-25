//using DotVVM.Framework.Controls;
//using LodgeMasterWeb.Migrations;
using LodgeMasterWeb.Services;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    public class InspectionSetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        public InspectionSetupController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
        #region "Inspection Info"

        /// <summary>
        /// Setup inspection 
        /// </summary>
        /// <returns></returns>
        public IActionResult InspectionSetup()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            return View();
        }

        [HttpPost]
        public IActionResult GetInspection()
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

                IQueryable<InspectionInfo> InspInfo = _context.InspectionInfos
                                                 .AsNoTracking()
                                                 .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0);



                //if (bActive == 1)
                //{
                //    itemsDep = itemsDep.Where(x => x.bActive == bActive);
                //}
                //itemsDep = itemsDep.OrderBy($"{sortColumn} {sortColumnDirection}");

                var recordsTotal = InspInfo.Count();
                var data = InspInfo.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult EditInspinfo(string Inspinfoid)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {

                InspectionInfo InspData = _context.InspectionInfos
                    .AsNoTracking()
                    .FirstOrDefault(x => x.CompanyID == _CompanyID && x.InspInfoId == Inspinfoid);

                if (InspData != null)
                {
                    return Json(new { success = true, returnData = InspData });
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
        public IActionResult CreateInspinfo(string dataObj)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionInfo dataOk = JsonConvert.DeserializeObject<InspectionInfo>(dataObj);

            if (string.IsNullOrEmpty(dataOk.InspName) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection name" });
            }
            else
            {
                isFound = _context.InspectionInfos
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.InspName == dataOk.InspName);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Inspection Name already exists." });
                }
            }


            //var item = new Item();
            dataOk.InspInfoId = Guid.NewGuid().ToString();
            dataOk.CompanyID = _CompanyID;

            dataOk.CreateDate = GeneralFun.GetCurrentTime();//DateTime.Now;
            dataOk.CreateEmpID = _UserID;
            dataOk.IsDeleted = 0;
            dataOk.DeleteEmpID = string.Empty;
            dataOk.DepartmentID = dataOk.DepartmentID;

            _context.InspectionInfos.Add(dataOk);
            _context.SaveChanges();
            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        [HttpPost]
        public IActionResult SaveEditInspinfo(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionInfo dataOk = JsonConvert.DeserializeObject<InspectionInfo>(dataObj);

            if (string.IsNullOrEmpty(dataOk.InspName) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection name" });
            }
            else
            {
                isFound = _context.InspectionInfos
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.InspName == dataOk.InspName && x.InspInfoId != dataOk.InspInfoId);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Inspection Name already exists." });
                }
            }


            var depUpdate = _context.InspectionInfos
                            .FirstOrDefault(i => i.CompanyID == _CompanyID && i.InspInfoId == dataOk.InspInfoId);

            if (depUpdate != null)
            {
                depUpdate.InspName = dataOk.InspName;
                depUpdate.InspDesc = dataOk.InspDesc;
                depUpdate.bActive = dataOk.bActive;
                depUpdate.InspToCreateOrder = dataOk.InspToCreateOrder;
                depUpdate.DepartmentID = dataOk.DepartmentID;

                _context.InspectionInfos.Update(depUpdate);
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
        public IActionResult DeleteInspinfo(string Inspinfoid)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var itemRemove = _context.InspectionInfos
                    .AsNoTracking()
                    .FirstOrDefault(i => i.CompanyID == _CompanyID && i.InspInfoId == Inspinfoid);

                if (itemRemove != null)
                {
                    itemRemove.IsDeleted = 1;
                    _context.InspectionInfos.Update(itemRemove);
                    _context.SaveChanges();
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

        #endregion

        #region "Inspection Department"
        public IActionResult InspectionDepartment()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            return View();
        }
        [HttpGet]
        public IActionResult GetInspectionInfo()
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            try
            {
                var itemsDep = _context.InspectionInfos
                     .AsNoTracking()
                     .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                     .OrderBy(x => x.InspName)
                     .Select(x => new SelectListItem { Text = x.InspName, Value = x.InspInfoId })
                     .ToList();

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetInspectionDepById(string InspInfoId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            try
            {
                if (InspInfoId != null)
                {
                    var itemsDep = _context.InspectionDeps
                         .AsNoTracking()
                         .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.InspInfoId == InspInfoId)
                         .OrderBy(x => x.InspDepName)
                         .Select(x => new SelectListItem { Text = x.InspDepName, Value = x.InspDepId })
                         .ToList();

                    var datajson = new { success = true, returnData = itemsDep };
                    return Json(datajson);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult GetInspDepartment(string Department)
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
                var mySQL = "";

                //if (string.IsNullOrEmpty(department)==false &&  department != "0" )
                //{
                IQueryable<InspectionDep> itemsGrid;

                //mySQL = "SELECT InspDepId, InspDepName, InspInfoId, bActive, q_QuestionCount.QuestionNo";
                //mySQL += " FROM InspectionDeps LEFT OUTER JOIN";
                //mySQL += " q_QuestionCount ON InspectionDeps.InspDepId = q_QuestionCount.PartID";
                //mySQL += " WHERE InspectionDeps.CompanyID='" + _CompanyID + "' AND InspectionDeps.InspInfoId='" + Department + "'";
                mySQL = "SELECT * FROM InspectionDeps " + " WHERE CompanyID='" + _CompanyID + "' AND InspInfoId='" + Department + "'";

                itemsGrid = _context.InspectionDeps.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}");

                var recordsTotal = itemsGrid.Count();
                var data = itemsGrid.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(datajson);
                //}
                //else
                //{
                //    var datajson = new { recordsFiltered = "", recordsTotal = "", data = "" };
                //    return Json(datajson);
                //}

            }

            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult EditInspDepartment(string InspDepId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {

                InspectionDep InspData = _context.InspectionDeps
                                    .AsNoTracking()
                                    .FirstOrDefault(x => x.CompanyID == _CompanyID && x.InspDepId == InspDepId);

                if (InspData != null)
                {
                    return Json(new { success = true, returnData = InspData });
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
        public IActionResult CreateInspDepartment(string dataObj)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionDep dataOk = JsonConvert.DeserializeObject<InspectionDep>(dataObj);

            if (string.IsNullOrEmpty(dataOk.InspDepName) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection Department name" });
            }
            else
            {
                isFound = _context.InspectionDeps
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.InspDepName == dataOk.InspDepName);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Inspection Department Name already exists." });
                }
            }

            var maxIdService = new GenericService(_context);
            //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
            int maxSortedNumber = maxIdService.GetMaxSorted<InspectionDep>(e => e.CompanyID == _CompanyID && e.InspInfoId == dataOk.InspInfoId, e => e.iSorted);

            //var item = new Item();
            dataOk.InspDepId = Guid.NewGuid().ToString();
            //dataOk.InspInfoId = Guid.NewGuid().ToString();
            dataOk.CompanyID = _CompanyID;

            dataOk.CreateDate = GeneralFun.GetCurrentTime();//DateTime.Now;
            dataOk.CreateEmpID = _UserID;
            dataOk.IsDeleted = 0;
            dataOk.DeleteEmpID = string.Empty;
            dataOk.iSorted = maxSortedNumber + 1;

            _context.InspectionDeps.Add(dataOk);
            _context.SaveChanges();
            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        [HttpPost]
        public IActionResult SaveEditInspDepartment(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionDep dataOk = JsonConvert.DeserializeObject<InspectionDep>(dataObj);

            if (string.IsNullOrEmpty(dataOk.InspDepName) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection Department name" });
            }
            else
            {
                isFound = _context.InspectionDeps
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.InspDepName == dataOk.InspDepName && x.InspDepId != dataOk.InspDepId);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Inspection Department Name already exists." });
                }
            }


            var depUpdate = _context.InspectionDeps
                            .FirstOrDefault(i => i.CompanyID == _CompanyID && i.InspDepId == dataOk.InspDepId);

            if (depUpdate != null)
            {
                depUpdate.InspDepName = dataOk.InspDepName;

                depUpdate.bActive = dataOk.bActive;

                _context.InspectionDeps.Update(depUpdate);
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
        public IActionResult DeleteInspDepartment(string InspDepId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var itemRemove = _context.InspectionDeps
                    .AsNoTracking()
                    .FirstOrDefault(i => i.CompanyID == _CompanyID && i.InspDepId == InspDepId);

                if (itemRemove != null)
                {
                    itemRemove.IsDeleted = 1;
                    _context.InspectionDeps.Update(itemRemove);
                    _context.SaveChanges();
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
        #endregion


        #region "Inspection Question"
        public IActionResult InspectionQuestion()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            return View();
        }
        [HttpGet]
        public IActionResult GetInspectionQuestion()
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            try
            {

                var itemsDep = _context.InspectionQuestions
                     .AsNoTracking()
                     .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                     .OrderBy(x => x.QuestionDisplay)
                     .Select(x => new SelectListItem { Text = x.QuestionDisplay, Value = x.QuestionID })
                     .ToList();

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult GetInspQuestion()
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
                var mySQL = "";

                //if (string.IsNullOrEmpty(department)==false &&  department != "0" )
                //{
                IQueryable<InspectionQuestion> itemsGrid;

                //mySQL = "SELECT InspDepId, InspDepName, InspInfoId, bActive, q_QuestionCount.QuestionNo";
                //mySQL += " FROM InspectionDeps LEFT OUTER JOIN";
                //mySQL += " q_QuestionCount ON InspectionDeps.InspDepId = q_QuestionCount.PartID";
                //mySQL += " WHERE InspectionDeps.CompanyID='" + _CompanyID + "' AND InspectionDeps.InspInfoId='" + Department + "'";
                mySQL = "SELECT * FROM InspectionQuestions " + " WHERE CompanyID='" + _CompanyID + "' AND isdeleted=0";

                itemsGrid = _context.InspectionQuestions.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}");

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
        [HttpPost]
        public IActionResult EditInspQuestion(string InspQuestionId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {

                InspectionQuestion InspData = _context.InspectionQuestions
                                    .AsNoTracking()
                                    .FirstOrDefault(x => x.CompanyID == _CompanyID && x.QuestionID == InspQuestionId);

                if (InspData != null)
                {
                    return Json(new { success = true, returnData = InspData });
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
        public IActionResult CreateInspQuestion(string dataObj)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionQuestion dataOk = JsonConvert.DeserializeObject<InspectionQuestion>(dataObj);

            if (string.IsNullOrEmpty(dataOk.QuestionDisplay) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection Question name" });
            }
            else
            {
                isFound = _context.InspectionDeps
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.InspDepName == dataOk.QuestionDisplay);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Question already exists." });
                }
            }


            //var item = new Item();
            dataOk.QuestionID = Guid.NewGuid().ToString();
            //dataOk.InspInfoId = Guid.NewGuid().ToString();
            dataOk.CompanyID = _CompanyID;

            dataOk.CreateDate = GeneralFun.GetCurrentTime();
            dataOk.CreateEmpID = _UserID;
            dataOk.IsDeleted = 0;

            _context.InspectionQuestions.Add(dataOk);
            _context.SaveChanges();
            var resJson = new { success = true, returnData = "Saved" };

            return Json(resJson);
        }
        [HttpPost]
        public IActionResult SaveEditInspQuestion(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            InspectionQuestion dataOk = JsonConvert.DeserializeObject<InspectionQuestion>(dataObj);

            if (string.IsNullOrEmpty(dataOk.QuestionDisplay) == true)
            {
                return Json(new { success = false, returnData = "enter Inspection Question" });
            }
            else
            {
                isFound = _context.InspectionQuestions
                    .AsNoTracking()
                    .Any(x => x.CompanyID == _CompanyID && x.QuestionDisplay == dataOk.QuestionDisplay && x.QuestionID != dataOk.QuestionID);

                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Inspection Department Name already exists." });
                }
            }


            var depUpdate = _context.InspectionQuestions
                            .FirstOrDefault(i => i.CompanyID == _CompanyID && i.QuestionID == dataOk.QuestionID);

            if (depUpdate != null)
            {
                depUpdate.QuestionDisplay = dataOk.QuestionDisplay;

                depUpdate.bActive = dataOk.bActive;

                _context.InspectionQuestions.Update(depUpdate);
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
        public IActionResult DeleteInspQuestion(string InspQuestionId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var itemRemove = _context.InspectionQuestions
                    .AsNoTracking()
                    .FirstOrDefault(i => i.CompanyID == _CompanyID && i.QuestionID == InspQuestionId);

                if (itemRemove != null)
                {
                    itemRemove.IsDeleted = 1;
                    _context.InspectionQuestions.Update(itemRemove);
                    _context.SaveChanges();
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
        #endregion

        #region "Inspection Link Question"
        public IActionResult InspectionLinkQuestion()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
        [HttpPost]
        public IActionResult GetInspLkQues(string inspinfoid, string insptype)
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
                var mySQL = "";

                //if (string.IsNullOrEmpty(department)==false &&  department != "0" )
                //{
                IQueryable<VMLinkQuestion> itemsGrid;

                //mySQL = "SELECT InspDepId, InspDepName, InspInfoId, bActive, q_QuestionCount.QuestionNo";
                //mySQL += " FROM InspectionDeps LEFT OUTER JOIN";
                //mySQL += " q_QuestionCount ON InspectionDeps.InspDepId = q_QuestionCount.PartID";
                //mySQL += " WHERE InspectionDeps.CompanyID='" + _CompanyID + "' AND InspectionDeps.InspInfoId='" + Department + "'";
                mySQL = "SELECT * FROM VMLinkQuestions " + " WHERE CompanyID='" + _CompanyID + "' AND [PartID] ='" + insptype + "' AND [InspectionId] = '" + inspinfoid + "' AND isDeleted=0";





                itemsGrid = _context.VMLinkQuestions.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}");

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

        [HttpPost]
        public IActionResult InsertInspLinkPartQuestion(string dataObj)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var isFound = false;
                InspectionLinkPartQuestion dataOk = JsonConvert.DeserializeObject<InspectionLinkPartQuestion>(dataObj);

                if (string.IsNullOrEmpty(dataOk.InspectionId) == true || string.IsNullOrEmpty(dataOk.PartID) == true || string.IsNullOrEmpty(dataOk.QuestionID) == true)
                {
                    return Json(new { success = false, returnData = "Select all Inspection " });
                }
                else
                {
                    if (dataOk.InspectionId == "0" || dataOk.PartID == "0" || dataOk.QuestionID == "0")
                    {
                        return Json(new { success = false, returnData = "Select all Inspection " });
                    }
                    isFound = _context.InspectionLinkPartQuestions//InspectionsLinkPartQuestions
                        .AsNoTracking()
                        .Any(x => x.CompanyID == _CompanyID && x.InspectionId == dataOk.InspectionId && x.PartID == dataOk.PartID && x.QuestionID == dataOk.QuestionID);

                    if (isFound == true)
                    {
                        return Json(new { success = false, returnData = "Question already exists." });
                    }
                }

                var maxIdService = new GenericService(_context);
                //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
                int maxSortedNumber = maxIdService.GetMaxSorted<InspectionLinkPartQuestion>(e => e.CompanyID == _CompanyID && e.InspectionId == dataOk.InspectionId && e.PartID == dataOk.PartID, e => e.iSorted);


                //var item = new Item();
                dataOk.LinkPQID = Guid.NewGuid().ToString();
                dataOk.CompanyID = _CompanyID;
                dataOk.iSorted = maxSortedNumber + 1;

                _context.InspectionLinkPartQuestions.Add(dataOk);
                _context.SaveChanges();
                var resJson = new { success = true, returnData = "Saved" };

                return Json(resJson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteInspLinkPartQuestion(string linkPQID)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            try
            {
                var itemRemove = _context.InspectionLinkPartQuestions
                    .AsNoTracking()
                    .FirstOrDefault(i => i.CompanyID == _CompanyID && i.LinkPQID == linkPQID);

                if (itemRemove != null)
                {
                    itemRemove.isDeleted = 1;
                    _context.InspectionLinkPartQuestions.Update(itemRemove);
                    _context.SaveChanges();
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
        public IActionResult PublishInspLinkPartQuestion()
        {
            //var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");
            //try
            //{
            //    var itemRemove = _context.InspectionLinkPartQuestions
            //        .AsNoTracking()
            //        .FirstOrDefault(i => i.CompanyID == _CompanyID && i.LinkPQID == linkPQID);

            //    if (itemRemove != null)
            //    {
            //        itemRemove.isDeleted = 1;
            //        _context.InspectionLinkPartQuestions.Update(itemRemove);
            //        _context.SaveChanges();
            //        var resJson = new { success = true, returnData = "Saved" };
            //        return Json(resJson);
            //    }
            //    else
            //    {
            //        var resJson = new { success = false, returnData = "not delete" };
            //        return Json(resJson);
            //    }
            //}
            //catch (Exception ex)
            //{
            //  var resJson = new { success = false, returnData = ex.Message };
            var resJson = new { success = false, returnData = "not delete" };
            return Json(resJson);
            //}

        }
        #endregion
    }
}
