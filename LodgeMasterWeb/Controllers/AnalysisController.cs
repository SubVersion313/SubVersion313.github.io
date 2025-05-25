using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace LodgeMasterWeb.Controllers;
public class AnalysisController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly string _empImagePath;
    private readonly string _empAVATAR = @"Images/avatar/bg-man.jpg";
    public AnalysisController(ApplicationDbContext context, UserManager<ApplicationUser> UserManager, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = UserManager;
        _webHostEnvironment = webHostEnvironment;
        _empImagePath = _webHostEnvironment.WebRootPath;
        
    }
    [Authorize(Permissions.Report.ReportAnalysis)]
    public IActionResult Analysis()
    {
        //CalcAvg();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CalcAvg(int startDate, int endDate)
    {

        //SELECT avg( DateDiff(MINUTE,DayHour,dtAction)) as MINUTECount"
        //,CAST(avg( DateDiff(MINUTE,DayHour,dtAction)) / 1440 AS VARCHAR(8)) + 'd ' + CAST((avg( DateDiff(MINUTE,DayHour,dtAction)) % 1440) / 60 AS VARCHAR(8)) + 'h ' +    FORMAT(avg( DateDiff(MINUTE,DayHour,dtAction)) % 60, 'D2') + 'min' AS [TIME_TEXT]"
        //,count(*) as RecordCount,t3.Satatus,t4.Status_E "
        //,t1.CatID,t2.CatName_E"
        // From q_orderproblem t1 "
        // left outer join tblCat t2 On t1.CatID=t2.CatID "
        // Inner join tblRegisterAction t3 ON  t1.OrderID=t3.OrderID "
        // inner join tblStatus t4 ON t4.StatusID=t3.[Satatus] "
        // WHERE t1.CatID <> 0 AND dtCraete BETWEEN " & StartDate & " AND " & EndDate
        // AND t3.Satatus in (2,3,4,9,12)"
        // group by t3.[Satatus] ,t2.CatName_E,t1.Catid,t4.Status_E "
        // order by t1.CatID,[satatus]"

        try
        {
            var companyId = HttpContext.Session.GetString("CompanyID");
            //var companyFolder = HttpContext.Session.GetString("CompanyFolder");

            var start = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);
            //var searchValue = Request.Form["search[value]"];
            //var sortColumnIndex = Request.Form["order[0][column]"];
            //var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            //var sortColumnDirection = Request.Form["order[0][dir]"];

            //var excludedStatusIds = new List<int> { 2, 3, 4, 9, 12,13 };

            ////var result = await query.ToListAsync();
            //var ordersQuery = _context.AvgTicketsVM
            //    .Where(x => x.CompanyID == companyId
            //    && !excludedStatusIds.Contains(x.Satatus)
            //    //&& (x.dtCraete >= 20240101 && x.dtCraete<=20240530)
            //);

            //string sql = @"
            //            SELECT 
            //                    AVG(DATEDIFF(MINUTE, DayHour, dtAction)) AS MINUTECount,
            //                    CAST(AVG(DATEDIFF(MINUTE, DayHour, dtAction)) / 1440 AS VARCHAR(8)) + 'd ' + 
            //                    CAST((AVG(DATEDIFF(MINUTE, DayHour, dtAction)) % 1440) / 60 AS VARCHAR(8)) + 'h ' + 
            //                    FORMAT(AVG(DATEDIFF(MINUTE, DayHour, dtAction)) % 60, 'D2') + 'min' AS TIME_TEXT,
            //                    COUNT(*) AS RecordCount,
            //                    t3.Satatus,
            //                    t4.Status_E,
            //                    t1.CatID,
            //                    t2.CatName_E
            //            FROM q_orderproblem t1 
            //            LEFT OUTER JOIN tblCat t2 ON t1.CatID = t2.CatID 
            //            INNER JOIN tblRegisterAction t3 ON t1.OrderID = t3.OrderID 
            //            INNER JOIN tblStatus t4 ON t4.StatusID = t3.Satatus
            //            WHERE t1.CatID <> 0 
            //                AND t1.dtCraete BETWEEN @StartDate AND @EndDate
            //                AND t3.Satatus IN (2, 3, 4, 9, 12)
            //            GROUP BY t3.Satatus, t2.CatName_E, t1.CatID, t4.Status_E
            //            ORDER BY t1.CatID, t3.Satatus"
            //;
            string sql = @"
                        SELECT 
                            AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) AS MinuteCount,
                            CAST(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) / 1440 AS VARCHAR(8)) +' D ' +
                            CAST(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) % 1440 / 60 AS VARCHAR(8)) + ' H ' + 
                            FORMAT(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) % 60, 'D2') + ' M' AS Time_Text, 
                            COUNT(*) AS RecordCount,
                            t3.Satatus,
                            t4.Status_E, 
                            t1.DepartmentID,
                            t2.DepName_E,
                            t1.CompanyID
                        FROM  VMOrders AS t1 
                        LEFT OUTER JOIN Departments AS t2 ON t1.DepartmentID = t2.DepartmentID 
                        INNER JOIN OrdersAction AS t3 ON t1.OrderID = t3.OrderID
                        INNER JOIN SysStatus AS t4 ON t4.StatusID = t3.Satatus
                        WHERE 
                            t1.dtCraete BETWEEN @StartDate AND @EndDate
                            AND t3.Satatus IN (2, 3, 4, 9, 12,13)
                        GROUP BY t3.Satatus, t2.DepName_E, t1.DepartmentID, t4.Status_E, t1.CompanyID
                        ORDER BY t1.DepartmentID, t3.Satatus";

            var ordersQuery = await _context.AvgTicketsVM
                    .FromSqlRaw(sql, new SqlParameter("@StartDate", startDate), new SqlParameter("@EndDate", endDate))
                    .ToListAsync();


            //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

            var recordsTotal = ordersQuery.Count;// CountAsync();

            var data = ordersQuery.Skip(start).Take(pageSize).ToList();

            var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Json(datajson);
        }
        catch (Exception ex)
        {
            return Json(new { success = true, returnData = ex.Message });
        }
    }
    //[HttpPost]
    //public async Task<IActionResult> CalcAvg()
    //{

    //    //SELECT avg( DateDiff(MINUTE,DayHour,dtAction)) as MINUTECount"
    //    //,CAST(avg( DateDiff(MINUTE,DayHour,dtAction)) / 1440 AS VARCHAR(8)) + 'd ' + CAST((avg( DateDiff(MINUTE,DayHour,dtAction)) % 1440) / 60 AS VARCHAR(8)) + 'h ' +    FORMAT(avg( DateDiff(MINUTE,DayHour,dtAction)) % 60, 'D2') + 'min' AS [TIME_TEXT]"
    //    //,count(*) as RecordCount,t3.Satatus,t4.Status_E "
    //    //,t1.CatID,t2.CatName_E"
    //    // From q_orderproblem t1 "
    //    // left outer join tblCat t2 On t1.CatID=t2.CatID "
    //    // Inner join tblRegisterAction t3 ON  t1.OrderID=t3.OrderID "
    //    // inner join tblStatus t4 ON t4.StatusID=t3.[Satatus] "
    //    // WHERE t1.CatID <> 0 AND dtCraete BETWEEN " & StartDate & " AND " & EndDate
    //    // AND t3.Satatus in (2,3,4,9,12)"
    //    // group by t3.[Satatus] ,t2.CatName_E,t1.Catid,t4.Status_E "
    //    // order by t1.CatID,[satatus]"

    //    try
    //    {
    //        var companyId = HttpContext.Session.GetString("CompanyID");
    //        var companyFolder = HttpContext.Session.GetString("CompanyFolder");

    //        var start = int.Parse(Request.Form["start"]);
    //        var pageSize = int.Parse(Request.Form["length"]);
    //        //var searchValue = Request.Form["search[value]"];
    //        //var sortColumnIndex = Request.Form["order[0][column]"];
    //        //var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
    //        //var sortColumnDirection = Request.Form["order[0][dir]"];

    //        var excludedStatusIds = new List<int> { 2, 3, 4, 9, 12,13 };

    //        //var result = await query.ToListAsync();
    //        var ordersQuery = _context.AvgTicketsVM
    //            .Where(x => x.CompanyID == companyId
    //            && !excludedStatusIds.Contains(x.Satatus)
    //            //&& (x.dtCraete >= 20240101 && x.dtCraete<=20240530)
    //        );




    //        //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

    //        var recordsTotal = await ordersQuery.CountAsync();
    //        //var data = await ordersQuery.Skip(0).Take(100).ToListAsync();
    //        var data = await ordersQuery.Skip(start).Take(pageSize).ToListAsync();

    //        var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

    //        return Json(datajson);
    //    }
    //    catch (Exception ex)
    //    {
    //        return Json(new { success = true, returnData = ex.Message });
    //    }
    //}

    [HttpPost]
    public async Task<IActionResult> CalcItems(int startDate, int endDate)
    {

        try
        {
            var companyId = HttpContext.Session.GetString("CompanyID");
            //var companyFolder = HttpContext.Session.GetString("CompanyFolder");

            var start = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);
            //var searchValue = Request.Form["search[value]"];
            //var sortColumnIndex = Request.Form["order[0][column]"];
            //var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            //var sortColumnDirection = Request.Form["order[0][dir]"];

            //var excludedStatusIds = new List<int> { 2, 3, 4, 9, 12,13 };


            string sql = @"
                        Select 
                             Count(*)as RecordCount
                             ,sum(t1.Qty) as TotalQty
                             ,DepName_E
                             ,t3.ItemName_E
			                 ,t1.DepartmentID
			                 ,t1.CompanyID
                        From q_OrderDet t1
                        left outer join Departments t2 On t1.DepartmentID = t2.DepartmentID
                        left outer join Items t3 On t3.ItemID = t1.ItemID
                        WHERE 
			                t1.isinspection= 0
                            AND 
                            t1.dtCraete BETWEEN @StartDate AND @EndDate
                        Group by t3.ItemID,t3.ItemName_E,t2.DepName_E,t1.DepartmentID,t1.CompanyID
                        ORDER by recordcount desc";

            var ordersQuery = await _context.TotalItemsVM
                    .FromSqlRaw(sql, new SqlParameter("@StartDate", startDate), new SqlParameter("@EndDate", endDate))
                    .ToListAsync();


            //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

            var recordsTotal = ordersQuery.Count;// CountAsync();

            var data = ordersQuery.Skip(start).Take(pageSize).ToList();

            var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

            return Json(datajson);
        }
        catch (Exception ex)
        {
            return Json(new { success = true, returnData = ex.Message });
        }
    }
    [HttpPost]
    public async Task<IActionResult> CalcUsers(int startDate, int endDate)
    {

        try
        {
            var companyId = HttpContext.Session.GetString("CompanyID");
            //var companyFolder = HttpContext.Session.GetString("CompanyFolder");

            var start = int.Parse(Request.Form["start"]);
            var pageSize = int.Parse(Request.Form["length"]);
            //var searchValue = Request.Form["search[value]"];
            //var sortColumnIndex = Request.Form["order[0][column]"];
            //var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
            //var sortColumnDirection = Request.Form["order[0][dir]"];

            //var excludedStatusIds = new List<int> { 2, 3, 4, 9, 12,13 };


            string sql = @"
                         Select Z.UserID
                         FROM    (
                            SELECT UserIDCreate as UserID
                         FROM VMOrders
                         WHERE 
			                 UserIDCreate is not null 
			                 and 
			                 [StatusId] in (2,3,12)
                             AND 
			                 dtCraete BETWEEN @StartDate AND @EndDate
                         Union All
                            select UserIDAssign  as UserID
                         FROM VMOrders
                         WHERE 
			                 UserIDAssign is not null 
			                 and 
			                 [StatusId] in (2,3,12)
                             AND 
			                 dtCraete BETWEEN @StartDate AND @EndDate
                         ) As Z
                         Group By Z.UserID";

            //var ordersQuery = await _context
            //        .FromSqlRaw(sql, new SqlParameter("@StartDate", startDate), new SqlParameter("@EndDate", endDate))
            //        .ToListAsync();


            //var orderedOrders = ordersQuery.OrderBy($"{sortColumn} {sortColumnDirection}").ThenBy($"{sortColumn2} {sortColumnDirection2}");

            //var recordsTotal = ordersQuery.Count;// CountAsync();

            //var data = ordersQuery.Skip(start).Take(pageSize).ToList();

            //var datajson = new { recordsFiltered = recordsTotal, recordsTotal, data };

            //return Json(datajson);
            return Json("");
        }
        catch (Exception ex)
        {
            return Json(new { success = true, returnData = ex.Message });
        }
    }

    public IActionResult analysisChart()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AnalysisDepartment(string startDate, string endDate)
    {
        try
        {
            //DateTime startDateTime = DateTime.ParseExact(startDate, "yyyyMMdd", null);
            //DateTime endDateTime = DateTime.ParseExact(endDate, "yyyyMMdd", null);

            var ColorList = new List<string> { "#FFAC73", "#98E0AD", "#94D8F6", "#F8A1A4", "5", "6", "7", "8", "#F8A1A4", "10", "11", "#61CC80", "#FFFA99" };
            var DepartmentDataChart = new List<ChartData>();

            var CompanyId = HttpContext.Session.GetString("CompanyID");

            var DepertmentCompany = await _context.Departments
                                .Where(x => x.CompanyID == CompanyId)
                                .ToListAsync();
            if (DepertmentCompany == null || !DepertmentCompany.Any())
            {
                return Json(new { success = false, message = "Departments not found!" });
            }

            foreach (var departmentData in DepertmentCompany)
            {

                string sql = @"
                        SELECT 
                            AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) AS MinuteCount,
                            CAST(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) / 1440 AS VARCHAR(8)) +' D ' +
                            CAST(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) % 1440 / 60 AS VARCHAR(8)) + ' H ' + 
                            FORMAT(AVG(DATEDIFF(MINUTE, t1.dtCraeteStamp, t3.dtAction)) % 60, 'D2') + ' M' AS Time_Text, 
                            COUNT(*) AS RecordCount,
                            t3.Satatus,
                            t4.Status_E, 
                            t1.DepartmentID,
                            t2.DepName_E,
                            t1.CompanyID
                        FROM  VMOrders AS t1 
                        LEFT OUTER JOIN Departments AS t2 ON t1.DepartmentID = t2.DepartmentID 
                        INNER JOIN OrdersAction AS t3 ON t1.OrderID = t3.OrderID
                        INNER JOIN SysStatus AS t4 ON t4.StatusID = t3.Satatus
                        WHERE 
                            t1.dtCraete BETWEEN @StartDate AND @EndDate
                            AND t3.Satatus IN (1,2, 3, 4, 9, 12,13) 
                            AND t1.CompanyID=@CompanyID
                            AND t1.DepartmentID=@DepartmentID
                        GROUP BY t3.Satatus, t2.DepName_E, t1.DepartmentID, t4.Status_E, t1.CompanyID
                        ORDER BY t1.DepartmentID, t3.Satatus";

                var ordersQuery = await _context.AvgTicketsVM
                        .FromSqlRaw(sql, new SqlParameter("@StartDate", startDate), new SqlParameter("@EndDate", endDate)
                        , new SqlParameter("@CompanyID", CompanyId), new SqlParameter("@DepartmentID", departmentData.DepartmentID))
                        .ToListAsync();


                var chartData = new ChartData
                {
                    CompanyID = CompanyId,
                    DepartmentID = departmentData.DepartmentID,
                    DepartmentName = departmentData.DepName_E,
                    Series = new List<int>(),
                    Colors = new List<string>(),
                    Labels = new List<string>()
                };
                if (ordersQuery != null && ordersQuery.Count > 0)
                {
                    foreach (var order in ordersQuery)
                    {
                        chartData.CompanyID = CompanyId;
                        //chartData.CompanyName = order.CompanyName;
                        chartData.DepartmentID = order.DepartmentID;
                        chartData.DepartmentName = order.DepName_E;

                        if (order.RecordCount > 0)
                        {
                            chartData.Series.Add(order.RecordCount);
                            chartData.Labels.Add(order.Status_E);// = new List<string> { "Completed", "In Process", "ReOpen", "Hold", "Closed", "Defer" }
                            chartData.Colors.Add(ColorList[order.Satatus - 1]);// = new List<string> { "#845fc3", "#c28252", "#6ac66a", "#5899be", "#37a210", "#b85443" },
                        }
                    }
                    DepartmentDataChart.Add(chartData);
                }
            }

            return Json(new { success = true, returnData = DepartmentDataChart });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, returnData = ex.Message });
        }

    }
    public class ChartData
    {
        public string CompanyID { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public List<int> Series { get; set; } = new List<int>();
        public List<string> Colors { get; set; } = new List<string>();
        public List<string> Labels { get; set; } = new List<string>();
    }


    public class ItemChartData
    {
        public string[] ItemNames { get; set; }
        public int[] RecordCounts { get; set; }
        public int[] TotalQtys { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> AnalysisItems(string startDate, string endDate)
    {
        try
        {
            // Fetch data from your database based on the date range
            //var data = await FetchChartDataAsync(startDate, endDate);
            var companyId = HttpContext.Session.GetString("CompanyID");

            string sql = @"
                        Select  TOP 10 
                             Count(*)as RecordCount
                             ,sum(t1.Qty) as TotalQty
                             ,DepName_E
                             ,t3.ItemName_E
			                 ,t1.DepartmentID
			                 ,t1.CompanyID
                        From q_OrderDet t1
                        left outer join Departments t2 On t1.DepartmentID = t2.DepartmentID
                        left outer join Items t3 On t3.ItemID = t1.ItemID
                        WHERE 
			                t1.isinspection= 0
                            AND 
                            t1.dtCraete BETWEEN @StartDate AND @EndDate
                            AND t1.CompanyID=@CompanyId
                        Group by t3.ItemID,t3.ItemName_E,t2.DepName_E,t1.DepartmentID,t1.CompanyID
                        ORDER by recordcount desc";

            var ordersQuery = await _context.TotalItemsVM
                    .FromSqlRaw(sql, new SqlParameter("@CompanyId", companyId), new SqlParameter("@StartDate", startDate), new SqlParameter("@EndDate", endDate))
                    .ToListAsync();

            var itemNames = ordersQuery.Select(o => o.ItemName_E).ToArray();
            var recordCounts = ordersQuery.Select(o => o.RecordCount).ToArray();
            var totalQtys = ordersQuery.Select(o => o.TotalQty).ToArray();

            var chartData = new ItemChartData
            {
                ItemNames = itemNames,
                RecordCounts = recordCounts,
                TotalQtys = totalQtys
            };

            return Json(new { success = true, data = chartData });
        }

        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }



}
