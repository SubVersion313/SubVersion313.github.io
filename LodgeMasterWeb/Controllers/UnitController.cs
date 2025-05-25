using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using LodgeMasterWeb.Services;
using Microsoft.CodeAnalysis.Operations;
using Newtonsoft.Json;
using static LodgeMasterWeb.Controllers.UnitController;

namespace LodgeMasterWeb.Controllers;
public class UnitController : Controller
{
    private readonly ApplicationDbContext _context;
    //private string _CompanyID = "2525cc";// string.Empty;
    private List<RoomTypeManagmentVM> roomNumbers = new List<RoomTypeManagmentVM>();
    public UnitController(ApplicationDbContext context)
    {
        _context = context;
    }
    public IActionResult Unit()
    {
        return View();
    }
    [HttpGet]
    public IActionResult UnitRooms()
    {
        var _CompanyID = HttpContext.Session.GetString("CompanyID");
        //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
        //var _UserID = HttpContext.Session.GetString("UserID");

        // var ListUnitTypeName = _context.UnitsType.Where(c => CompanyID =).Tolist();
        UnitRoomViewModel UnitRoomData = new()
        {
            //ListAreaTypeName = _context.Items
            //                .AsNoTracking()
            //                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
            //                .OrderBy(x => x.ItemName_E)
            //                .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
            //                .ToList(),
            // ListUnitTypeName= _context.CompanyUnitsCat
            //                .AsNoTracking()
            //                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
            //                .OrderBy(x => x.ItemName_E)
            //                .Select(s => new SelectListItem { Value = s.ItemID, Text = s.ItemName_E })
            //                .ToList(),
            //ListFloorName = _context.Floors
            //                .AsNoTracking()
            //                .Where(c => c.CompanyID == _CompanyID && c.bActive == 1 && c.IsDeleted == 0)
            //                .OrderBy(x => x.DepName_E)
            //                .Select(s => new SelectListItem { Value = s.DepartmentID, Text = s.DepName_E })
            //                .ToList()
        };


        return View();
    }
    [HttpPost]
    public IActionResult UnitRooms(UnitRoomViewModel model)
    {
        return View();
    }
    public IActionResult UnitType()
    {
        return View();
    }
    public IActionResult UnitFloor()
    {
        return View();
    }
    public IActionResult UnitWizard()
    {
        //ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
        return View();
    }
    public async Task<IActionResult> GetAllFloors()
    {
        List<SelectListItem> FloorsNo = new List<SelectListItem>();

        for (int i = 1; i < 21; i++)
        {
            FloorsNo.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
        }

        return Json(new { success = true, returnData = FloorsNo });

    }
    //[HttpPost]
    //public async Task<IActionResult> GeneraterRooms(string dataObj)
    //{
    //    try
    //    {


    //        var _CompanyID = HttpContext.Session.GetString("CompanyID");
    //        var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
    //        var _UserID = HttpContext.Session.GetString("UserID");

    //        GenerateRooms dataOk = JsonConvert.DeserializeObject<GenerateRooms>(dataObj);

    //        int FloorsNo = int.Parse(dataOk.Floors);
    //        List<string> roomNumbers = new List<string>();
    //        if (FloorsNo < 10)
    //        {

    //            string startNo = dataOk.StartRoomNo.Substring(1, (dataOk.StartRoomNo.Length - 1));
    //            string endNo = dataOk.EndRoomNo.Substring(1, (dataOk.EndRoomNo.Length - 1));

    //            //int start = int.Parse(dataOk.StartRoomNo);
    //            //int end = int.Parse(dataOk.EndRoomNo);
    //            int start = int.Parse(startNo);
    //            int end = int.Parse(endNo);


    //            string RoomName = "";
    //            for (int f = 1; f <= FloorsNo; f++)
    //            {
    //                RoomName = "";
    //                for (int r = start; r <= end; r++)
    //                {
    //                    if (r < 10)
    //                    {
    //                        RoomName = $"{f}0{r}";
    //                    }
    //                    else
    //                    {
    //                        RoomName = $"{f}{r}";
    //                    }

    //                    roomNumbers.Add(RoomName);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            string startNo = dataOk.StartRoomNo.Substring(1, (dataOk.StartRoomNo.Length - 1));
    //            string endNo = dataOk.EndRoomNo.Substring(1, (dataOk.EndRoomNo.Length - 1));

    //            //int start = int.Parse(dataOk.StartRoomNo);
    //            //int end = int.Parse(dataOk.EndRoomNo);
    //            int start = int.Parse(startNo);
    //            int end = int.Parse(endNo);


    //            string RoomName = "";
    //            for (int f = 1; f <= FloorsNo; f++)
    //            {
    //                RoomName = "";
    //                for (int r = start; r <= end; r++)
    //                {
    //                    if (r < 10)
    //                    {
    //                        RoomName = $"{f}00{r}";
    //                    }
    //                    else
    //                    {
    //                        RoomName = $"{f}0{r}";
    //                    }

    //                    roomNumbers.Add(RoomName);
    //                }
    //            }
    //        }
    //        return Json(new { success = true, returnData = roomNumbers });
    //    }
    //    catch (Exception)
    //    {

    //        return Json(new { success = false });
    //    }
    //}
    [HttpPost]
    public IActionResult GeneraterRooms(string dataObj)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            GenerateRooms dataOk = JsonConvert.DeserializeObject<GenerateRooms>(dataObj);

            int FloorsNo = int.Parse(dataOk.Floors);


            //if (FloorsNo < 10)
            //{

            int CountRoom = int.Parse(dataOk.RoomsNo);
            //int StartNo = int.Parse(dataOk.StartRoomNo);

            int countIndex = 0;
            int counter = 1;
            string RoomName = "";

            for (int f = 1; f <= FloorsNo; f++)
            {
                RoomName = "";
                counter = 1;// StartNo;
                for (int r = 1; r <= CountRoom; r++)
                {

                    if (counter < 10)
                    {
                        RoomName = $"{f}0{counter}";
                    }
                    else
                    {
                        RoomName = $"{f}{counter}";
                    }
                    countIndex += 1;
                    counter += 1;
                    roomNumbers.Add(new RoomTypeManagmentVM { RoomId = countIndex, RoomNumber = RoomName, Floor = f });
                }
            }
            //}
            //else
            //{
            //    int CountRoom = int.Parse(dataOk.RoomsNo);
            //    //int StartNo = int.Parse(dataOk.StartRoomNo);
            //    //int start = int.Parse(startNo);
            //    //int end = int.Parse(endNo);

            //    int countIndex = 0;
            //    int counter = 1;
            //    string RoomName = "";
            //    for (int f = 1; f <= FloorsNo; f++)
            //    {
            //        RoomName = "";
            //        //counter = StartNo;
            //        for (int r = 1; r <= CountRoom; r++)
            //        {

            //            if (counter < 10)
            //            {
            //                RoomName = $"{f}0{counter}";
            //            }
            //            else
            //            {
            //                RoomName = $"{f}{counter}";
            //            }
            //            countIndex += 1;
            //            counter += 1;
            //            roomNumbers.Add(new RoomTypeManagmentVM { RoomId = countIndex, RoomNumber = RoomName, Floor = f });
            //        }
            //    }
            //}
            //TempData["RoomData"] = roomNumbers;
            //return RedirectToAction("DisplayRooms","Unit");
            //return RedirectToAction("DisplayRooms", new { RoomData = roomNumbers });
            //return Json(new { success = true, returnData = roomNumbers });

            HttpContext.Session.Set("RoomData", roomNumbers);
            //return RedirectToAction("DisplayRooms");
            return Json(new { success = true, returnData = roomNumbers });
        }
        catch (Exception)
        {

            return Json(new { success = false });
        }
    }
    public class GenerateRooms
    {
        public string Floors { get; set; }
        public string RoomsNo { get; set; }
        //public string StartRoomNo { get; set; }
    }
    public async Task<IActionResult> GenerateCategories()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var ListRoomCat = await _context.CompanyUnitsCat
                .Where(x => x.CompanyID == _CompanyID)
                .Select(x => new { x.UnitCatId, x.UnitCatName, x.UnitCatColor })
                .ToListAsync();

            string DrawList = "";
            int TotalRooms = 0;
            var roomData = HttpContext.Session.Get<List<RoomTypeManagmentVM>>("RoomData");
            if (roomData != null)
            {
                TotalRooms = roomData.Count;
            }
            if (ListRoomCat.Count > 0)
            {
                DrawList = $@"<div class='card-item' style='--main-color: #ede6f9'>
                            <h3 id='lblTotalCat' class='number'>{TotalRooms}</h3>
                            <div class='title'> <i class='fa-solid fa-bed'> </i><span class='text'>Total Rooms</span></div>
                            </div>";
                foreach (var item in ListRoomCat)
                {
                    DrawList += $@"<div id='{item.UnitCatId}' class='card-item' style='--main-color: {item.UnitCatColor}'>
                                <h3 id='lblCount_{item.UnitCatId}' class='number'>0</h3>
                                <div class='title'><i class='fa-solid fa-bed'></i><span id='CatName_{item.UnitCatId}' class='text'>{item.UnitCatName}</span></div>
                                </div>";

                }
            }

            return Json(new { success = true, returnData = DrawList });
        }
        catch (Exception)
        {

            return Json(new { success = false });
        }

    }

    #region UnitsRoom Display
    public class FloorData
    {
        public string floorId { get; set; }
        public string floorLabel { get; set; } = string.Empty;
        public List<RoomData> rooms { get; set; }
    }

    public class RoomData
    {
        public int roomId { get; set; }
        public string catId { get; set; }
        public string roomNumber { get; set; }
        public string floorId { get; set; }
    }

    public IActionResult DisplayRooms()
    {
        var roomData = HttpContext.Session.Get<List<RoomTypeManagmentVM>>("RoomData");
        // Pass the data to the view
        return View(roomData);
    }
    [HttpPost]
   // public async Task<IActionResult> AddUnitRooms([FromBody] List<FloorData> floors)
    public async Task<IActionResult> AddUnitRooms(string floors)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            if (floors == null)
            {
                return Json(new { success = false, returnData = true, message = "Data deserialization failed!" });
            }
            var floorData = JsonConvert.DeserializeObject<List<FloorData>>(floors);

            //if (floors == null || !floors.Any())
            //{
            //    return Json(new { success = false, returnData = true, message = "No data received!" });
            //}

            var isAnyUnits = await _context.CompanyUnits
                             .AnyAsync(x => x.CompanyID == _CompanyID && x.isDeleted == 0);

            if (isAnyUnits)
            {
                return Json(new { success = false, returnData = true, message = "Units already exist!" });
            }

            var companyUnitFloors = new List<CompanyUnitFloor>();
            var companyUnits = new List<CompanyUnit>();
            
            var maxIdService = new GenericService(_context);
            //int maxIdNumber = maxIdService.GetMaxId<Item>(e => e.CompanyID == _CompanyID, e => e.Item_cd);
            int maxSortedNumberFloors = maxIdService.GetMaxSorted<CompanyUnitFloor>(e => e.CompanyID == _CompanyID, e => e.iSorted);
            int maxSortedNumberUnits = maxIdService.GetMaxSorted<CompanyUnit>(e => e.CompanyID == _CompanyID, e => e.iSorted);

            foreach (var floor in floorData)//floors)
            {
                maxSortedNumberFloors++;

                var floorEntity = new CompanyUnitFloor
                {
                    FloorId = Guid.NewGuid().ToString(),
                    FloorName = string.IsNullOrEmpty(floor.floorLabel) ? $"Floor {floor.floorId}" : floor.floorLabel,
                    CompanyID = _CompanyID,
                    BranchID = string.Empty,
                    bActive = 1,
                    iSorted= maxSortedNumberFloors
                };

                companyUnitFloors.Add(floorEntity);
                
                foreach (var room in floor.rooms)
                {
                    maxSortedNumberUnits++;
                    var roomEntity = new CompanyUnit
                    {
                        LocationID = Guid.NewGuid().ToString(),
                        LocationName = room.roomNumber,
                        LocDesc = $"Room {room.roomNumber} in Floor {floor.floorId}",
                        CompanyID = _CompanyID,
                        BranchID = string.Empty,
                        FloorID = floorEntity.FloorId,
                        LocType = 1,
                        LocGroup = 1,
                        UnitCat = room.catId,
                        bActive = 1,
                        iSorted = maxSortedNumberUnits,
                        isDeleted = 0
                    };
                    companyUnits.Add(roomEntity);
                }
            }

            await _context.CompanyUnitFloors.AddRangeAsync(companyUnitFloors);
            await _context.CompanyUnits.AddRangeAsync(companyUnits);
            await _context.SaveChangesAsync();

            return Json(new { success = true, returnData = true, message = "Data saved successfully." });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, returnData = false, message = ex.Message });
        }
    }
    #endregion
    public async Task<IActionResult> GetAllRoomCatwithColor()
    {
        var _CompanyID = HttpContext.Session.GetString("CompanyID");

        var ListRoomCat = await _context.CompanyUnitsCat
            .Where(x => x.CompanyID == _CompanyID)
            .Select(x => new SelectListItem
            { Text = x.UnitCatName, Value = x.UnitCatId })
            .ToListAsync();
        var ListRoomCatColor = await _context.CompanyUnitsCat
                    .Where(x => x.CompanyID == _CompanyID)
                    .Select(x => new SelectListItem
                    { Text = x.UnitCatColor, Value = x.UnitCatId })
                    .ToListAsync();

        return Json(new { success = true, returnData = ListRoomCat, returnDataColor = ListRoomCatColor });

    }
    //public async Task<IActionResult> GetAllRoomCat()
    //{
    //    var _CompanyID = HttpContext.Session.GetString("CompanyID");

    //    var ListRoomCat = await _context.CompanyUnitsCat
    //        .Where(x => x.CompanyID == _CompanyID)
    //        .Select(x => new SelectListItem
    //        { Text = x.UnitCatName, Value = x.UnitCatId })
    //        .ToListAsync();

    //    return Json(new { success = true, returnData = ListRoomCat });

    //}
    [HttpPost]
    public async Task<IActionResult> UpdateCategoryColor(string catid, string colorname)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            if (catid == null || colorname == null)
                return Json(new { success = false, returnData = "error category color" });

            CompanyUnitCat CategoryData = await _context.CompanyUnitsCat
                .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID && x.UnitCatId == catid);

            if (CategoryData != null)
            {
                CategoryData.UnitCatColor = colorname;
                _context.CompanyUnitsCat.Update(CategoryData);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, returnData = "error category color" });
            }

        }
        catch (Exception ex)
        {
            return Json(new { success = false, returnData = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateCategoryRoom(string dataObj)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            CompanyUnitCat CategoryData = JsonConvert.DeserializeObject<CompanyUnitCat>(dataObj);

            if (CategoryData == null || string.IsNullOrEmpty(CategoryData.UnitCatName))
                return Json(new { success = false, returnData = "enter category name" });

            var isFoundData = await _context.CompanyUnitsCat
                            .AnyAsync(x => x.CompanyID == _CompanyID
                            && x.UnitCatName.ToUpper() == CategoryData.UnitCatName.ToUpper());
            if (isFoundData == true)
            {
                return Json(new { success = false, returnData = "Already category name exist!!" });
            }
            CategoryData.UnitCatId = Guid.NewGuid().ToString();
            CategoryData.CompanyID = _CompanyID;
            CategoryData.CreateEmpID = _UserID;
            CategoryData.IsDeleted = 0;
            CategoryData.UnitCatColor = CategoryData.UnitCatColor;

            _context.CompanyUnitsCat.Add(CategoryData);
            await _context.SaveChangesAsync();

            return Json(new { success = true });

        }
        catch (Exception ex)
        {
            return Json(new { success = false, returnData = ex.Message });
        }
    }
    public class RoomListRequest
    {
        public string Catroom { get; set; }
        public List<string> RoomIds { get; set; }
    }
    [HttpPost]
    public async Task<IActionResult> CreateRoomList([FromBody] RoomListRequest request)
    //public async Task<IActionResult> CreateRoomList(string dataObj,string catroom)
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            // var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var CatRoom = request.Catroom;

            List<string> RoomNames = request.RoomIds;

            //CompanyUnitCat CategoryData = JsonConvert.DeserializeObject<CompanyUnitCat>(dataObj);

            if (CatRoom == null || string.IsNullOrEmpty(CatRoom))
                return Json(new { success = false, returnData = "Room category is required." });


            if (RoomNames == null || RoomNames.Count == 0)
                return Json(new { success = false, returnData = "At least one room name is required." });


            foreach (var unitroom in RoomNames)
            {
                var UnitRoomFound = _context.CompanyUnits
                                   .Any(x => x.CompanyID == _CompanyID && x.LocationName == unitroom);

                if (UnitRoomFound == false)
                {
                    var newCompanyUnit = new CompanyUnit
                    {
                        LocationID = Guid.NewGuid().ToString(),
                        LocationName = unitroom,
                        CompanyID = _CompanyID,
                        UnitCat = CatRoom,
                        CreateEmpID = _UserID,
                        bActive = 1,
                        iSorted = 0,
                        isDeleted = 0
                    };

                    _context.CompanyUnits.Add(newCompanyUnit);
                }
            }
            await _context.SaveChangesAsync();

            return Json(new { success = true, returnData = "Room list saved successfully." });

        }
        catch (Exception ex)
        {
            return Json(new { success = false, returnData = ex.Message });
        }
    }
    [HttpGet]
    public async Task<IActionResult> GetAllCategoryRoom()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var ListRoomCat = await _context.CompanyUnitsCat
                .Where(x => x.CompanyID == _CompanyID)
                .Select(x => new { x.UnitCatId, x.UnitCatName, x.UnitCatColor })
                .ToListAsync();

            string DrawList = "";

            if (ListRoomCat.Count > 0)
            {
                foreach (var item in ListRoomCat)
                {
                    DrawList += $@"
                                <li class='d-flex justify-content-between align-items-center border-bottom pb-2 mb-3'>
                                    <div class='d-flex align-items-center'>
                                        <div class='icon me-2 rounded' data-catid='{item.UnitCatId}' style='width: 30px; height: 30px; background-color: {item.UnitCatColor}'></div>
                                        {item.UnitCatName}
                                    </div>
                                    <div class='d-flex align-items-center'>
                                        <label class='me-2'>Change Color: </label>
                                        <button type='button' class='btn btn-sm me-2' style='background-Color:{item.UnitCatColor}' 
                                        data-bs-toggle='modal' data-bs-target='#colorGridModal' data-catid='{item.UnitCatId}'></button>
                                    </div>
                                </li>";
                    //DrawList += $@"
                    //            <li class='d-flex justify-content-between align-items-center border-bottom pb-2 mb-3'>
                    //                <div class='d-flex align-items-center'>
                    //                    <div class='icon me-2 rounded' data-catid='{item.UnitCatId}' style='width: 30px; height: 30px; background-color: {item.UnitCatColor}'></div>
                    //                    {item.UnitCatName}
                    //                </div>
                    //                <div class='d-flex align-items-center'>
                    //                    <label class='me-2'>Change Color: </label>
                    //                    <input type='color' name='color_{item.UnitCatId}' value='{item.UnitCatColor}' />
                    //                   
                    //                </div>
                    //            </li>";
                }
            }

            return Json(new { success = true, returnData = DrawList });
        }
        catch (Exception)
        {

            return Json(new { success = false });
        }
    }
    [HttpPost]
    public async Task<IActionResult> SaveData([FromBody] List<FloorData> floorsData)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                string _BranchID = Guid.NewGuid().ToString();
                int sortedFloor = 1;

                // Loop through the received data and save to the database
                foreach (var floor in floorsData)
                {
                    // Save floor data
                    var FloorID = Guid.NewGuid().ToString();
                    var floorEntity = new CompanyUnitFloor
                    {
                        FloorId = FloorID,
                        FloorName = floor.floorLabel,
                        CompanyID = _CompanyID,
                        BranchID = _BranchID,
                        iSorted = sortedFloor,
                        bActive = 1
                    };
                    sortedFloor++;

                    await _context.CompanyUnitFloors.AddAsync(floorEntity);

                    int sortedRoom = 1;
                    foreach (var room in floor.rooms)
                    {
                        // Save room data
                        var roomEntity = new CompanyUnit
                        {
                            LocationID = Guid.NewGuid().ToString(),
                            LocationName = room.roomNumber,
                            LocDesc = string.Empty,
                            CompanyID = _CompanyID,
                            BranchID = _BranchID,
                            FloorID = FloorID,
                            LocType = 1,
                            LocGroup = 1,
                            UnitCat = room.catId,
                            CreateEmpID = _UserID,
                            bActive = 1,
                            iSorted = sortedRoom,
                            isDeleted = 0
                        };
                        sortedRoom++;

                        await _context.CompanyUnits.AddAsync(roomEntity);
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                UpdateStatusGenerateRooms();
                // Return a success response
                return Json(new { success = true, message = "Data saved successfully!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Log the exception (ex) here if needed
                return Json(new { success = false, message = "Data save error!" });
            }
        }

    }
    public async Task<IActionResult> UpdateStatusGenerateRooms()
    {
        try
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");

            Company CurrCompany = await _context.Companies
                .FirstOrDefaultAsync(x => x.CompanyID == _CompanyID);
            if (CurrCompany == null)
                return Json(new { success = false });

            CurrCompany.GenerateRoom = 1;
            _context.Companies.Update(CurrCompany);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false });
        }
    }
}

//public class FloorData
//{
//    public string FloorId { get; set; }
//    public string FloorLabel { get; set; }
//    public List<RoomData> Rooms { get; set; }
//}
//public class RoomData
//{
//    public int RoomId { get; set; }
//    public string RoomNumber { get; set; }
//    public string CatId { get; set; }
//    public string FloorId { get; set; }
//}

