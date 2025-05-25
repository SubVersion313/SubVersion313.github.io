
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;
        private readonly string _empAVATAR = @"Images/avatar/bg-man.jpg";
        public ReportController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _empImagePath = _webHostEnvironment.WebRootPath;

        }
        #region "Inspection Report"
        [Authorize(Permissions.Report.ReportInspection)]
        public IActionResult InspectionReport()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
        [HttpPost]
        public IActionResult GetInspectionReport(string dataObj)
        {
            try
            {

                ReportFilterVM dataOk = JsonConvert.DeserializeObject<ReportFilterVM>(dataObj);

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<VMInspMaster> tableInspMasters = Enumerable.Empty<VMInspMaster>().AsQueryable();

                //IQueryable<VMInspMaster> tableInspMasters;

                if (dataOk.isComeFrom == 0)
                {
                    tableInspMasters = _context.VMInspMasters
                               .Where(x => x.CompanyID == _CompanyID);
                }
                else if (dataOk.isComeFrom == 1) //Filter by Inspection Type
                {

                    //if (dataOk.filterPeriod == 0 && string.IsNullOrEmpty(dataOk.filterDateFrom)==false  && string.IsNullOrEmpty(dataOk.filterDateTo)== false)
                    if (string.IsNullOrEmpty(dataOk.filterDateFrom) == false && string.IsNullOrEmpty(dataOk.filterDateTo) == false)
                    {

                        tableInspMasters = _context.VMInspMasters
                                       .Where(x => x.CompanyID == _CompanyID
                                                && x.InspDate >= DateTime.Parse(dataOk.filterDateFrom)
                                                && x.InspDate <= DateTime.Parse(dataOk.filterDateTo));
                    }
                    else
                    {

                        //<Value = "1" > This Today 
                        //<value = "2" > This Week
                        //<value = "3" > Before Week
                        //<value = "4" > Before Month
                        //<value = "5" > Before Year
                        DateTime currntDateTo = GeneralFun.GetCurrentTime();
                        DateTime currntDateFrom = GeneralFun.GetCurrentTime();

                        if (dataOk.filterPeriod == 1)
                            currntDateFrom = currntDateTo;
                        if (dataOk.filterPeriod == 2)
                            currntDateFrom = currntDateTo.AddDays(-7);
                        if (dataOk.filterPeriod == 3)
                        {
                            currntDateTo = currntDateTo.AddDays(-7);
                            currntDateFrom = currntDateTo.AddDays(-7);
                        }
                        if (dataOk.filterPeriod == 4)
                            currntDateFrom = currntDateTo.AddDays(-30);
                        if (dataOk.filterPeriod == 5)
                            currntDateFrom = currntDateTo.AddDays(-365);

                        tableInspMasters = _context.VMInspMasters
                               .Where(x => x.CompanyID == _CompanyID
                                        && x.InspDate >= currntDateFrom
                                        && x.InspDate <= currntDateTo);
                    }
                }

                tableInspMasters = tableInspMasters.OrderBy($"{sortColumn} {sortColumnDirection}");
                var recordsTotal = tableInspMasters.Count();
                var data = tableInspMasters.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = true, returnData = ex.Message });
            }
        }

        public IActionResult InspDetails(string Id)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                CultureInfo cultureInfo = new CultureInfo("en-US");

                VMInspDetails VMInspDetail = new VMInspDetails();

                var InspMst = _context.InspectionsMaster
                                .AsNoTracking()
                                .FirstOrDefault(x => x.InspectionID == Id && x.CompanyID == _CompanyID);

                if (InspMst != null)
                {

                    // var pathImage = Path.Combine(_empImagePath, "Companies", _CompanyFolder, "images", "employee");

                    VMInspDetail.InspectionID = Id;
                    VMInspDetail.CompanyID = _CompanyID;
                    VMInspDetail.InspName = GetInspectionNameById(InspMst.InspInfoId);
                    VMInspDetail.CreateDate = InspMst.CreateDate.ToString("dd MMMM, yyyy", cultureInfo);// "dd/MM/yyyy");

                    VMInspDetail.LocationName = GetLocationNameById(InspMst.LocationID);
                    VMInspDetail.StatusName = InspMst.statusName;


                    var InspQuestionDet = _context.InspectionsDet
                            .Where(x => x.InspectionID == Id)
                            .OrderBy(x => x.iSorted)
                            .ToList();

                    if (InspQuestionDet != null)
                    {

                        //IEnumerable<VMInspQuestionDetails> lstQD =new VMInspQuestionDetails();

                        List<VMInspQuestionDetails> lstQD = new List<VMInspQuestionDetails>();

                        var MisPicBefor = "";
                        var TitlePicBefor = "";
                        var MisPicAfter = "";
                        var TitlePicAfter = "";
                        foreach (var inspqd in InspQuestionDet)
                        {
                            if (string.IsNullOrEmpty(inspqd.PicBefore))
                            {
                                //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", fileName);
                                //MisPic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",  "images", "Avatar", "no-photo.png");
                                MisPicBefor = Path.Combine(@"../..", "images", "Avatar", "no-photo.png");
                                TitlePicBefor = "No-photo";
                            }
                            else
                            {
                                TitlePicBefor = inspqd.PicBefore;
                                MisPicBefor = Path.Combine(@"../../companies", _CompanyFolder, "images", "inspection", inspqd.PicBefore);
                                //MisPic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", inspqd.PicBefore);
                                //MisPic = Path.Combine(@"../../companies", _CompanyFolder, "images", "inspection", inspqd.PicBefore);
                            }
                            if (string.IsNullOrEmpty(inspqd.PicAfter))
                            {
                                //Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", fileName);
                                //MisPic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",  "images", "Avatar", "no-photo.png");
                                MisPicAfter = Path.Combine(@"../..", "images", "Avatar", "no-photo.png");
                                TitlePicAfter = "No-photo";
                            }
                            else
                            {
                                TitlePicAfter = inspqd.PicAfter;
                                MisPicAfter = Path.Combine(@"../../companies", _CompanyFolder, "images", "inspection", inspqd.PicAfter);
                                //MisPic = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", inspqd.PicBefore);
                                //MisPic = Path.Combine(@"../../companies", _CompanyFolder, "images", "inspection", inspqd.PicBefore);
                            }
                            VMInspQuestionDetails qutdet = new VMInspQuestionDetails
                            {
                                PartName = inspqd.PartName,
                                CommetAnswer = inspqd.CommetAnswer,
                                QuestionDisplay = inspqd.QuestionDisplay,
                                UserAnswer = inspqd.UserAnswer,
                                PicBefore = TitlePicBefor,//inspqd.PicBefore,
                                PicBeforePath = MisPicBefor,
                                PicAfter = TitlePicAfter,//inspqd.PicBefore,
                                PicAfterPath = MisPicAfter,
                                CommetAnswerAfter = inspqd.CommetAnswerAfter,
                                UserAnswerAfter = inspqd.UserAnswerAfter,
                                DetailID = inspqd.DetailID

                            };
                            lstQD.Add(qutdet);
                        }

                        VMInspDetail.LstQuestionData = lstQD;
                    }

                }

                return View(VMInspDetail);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        private string GetInspectionNameById(string InspId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var Result = "";

            try
            {
                var itemsDep = _context.InspectionInfos
                     .AsNoTracking()
                     .FirstOrDefault(x => x.CompanyID == _CompanyID && x.InspInfoId == InspId);

                if (itemsDep != null)
                {
                    Result = itemsDep.InspName;
                }

                return (Result);
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        private string GetLocationNameById(string LocationId)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var Result = "";

            try
            {
                var itemsDep = _context.CompanyUnits
                     .AsNoTracking()
                     .FirstOrDefault(x => x.CompanyID == _CompanyID && x.LocationID == LocationId);

                if (itemsDep != null)
                {
                    Result = itemsDep.LocationName;
                }

                return (Result);
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        #endregion
        #region Edit answers quz
        //public IActionResult UpdateRow(string inspectioid, int answer, string notes, long rowId, string picture)
        [HttpPost]
        //public async Task<IActionResult> UpdateFormRow(string dataObj)
        public async Task<IActionResult> UpdateFormRow()
        {
            try
            {
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var form = HttpContext.Request.Form;
                List<int> LstRowNum = new List<int>();

                foreach (var key in form.Keys)
                {
                    // Check if the key represents a row identifier
                    if (key.StartsWith("dataObj["))
                    {
                        var rowId = key.Substring(key.IndexOf("[") + 1, key.IndexOf("]") - key.IndexOf("[") - 1);

                        bool bFoundId = false;

                        for (int i = 0; i < LstRowNum.Count; i++)
                        {
                            if (LstRowNum[i] == int.Parse(rowId))
                            {
                                bFoundId = true;
                                break;
                            }
                        }
                        if (bFoundId == false)
                        {
                            LstRowNum.Add(int.Parse(rowId));
                        }

                    }
                }
                bool bFoundData = false;
                foreach (var rowNo in LstRowNum)
                {
                    // Extract data for the current row
                    string inspectioId = form[$"dataObj[{rowNo}].inspectioId"];
                    long rowid = long.Parse(form[$"dataObj[{rowNo}].rowid"]);
                    int answer = int.Parse(form[$"dataObj[{rowNo}].answer"]);
                    string note = form[$"dataObj[{rowNo}].note"];

                    IFormFile image = null;

                    // Check if an image file is selected for the current row
                    for (int i = 0; i < form.Files.Count; i++)
                    {
                        var FileData = form.Files[i];
                        if (FileData != null && FileData.Name == $"dataObj[{rowNo}].image")
                        {
                            image = form.Files[i];
                            break;
                        }
                    }

                    var UpdateRowData = await _context.InspectionsDet
                        .FirstOrDefaultAsync(x => x.InspectionID == inspectioId && x.DetailID == rowid);

                    if (UpdateRowData != null)
                    {
                        string imageName = "";
                        bFoundData = true;
                        if (image != null && image.Length > 0)
                        {
                            var fileExtension = Path.GetExtension(image.FileName);
                            imageName = Guid.NewGuid().ToString() + fileExtension;
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", imageName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await image.CopyToAsync(stream);
                            }

                            if (!string.IsNullOrEmpty(UpdateRowData.PicAfter))
                            {
                                var filePathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", UpdateRowData.PicAfter);
                                if (System.IO.File.Exists(filePathDelete) == true)
                                {
                                    System.IO.File.Delete(filePathDelete);
                                    //imageName = "";
                                }
                            }
                            UpdateRowData.PicAfter = imageName;
                        }

                        UpdateRowData.UserAnswerAfter = answer;
                        UpdateRowData.CommetAnswerAfter = note;


                        _context.InspectionsDet.Update(UpdateRowData);
                    }

                }
                if (bFoundData == true)
                {
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        ////public IActionResult UpdateRow(string inspectioid, int answer, string notes, long rowId, string picture)
        //[HttpPost]
        ////public async Task<IActionResult> UpdateFormRow(string dataObj)
        //    public async Task<IActionResult> UpdateFormRow([FromForm] List<IFormFile> dataArrayImage, [FromForm] string dataObj)
        //{
        //    try
        //    {
        //        //var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //        //var _UserID = HttpContext.Session.GetString("UserID");

        //       List<ImageUploadModel> dataOk = JsonConvert.DeserializeObject<List<ImageUploadModel>>(dataObj);

        //        //string filePath = "";
        //        string ImageName = "";
        //       // int count = 0;

        //        foreach (var formdatarow in dataOk)
        //        {
        //            var UpdateRowData = await _context.InspectionsDet
        //                    .FirstOrDefaultAsync(x => x.InspectionID == formdatarow.inspectioId && x.DetailID == formdatarow.rowid);


        //            if (UpdateRowData != null)
        //            {
        //                ImageName = "";

        //                if (dataArrayImage != null && dataArrayImage.Count > 0)
        //                {
        //                    var picture = dataArrayImage.First();
        //                    var fileExtension = Path.GetExtension(picture.FileName);
        //                    ImageName = Guid.NewGuid().ToString() + fileExtension;
        //                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", ImageName);

        //                    // Save the file to the specified path
        //                    using (var stream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await picture.CopyToAsync(stream);
        //                    }

        //                }
        //                UpdateRowData.UserAnswerAfter = formdatarow.answer;
        //                UpdateRowData.CommetAnswerAfter = formdatarow.note;
        //                UpdateRowData.PicAfter = ImageName;

        //                _context.InspectionsDet.Update(UpdateRowData);
        //            }



        //        }
        //        await _context.SaveChangesAsync();
        //        return Json(new { success = true, message = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }

        //}

        //[HttpPost]
        //        public async Task<IActionResult> UpdateFormTable([FromBody]List<IFormFile> images, [FromForm] List<string> inspectioIds,  List<long> rowIds,  List<int> answers,  List<string> notes)
        //public async Task<IActionResult> UpdateFormTable([FromBody]List<ImageUploadModel> DataEite)
        //{
        //    try
        //    {
        //        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");

        //        for (int i = 0; i < inspectioIds.Count; i++)
        //        {
        //            var inspectioId = inspectioIds[i];
        //            var rowId = rowIds[i];
        //            var answer = answers[i];
        //            var note = notes[i];
        //            var image = images[i];

        //            var UpdateRowData = await _context.InspectionsDet.FirstOrDefaultAsync(x => x.InspectionID == inspectioId && x.DetailID == rowId);
        //            if (UpdateRowData != null)
        //            {
        //                string imageName = "";
        //                if (image != null && image.Length > 0)
        //                {
        //                    var fileExtension = Path.GetExtension(image.FileName);
        //                    imageName = Guid.NewGuid().ToString() + fileExtension;
        //                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", imageName);

        //                    // Save the file to the specified path
        //                    using (var stream = new FileStream(filePath, FileMode.Create))
        //                    {
        //                        await image.CopyToAsync(stream);
        //                    }
        //                }

        //                UpdateRowData.UserAnswerAfter = answer;
        //                UpdateRowData.CommetAnswerAfter = note;
        //                UpdateRowData.PicAfter = imageName;

        //                _context.InspectionsDet.Update(UpdateRowData);
        //            }
        //            await _context.SaveChangesAsync();
        //            return Json(new { success = true, message = "Data updated successfully." });
        //        }

        //        return Json(new { success = false, message = "" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error
        //        Console.WriteLine($"An error occurred while updating form rows: {ex}");

        //        // Return detailed error message
        //        return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        //    }
        //}
        //public class ImageUploadModel
        //{
        //    public string? inspectioId { get; set; }
        //    public long? rowid { get; set; }
        //    public int answer { get; set; }
        //    public string note { get; set; }
        //    //public IFormFile ImageAfter { get; set; }
        //    //public Stream ImageAfter { get; set; }

        //}
        #endregion

        #region "ItemsReport"
        [Authorize(Permissions.Report.ReportItems)]
        public IActionResult ItemsReport()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }

        [HttpGet]
        public IActionResult GetDepartment()
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var itemsDep = _context.Departments
                     .AsNoTracking()
                     .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                     .OrderBy(x => x.DepName_E)
                     .Select(x => new SelectListItem { Text = x.DepName_E, Value = x.DepartmentID })
                     .ToList();

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }

        #endregion

        #region "Orders Report"
        [Authorize(Permissions.Report.ReportOrders)]
        public IActionResult OrdersReport()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }

        [HttpPost]
        public IActionResult GetOrders(string dataObj)
        {
            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(_CompanyID) || string.IsNullOrEmpty(dataObj))
                {
                    return Json(new { error = "Invalid CompanyID" });
                }

                ReportFilterVM dataOk = JsonConvert.DeserializeObject<ReportFilterVM>(dataObj);

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<VMOrders> tableInspMasters = Enumerable.Empty<VMOrders>().AsQueryable();

                if (dataOk.isComeFrom == 0)
                {
                    tableInspMasters = _context.Vmorders
                        .Where(x => x.CompanyID == _CompanyID);
                }
                else if (dataOk.isComeFrom == 1)
                {
                    if (!string.IsNullOrEmpty(dataOk.filterDateFrom) && !string.IsNullOrEmpty(dataOk.filterDateTo))
                    {
                        if (DateTime.TryParseExact(dataOk.filterDateFrom, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromDate) &&
                            DateTime.TryParseExact(dataOk.filterDateTo, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toDate))
                        {
                            tableInspMasters = _context.Vmorders
                                .Where(x => x.CompanyID == _CompanyID
                                         && x.dtCraeteStamp >= fromDate.Date
                                         && x.dtCraeteStamp <= toDate.Date);
                        }
                        else
                        {
                            return Json(new { error = "Invalid date format" });
                        }
                    }
                    else
                    {
                        DateTime currentDateTo = DateTime.Now;
                        DateTime currentDateFrom = DateTime.Now;

                        switch (dataOk.filterPeriod)
                        {
                            case 1:
                                currentDateFrom = currentDateTo;
                                break;
                            case 2:
                                currentDateFrom = currentDateTo.AddDays(-7);
                                break;
                            case 3:
                                currentDateTo = currentDateTo.AddDays(-7);
                                currentDateFrom = currentDateTo.AddDays(-7);
                                break;
                            case 4:
                                currentDateFrom = currentDateTo.AddMonths(-1);
                                break;
                            case 5:
                                currentDateFrom = currentDateTo.AddYears(-1);
                                break;
                        }

                        tableInspMasters = _context.Vmorders
                            .Where(x => x.CompanyID == _CompanyID
                                     && x.dtCraeteStamp >= currentDateFrom.Date
                                     && x.dtCraeteStamp <= currentDateTo.Date);
                    }
                }

                tableInspMasters = tableInspMasters.OrderByDescending(o => o.Order_cd);
                var recordsTotal = tableInspMasters.Count();
                var data = tableInspMasters.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(datajson);

        ////ReportFilterVM dataOk = JsonConvert.DeserializeObject<ReportFilterVM>(dataObj);

        ////var _CompanyID = HttpContext.Session.GetString("CompanyID");
        ////var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        ////var _UserID = HttpContext.Session.GetString("UserID");

        ////var Skip = int.Parse(Request.Form["start"]);
        ////var pageSize = int.Parse(Request.Form["length"]);

        ////var sortColumnIndex = Request.Form["order[0][column]"];
        ////var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
        ////var sortColumnDirection = Request.Form["order[0][dir]"];

        ////IQueryable<VMOrders> tableInspMasters = Enumerable.Empty<VMOrders>().AsQueryable();

        //////IQueryable<VMInspMaster> tableInspMasters;

        ////if (dataOk.isComeFrom == 0)
        ////{
        ////    tableInspMasters = _context.Vmorders
        ////               .Where(x => x.CompanyID == _CompanyID);
        ////}
        ////else if (dataOk.isComeFrom == 1) //Filter by Inspection Type
        ////{

        ////    //if (dataOk.filterPeriod == 0 && string.IsNullOrEmpty(dataOk.filterDateFrom)==false  && string.IsNullOrEmpty(dataOk.filterDateTo)== false)
        ////    if (string.IsNullOrEmpty(dataOk.filterDateFrom) == false && string.IsNullOrEmpty(dataOk.filterDateTo) == false)
        ////    {
        ////        var fromDate = DateTime.Parse(dataOk.filterDateFrom);
        ////        var toDate = DateTime.Parse(dataOk.filterDateTo);

        ////        DateTime fromd = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
        ////        DateTime tod = new DateTime(toDate.Year, toDate.Month, toDate.Day);

        ////        tableInspMasters = _context.Vmorders
        ////                       .Where(x => x.CompanyID == _CompanyID
        ////                                && x.dtCraeteStamp >= fromd
        ////                                && x.dtCraeteStamp <= tod);
        ////        //&& x.dtCraeteStamp >= DateTime.Parse(dataOk.filterDateFrom)
        ////        //                        && x.dtCraeteStamp <= DateTime.Parse(dataOk.filterDateTo));
        ////    }
        ////    else
        ////    {

        ////        //<Value = "1" > This Today 
        ////        //<value = "2" > This Week
        ////        //<value = "3" > Before Week
        ////        //<value = "4" > Before Month
        ////        //<value = "5" > Before Year
        ////        DateTime currntDateTo = GeneralFun.GetCurrentTime();
        ////        DateTime currntDateFrom = GeneralFun.GetCurrentTime();

        ////        if (dataOk.filterPeriod == 1)
        ////            currntDateFrom = currntDateTo;
        ////        if (dataOk.filterPeriod == 2)
        ////            currntDateFrom = currntDateTo.AddDays(-7);
        ////        if (dataOk.filterPeriod == 3)
        ////        {
        ////            currntDateTo = currntDateTo.AddDays(-7);
        ////            currntDateFrom = currntDateTo.AddDays(-7);
        ////        }
        ////        if (dataOk.filterPeriod == 4)
        ////            currntDateFrom = currntDateTo.AddDays(-30);
        ////        if (dataOk.filterPeriod == 5)
        ////            currntDateFrom = currntDateTo.AddDays(-365);

        ////        tableInspMasters = _context.Vmorders
        ////               .Where(x => x.CompanyID == _CompanyID
        ////                        && x.dtCraeteStamp >= currntDateFrom
        ////                        && x.dtCraeteStamp <= currntDateTo);
        ////    }
        ////}

        //////tableInspMasters = tableInspMasters.OrderBy($"{sortColumn} {sortColumnDirection}");
        ////tableInspMasters = tableInspMasters.OrderByDescending(o => o.Order_cd);
        ////var recordsTotal = tableInspMasters.Count();
        ////var data = tableInspMasters.Skip(Skip).Take(pageSize).ToList();

        ////var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

        ////return Json(datajson);
    }
            catch (Exception ex)
            {

                return Json(new { success = true, returnData = ex.Message });
            }
        }

        // Action to export data to Excel
        [HttpGet]
        public IActionResult ExportToExcel()
        {
            try
            {
                //var data = _context.Vmorders.ToList();

                //using (var package = new ExcelPackage())
                //{
                //    var worksheet = package.Workbook.Worksheets.Add("Data");

                //    //// Add column headers
                //    //for (int i = 0; i < data.Columns.Count; i++)
                //    //{
                //    //    worksheet.Cells[1, i + 1].Value = data.Columns[i].ColumnName;
                //    //}

                //    //// Add data rows
                //    //for (int i = 0; i < data.Rows.Count; i++)
                //    //{
                //    //    for (int j = 0; j < data.Columns.Count; j++)
                //    //    {
                //    //        worksheet.Cells[i + 2, j + 1].Value = data.Rows[i][j];
                //    //    }
                //    //}
                //    var properties = typeof(VMOrders).GetProperties();
                //    for (int i = 0; i < properties.Length; i++)
                //    {
                //        worksheet.Cells[1, i + 1].Value = properties[i].Name;
                //    }

                //    // Add data rows
                //    for (int i = 0; i < data.Count; i++)
                //    {
                //        var item = data[i];
                //        for (int j = 0; j < properties.Length; j++)
                //        {
                //            worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(item);
                //        }
                //    }
                //    var stream = new MemoryStream();
                //    package.SaveAs(stream);
                //    stream.Position = 0;
                //    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "data.xlsx");
                //}
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        #endregion

    }
}
