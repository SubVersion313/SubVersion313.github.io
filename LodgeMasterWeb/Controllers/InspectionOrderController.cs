using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;

//using LodgeMasterWeb.Migrations;
using Newtonsoft.Json;

namespace LodgeMasterWeb.Controllers
{
    public class InspectionOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InspectionOrderController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager)
        {
            _context = context;
            _userManager = UserManager;
        }
        public IActionResult InspectionOrder()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }

        [HttpGet]
        public IActionResult InspectionWizard()//string InspectionGUID1, string InspectionId1)
        {
            ////////////////////
            ///تجهيز صفحة طرح الائسلة حس القسم
            ///////////////////

            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            var inspectionid = HttpContext.Session.GetString("inspectionid");
            //var locationid = HttpContext.Session.GetString("locationid");
            //var locationName = HttpContext.Session.GetString("locationname");
            var inspectionGuid = HttpContext.Session.GetString("inspectionGuid");



            var model = GetInspWizardDisplay("1", inspectionid, inspectionGuid);


            return View(model);

        }

        public IActionResult StartQuestion()
        {
            ////////////////////
            ///تجهيز الاسائلة وتحديثه عددها على شكل مجموعات حسب الاقسام
            ///////////////////
            try
            {
                if (!GeneralFun.CheckLoginUser(HttpContext))
                {
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var inspectionid = HttpContext.Session.GetString("inspectionid");
                var locationid = HttpContext.Session.GetString("locationid");
                var locationName = HttpContext.Session.GetString("locationname");

                var inspectionGuid = Guid.NewGuid().ToString();
                //تخزين رقم لل انبيكشن
                HttpContext.Session.SetString("inspectionGuid", inspectionGuid);
                //حذف اي شي متعلق بالمستخدم مع رقم الشركة من جدول السلة 
                _context.Database.ExecuteSqlRaw("DELETE FROM InspectionBaskets WHERE CompanyID ='" + _CompanyID + "' AND UserId ='" + _UserID + "'");
                _context.SaveChanges();

                //جلب الائلة المتعلقة برقم الاسبكشن المختار مع ترتيبها في قائمة
                var InspBasketQuestion = _context.VMLinkQuestions
                    .Where(i => i.CompanyID == _CompanyID && i.InspectionId == inspectionid)
                    .OrderBy(i => i.SortPart)
                    .ThenBy(i => i.SortQuestion)
                    .ToList();

                if (InspBasketQuestion != null && InspBasketQuestion.Any())
                {
                    var countCurr = 0;

                    foreach (var question in InspBasketQuestion)
                    {
                        countCurr += 1;
                        //اضافة الاسائلة في جدول السلة
                        _context.InspectionBaskets.Add(new InspectionBasket
                        {
                            InspectionGUID = inspectionGuid,
                            CompanyID = question.CompanyID,
                            dtEntry = GeneralFun.GetCurrentTime(),// DateTime.Now,
                            LocationID = locationid,
                            LocationName = locationName,
                            InspectionId = question.InspectionId,
                            InspectionName = question.InspName,
                            PartId = question.PartID,
                            PartName = question.InspDepName,
                            QuestionId = question.QuestionID,
                            QuestionName = question.QuestionDisplay,
                            UserAnswer = 0,
                            CommetAnswer = string.Empty,
                            PicAnswer = string.Empty,
                            UserId = _UserID,
                            CurrntQuestion = countCurr == 1 ? 1 : 0,
                            SortQuestion = countCurr
                        }); ;

                    }
                    _context.SaveChanges();


                    // اختيار الاسائلة مع جعلها في مجموعات حسب الاقسام في النموذج

                    var itemsDep = _context.InspectionBaskets
                            .Where(i => i.CompanyID == _CompanyID
                                        && i.InspectionGUID == inspectionGuid
                                        && i.UserId == _UserID
                                        && i.InspectionId == inspectionid)
                            .GroupBy(i => new { i.PartId, i.PartName })
                            .Select(g => new
                            {
                                PartId = g.Key.PartId,
                                PartName = g.Key.PartName
                            })
                            .ToList();

                    if (itemsDep != null)
                    {

                        var countQuestion = 0;
                        var questionNo = 0;
                        //الدوران على المجوعات مع اختيار السوال
                        foreach (var item in itemsDep)
                        {

                            var DepDetils = _context.InspectionBaskets
                                        .Where(i => i.CompanyID == _CompanyID
                                            && i.InspectionGUID == inspectionGuid
                                            && i.UserId == _UserID
                                            && i.InspectionId == inspectionid
                                            && i.PartId == item.PartId)
                                        .OrderBy(i => i.SortQuestion)
                                        .ToList();
                            //تحديث عدد الاسائلة لكل مجموعة
                            if (DepDetils != null)
                            {
                                countQuestion = DepDetils.Count();
                                foreach (var qut in DepDetils)
                                {
                                    questionNo += 1;
                                    qut.QuestionTotal = countQuestion.ToString();
                                    qut.QuestionNo = questionNo;
                                    _context.InspectionBaskets.Update(qut);
                                }
                                _context.SaveChanges();
                                countQuestion = 0;
                                questionNo = 0;
                            }
                        }
                    }

                }

                //return Json(new { result = true });
                return RedirectToAction("InspectionWizard");//
                //return View(nameof(InspectionWizard), model);
                // return (model);
                //return RedirectToAction(nameof(InspectionWizard));
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetInspectionName()
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            try
            {
                var Result = await _userManager.GetUserAsync(User);

                if (Result != null)
                {
                    var DepartmentIdUser = Result.DepartmentID;

                    var itemsDep = _context.InspectionInfos
                         .AsNoTracking()
                         .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)// && x.DepartmentID==DepartmentIdUser)
                         .OrderBy(x => x.InspName)
                         .Select(x => new SelectListItem { Text = x.InspName, Value = x.InspInfoId })
                         .ToList();

                    var datajson = new { success = true, returnData = itemsDep };
                    return Json(datajson);
                }
                else
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

            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult AutocompleteLocation(string prefix, string Inspectionid)
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
        [HttpPost]
        public IActionResult StartInspectionSession(string inspectionid, string locationid, string locationname)
        {
            ////////////////////
            ///تجهيز جلسة انشاء inspection
            ///////////////////
            try
            {
                ////////////////////
                ///تخزين  رقم انسبيكشن المختار + رقم الوكيش مع الاسم في session
                ///توجيه الى الصفحة الاسائلة
                /// StartQuestion عن طريق تنفيذ الاكشن
                ///////////////////
                //  checked data first

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var inspectionData = _context.InspectionInfos.Any(c => c.CompanyID == _CompanyID && c.InspInfoId == inspectionid);

                if (inspectionData == false)
                {
                    return Json(new { result = false });
                }

                var LocationData = _context.CompanyUnits.FirstOrDefault(c => c.CompanyID == _CompanyID && c.LocationID == locationid);

                if (LocationData == null)
                {
                    return Json(new { result = false });
                }

                var locName = LocationData.LocationName;

                HttpContext.Session.SetString("inspectionid", inspectionid);
                HttpContext.Session.SetString("locationid", locationid);
                HttpContext.Session.SetString("locationname", locName);

                //return RedirectToAction(nameof(InspectionStart));

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return Json(new { result = false });
            }
        }

        public InspectionWizardViewModel GetInspWizardDisplay(string questionNo, string Inspectionid, string InspectionGUID)
        {
            ////////////////////
            /// بداية الاسائلة الويزرد
            ///////////////////
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            try
            {
                InspectionWizardViewModel DataQuestion = new InspectionWizardViewModel();

                var itemsDep = _context.InspectionBaskets
                            .Where(i => i.CompanyID == _CompanyID
                                        && i.InspectionGUID == InspectionGUID
                                        && i.UserId == _UserID
                                        && i.InspectionId == Inspectionid)
                            .GroupBy(i => new { i.PartId, i.PartName })
                            .Select(g => new
                            {
                                PartId = g.Key.PartId,
                                PartName = g.Key.PartName
                            })
                            .ToList();

                List<VMDisplayPart> PartDisplay = new List<VMDisplayPart>();

                if (itemsDep != null)
                {

                    var countQuestion = 0;
                    var PartActive = 0;
                    var PartNo = 0;
                    var PartQuestionNo = 0;

                    VMDisplayPart dataList;

                    foreach (var item in itemsDep)
                    {
                        dataList = new VMDisplayPart();

                        PartNo += 1;
                        var DepDetils = _context.InspectionBaskets
                                    .Where(i => i.CompanyID == _CompanyID
                                        && i.InspectionGUID == InspectionGUID
                                        && i.UserId == _UserID
                                        && i.InspectionId == Inspectionid
                                        && i.PartId == item.PartId)
                                    .ToList();

                        if (DepDetils != null)
                        {
                            countQuestion = DepDetils.Count();
                            var PartActiveFind = DepDetils.Any(i => i.CurrntQuestion == 1);
                            if (PartActiveFind == true)
                            {
                                PartActive = 1;
                            }
                        }
                        dataList.PartNo = PartNo;
                        dataList.PartId = item.PartId;
                        dataList.PartName = item.PartName;
                        dataList.PartActive = PartActive;
                        dataList.PartQuestionNo = countQuestion;

                        PartDisplay.Add(dataList);

                        countQuestion = 0;
                        PartActive = 0;


                    }

                    var currtQuestion = _context.InspectionBaskets
                                    .FirstOrDefault(i => i.CompanyID == _CompanyID
                                        && i.InspectionGUID == InspectionGUID
                                        && i.UserId == _UserID
                                        && i.InspectionId == Inspectionid
                                        && i.CurrntQuestion == 1);

                    if (currtQuestion != null)
                    {

                        // DataQuestion.InspectionGUID=currtQuestion ;

                        DataQuestion.InspectionBasketId = currtQuestion.InspectionBasketId;
                        DataQuestion.InspectionGUID = currtQuestion.InspectionGUID;
                        DataQuestion.CompanyID = currtQuestion.CompanyID;
                        DataQuestion.dtEntry = currtQuestion.dtEntry;
                        DataQuestion.LocationID = currtQuestion.LocationID;
                        DataQuestion.LocationName = currtQuestion.LocationName;
                        DataQuestion.InspectionId = currtQuestion.InspectionId;
                        DataQuestion.InspectionName = currtQuestion.InspectionName;
                        DataQuestion.PartId = currtQuestion.PartId;
                        DataQuestion.PartName = currtQuestion.PartName;
                        DataQuestion.QuestionId = currtQuestion.QuestionId;
                        DataQuestion.QuestionName = currtQuestion.QuestionName;
                        DataQuestion.UserAnswer = currtQuestion.UserAnswer;
                        DataQuestion.CommetAnswer = currtQuestion.CommetAnswer;
                        DataQuestion.PicAnswer = currtQuestion.PicAnswer;
                        DataQuestion.UserId = currtQuestion.UserId;
                        DataQuestion.CurrntQuestion = currtQuestion.CurrntQuestion;
                        DataQuestion.SortQuestion = currtQuestion.SortQuestion;
                        DataQuestion.LstParts = PartDisplay;
                        DataQuestion.QuestionNo = currtQuestion.QuestionNo;
                        DataQuestion.QuestionTotal = currtQuestion.QuestionTotal;

                    }
                }

                // var datajson = new { success = true, returnDataPart = PartDisplay, returnDataQuestion = DataQuestion };
                return (DataQuestion);
            }
            catch (Exception ex)
            {
                return null;//Json(new { success = false, returnData = ex.Message });
            }
        }

        public async Task<IActionResult> AnswerYes(string dataObj, IFormFile file)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");


                InspectionBasket dataOk = JsonConvert.DeserializeObject<InspectionBasket>(dataObj);


                //////////////////////////////
                ///

                if (file != null && file.Length > 0)
                {
                    // Save the file to the server or perform other actions
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "inspection", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                        dataOk.PicAnswer = fileName;
                    }
                }

                //////////////////////////////
                ///
                var NextNoQuestion = 0;

                var NextQuestion = _context.InspectionBaskets
                                .FirstOrDefault(i => i.CompanyID == _CompanyID
                                    && i.InspectionBasketId > dataOk.InspectionBasketId
                                    && i.InspectionGUID == dataOk.InspectionGUID);

                if (NextQuestion != null)
                {
                    NextNoQuestion = NextQuestion.InspectionBasketId;
                }
                else
                {
                    NextNoQuestion = -1;
                }

                var depUpdate = _context.InspectionBaskets
                                .FirstOrDefault(i => i.CompanyID == _CompanyID
                                                && i.InspectionBasketId == dataOk.InspectionBasketId
                                                && i.InspectionGUID == dataOk.InspectionGUID);

                if (depUpdate != null)
                {
                    depUpdate.UserAnswer = dataOk.UserAnswer;
                    depUpdate.CommetAnswer = dataOk.CommetAnswer;
                    depUpdate.PicAnswer = dataOk.PicAnswer;
                    depUpdate.CurrntQuestion = 0;

                    _context.InspectionBaskets.Update(depUpdate);
                    //_context.SaveChanges();

                    if (NextNoQuestion != -1)
                    {
                        var UpdateNextQuestion = _context.InspectionBaskets
                                    .FirstOrDefault(i => i.CompanyID == _CompanyID
                                        && i.InspectionBasketId == NextNoQuestion
                                        && i.InspectionGUID == dataOk.InspectionGUID);
                        if (UpdateNextQuestion != null)
                        {
                            UpdateNextQuestion.CurrntQuestion = 1;
                            _context.InspectionBaskets.Update(UpdateNextQuestion);
                        }
                        _context.SaveChanges();
                        return Json(new { success = true, CurrntQuestion = NextNoQuestion });
                    }
                    else
                    {
                        return Json(new { success = true, CurrntQuestion = "-1" });
                    }

                }

                else
                {
                    return Json(new { success = false, message = "Error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult NextQuestion(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");


            InspectionBasket dataOk = JsonConvert.DeserializeObject<InspectionBasket>(dataObj);

            var depUpdate = _context.InspectionBaskets
                             .FirstOrDefault(i => i.CompanyID == _CompanyID
                                             && i.InspectionBasketId == dataOk.InspectionBasketId
                                             && i.InspectionGUID == dataOk.InspectionGUID);

            if (depUpdate != null)
            {

                return Json(new { success = true, CurrntQuestion = depUpdate });
            }

            else
            {
                return Json(new { success = false, CurrntQuestion = "" });
            }
        }
        public IActionResult FinishInspection(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            var _UserDep = GeneralFun.GetDepartmentIDForUserId(_UserID, _CompanyID, _userManager);

            var locationid = HttpContext.Session.GetString("locationid");
            var inspectionInfoid = HttpContext.Session.GetString("inspectionid");

            InspectionBasket dataOk = JsonConvert.DeserializeObject<InspectionBasket>(dataObj);

            if (dataOk != null)
            {

                //Save inspection Basket to InspectionMaster & InspectionDet Tables
                var InspectionIdNewGuid = Guid.NewGuid().ToString();

                InspectionMaster InspMater = new InspectionMaster
                {
                    InspectionID = InspectionIdNewGuid,
                    InspInfoId = inspectionInfoid,
                    LocationID = locationid,
                    CompanyID = _CompanyID,
                    CreateDate = GeneralFun.GetCurrentTime(),
                    CreateEmpID = _UserID,
                    EmpDepId = _UserDep,
                    statusName = "Open",
                    isDeleted = 0,
                };


                _context.InspectionsMaster.Add(InspMater);
                _context.SaveChanges();

                var InspBasketsDetail = _context.InspectionBaskets
                             .Where(i => i.CompanyID == _CompanyID
                                    && i.InspectionGUID == dataOk.InspectionGUID
                                    && i.UserId == _UserID)
                             .OrderBy(i => i.SortQuestion)
                             .ToList();

                if (InspBasketsDetail != null)
                {
                    foreach (var item in InspBasketsDetail)
                    {
                        InspectionDet Det = new InspectionDet
                        {
                            InspectionID = InspectionIdNewGuid,
                            QuestionID = item.QuestionId,
                            QuestionDisplay = item.QuestionName,
                            PartID = item.PartId,
                            PartName = item.PartName,
                            iSorted = item.SortQuestion,
                            UserAnswer = item.UserAnswer,
                            CommetAnswer = item.CommetAnswer,
                            PicBefore = item.PicAnswer,
                            PicBeforCreate = GeneralFun.GetCurrentTime(),//DateTime.Now,
                            PicAfter = string.Empty,
                            bDone = 0,
                            PicAfterCreate = GeneralFun.GetCurrentTime(),//DateTime.Now,// DateTime Det.PicAfterCreate 
                            DoneCreate = GeneralFun.GetCurrentTime(),//DateTime.Now,//DateTime Det.DoneCreate { get; set; }

                        };

                        _context.InspectionsDet.Add(Det);
                    }
                    _context.SaveChanges();
                }

                ////////
                ///create Order if Inespection Create
                ///

                //var InfoMasterInspection=_context.InspectionsMaster

                //   .Where(i => i. == dataOk.InspectionId)
                //   .FirstOrDefault();

                var InspectionId = dataOk.InspectionId;

                var CreateOrder = _context.InspectionInfos
                              .FirstOrDefault(i => i.CompanyID == _CompanyID
                                              && i.IsDeleted == 0
                                              && i.InspInfoId == InspectionId);
                if (CreateOrder != null)
                {
                    var isCreate = CreateOrder.InspToCreateOrder;

                    if (isCreate == 1)
                    {
                        //ToCreate Order By Answer
                        //item ispection it

                        //InspMater
                        var DepartmentToCreate = CreateOrder.DepartmentID;
                        if (CreateOrderFromInspection(InspMater, DepartmentToCreate) == false)
                        {
                            return Json(new { success = false, InspectionOrder = true });

                        }

                    }
                    else
                    {

                    }
                }
                return Json(new { success = true });
            }
            else
            {

                return Json(new { success = false });
            }

        }
        public bool CreateOrderFromInspection(InspectionMaster _InspMater, string _DepartmentId)
        {
            var OkCraete = false;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    ///Master Order Table
                    ///
                    OrderMaster OrderMS = new OrderMaster();

                    var OrderGUID = Guid.NewGuid().ToString();

                    var maxOrderCd = _context.OrdersMaster
                                    .Where(c => c.CompanyID == _InspMater.CompanyID) // Apply your condition here
                                    .Select(o => o.Order_cd)
                                    .DefaultIfEmpty()
                                    .Max();


                    if (maxOrderCd != null)
                    {
                        maxOrderCd += 1;
                    }
                    else
                    {
                        maxOrderCd = 1;
                    }

                    OrderMS.OrderID = OrderGUID;
                    OrderMS.CompanyID = _InspMater.CompanyID;// _CompanyID;
                    OrderMS.Order_cd = maxOrderCd;
                    OrderMS.dtCraete = Int32.Parse(GeneralFun.GetCurrentTime().ToString("yyyyMMdd"));
                    OrderMS.dtCraeteStamp = GeneralFun.GetCurrentTime();// DateTime.Now;// DateTime.ParseExact(DateTime.Now,"dd/MM/yyyy HH:mm:ss",);

                    OrderMS.LocationID = _InspMater.LocationID;

                    OrderMS.UserIDCreate = _InspMater.CreateEmpID;
                    OrderMS.sNotes = string.Empty;
                    OrderMS.Status = (int)enumStatus.Open;
                    OrderMS.CatID = 0;
                    OrderMS.DelayTime = GeneralFun.GetCurrentTime();// DateTime.Now;
                    OrderMS.StampAssign = GeneralFun.GetCurrentTime();// DateTime.Now;

                    //Change value
                    OrderMS.DepartmentID = _DepartmentId;

                    var DepartmentDataSuper = GeneralFun.GetSupervisorDep(_DepartmentId, _InspMater.CompanyID, _userManager);


                    OrderMS.DepartmentAssignUserId = DepartmentDataSuper.Result;
                    OrderMS.UserIDAssign = string.Empty;
                    OrderMS.DeptIDAssign = string.Empty;
                    OrderMS.ForSuperviser = 0;

                    OrderMS.LinkData = 0;
                    OrderMS.bDelay = 0;
                    OrderMS.isInspection = 1;
                    _context.OrdersMaster.Add(OrderMS);

                    ////END MASTER ORDER//////////

                    ////START DETAILS////////

                    var AnswerInspDet = _context.InspectionsDet
                                        .AsNoTracking()
                                        .Where(x => x.InspectionID == _InspMater.InspectionID
                                                && x.UserAnswer == 0)
                                        .OrderBy(x => x.DetailID)
                                        .ToList();

                    foreach (var item in AnswerInspDet)
                    {
                        var OrderDT = new OrderDet
                        {
                            OrderID = OrderGUID,
                            CompanyID = _InspMater.CompanyID,
                            ItemID = ConstVariables.InspectionItemForOrder,
                            Qty = 1,
                            sItemNotes = $"{item.PartName} > {item.QuestionDisplay}",
                            isClosed = 0,
                            PhotoName = item.PicBefore ?? ""

                        };
                        _context.OrdersDet.Add(OrderDT);
                    };
                    _context.SaveChanges();

                    ///////END DETAILS/////////
                    transaction.Commit();
                    OkCraete = true;
                    return OkCraete;


                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }


            }
        }
    }
}

