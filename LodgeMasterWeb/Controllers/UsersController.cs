using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<ApplicationRole> _RoleManager;
        //private string _CompanyID = "2525cc";// string.Empty;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;
        //private string _CompanyFolder = "hjcont";
        // private string _UserLoginID;
        public UsersController(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _empImagePath = _webHostEnvironment.WebRootPath;
            _RoleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetRoleGroup()
        {
            try
            {
                //var itemsDep = _context.SysRoles
                //     // .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0)
                //     .AsNoTracking()
                //     .Select(x => new SelectListItem { Text = x.RollName_E, Value = x.RoleID })
                //     .OrderBy(x => x.Text)
                //     .ToList();
                var itemsDep = _RoleManager.Roles
                     // .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0)
                     .AsNoTracking()
                     .Select(x => new SelectListItem { Text = x.Name, Value = x.Id })
                     .OrderBy(x => x.Text)
                     .ToList();

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
        }
        public IActionResult Users()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetUsers(string department, int visor, int bActive)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];
                //var mySQL = "";
                //var depSQL = "";
                //var ActivekSQL = "";
                //var supervisorSQL = "";

                //IQueryable<Employee> Employeies = _context.Employees.Where(user => user.CompanyID == _CompanyID && user.isDeleted == 0);

                ////mySQL = "SELECT * FROM Employees WHERE CompanyID='" + _CompanyID + "' ";
                //if (department != "0")
                //{
                //    //depSQL = " AND DepartmentID='" + department + "'";
                //    Employeies = Employeies.Where(user => user.DepartmentID == department);
                //}
                //if (bActive == 1)
                //{
                //    //ActivekSQL = " AND ItemStock=1";
                //    Employeies = Employeies.Where(user => user.bActive == bActive);
                //}
                //if (visor == 1)
                //{
                //    //supervisorSQL = " AND supervisor=1";
                //    Employeies = Employeies.Where(user => user.supervisor == visor);// && user.bActive == bActive);
                //}

                // mySQL += depSQL + ActivekSQL + supervisorSQL;

                //UsersGrid = _context.Employees.FromSqlRaw(mySQL).OrderBy($"{sortColumn} {sortColumnDirection}")
                //Aziz
                //IQueryable<UsersViewModel> UsersGrid = Employeies
                //                                .OrderBy($"{sortColumn} {sortColumnDirection}")
                //                                .Select(user => new UsersViewModel
                //                                {
                //                                    EmpID = user.EmpID,
                //                                    User_cd = user.User_cd,
                //                                    // UserLogin = user.UserLogin,
                //                                    FirstName = user.FirstName,
                //                                    LastName = user.LastName,
                //                                    bPhoto = user.bPhoto,
                //                                    Photopath = user.Photopath,
                //                                    //DepartmentID = user.DepartmentID,
                //                                    //DepartmentName = user.DepartmentData.DepName_E,
                //                                    // UserGroup = user.UserGroup,
                //                                    //UserRoleName = user.RoleData.RollName_E,
                //                                    sEmail = user.sEmail,
                //                                    //CreateEmpID = user.CreateEmpID,
                //                                    //CreateEmpName = user,
                //                                    bActive = user.bActive,
                //                                    // bActiveAccept = user.bActiveAccept,
                //                                    // LangDef = user.LangDef,
                //                                    // expiredate = user.expiredate,
                //                                    // mobile = user.mobile,
                //                                    supervisor = user.supervisor,
                //                                    //dtpasswordupdate = user.dtpasswordupdate,
                //                                    //iSorted = user.iSorted,
                //                                    //isDeleted = user.isDeleted
                //                                });
                //var ResultUsers = UsersGrid.ToList();

                //var recordsTotal = UsersGrid.Count();
                // var data = UsersGrid.Skip(Skip).Take(pageSize).ToList();

                //var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json("");// datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                CreateUserFormViewModel viewModel = new()
                {
                    DepartmentData = _context.Departments.AsNoTracking()
                                    .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                                    .Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                                    .OrderBy(o => o.Text)
                                    .ToList(),
                    //sysRole = _context.SysRoles.AsNoTracking()
                    //                //.Where(w =>  w.isDeleted == 0)
                    //                .Select(s => new SelectListItem { Text = s.RollName_E, Value = s.RoleID })
                    //                .OrderBy(o => o.Text)
                    //                .ToList(),

                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(CreateUserFormViewModel model)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            if (!ModelState.IsValid)
            {
                model.DepartmentData = _context.Departments.AsNoTracking()
                                    .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                                    .Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                                    .OrderBy(o => o.Text)
                                    .ToList();
                //model.sysRole = _context.SysRoles.AsNoTracking()
                //                //.Where(w =>  w.isDeleted == 0)
                //                .Select(s => new SelectListItem { Text = s.RollName_E, Value = s.RoleID })
                //                .OrderBy(o => o.Text)
                //                .ToList();
                return View(model);
            }

            //save data
            //save image in server

            var empImage = $"{Guid.NewGuid()}{Path.GetExtension(model.EmployeeImage.FileName)}";
            //var pathImage = Path.Combine($"{_empImagePath}/Companies/{_CompanyFolder}/images/employee", empImage);
            var pathImage = Path.Combine(_empImagePath, "Companies", _CompanyFolder, "images", "employee", empImage);

            using (var stream = new FileStream(pathImage, FileMode.Create))
            {
                await model.EmployeeImage.CopyToAsync(stream);

            }

            Employee DataEmp = new()
            {
                EmpID = Guid.NewGuid().ToString(),
                User_cd = 1,
                UserLogin = model.UserLogin,
                UserPassword = model.UserPassword,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyID = _CompanyID,
                DepartmentID = model.DepartmentID,
                //UserGroup = model.UserGroup,
                sEmail = model.sEmail,
                CreateEmpID = _UserID,
                bActive = model.bActive,
                bPhoto = empImage,
                Photopath = _CompanyFolder,
                bActiveAccept = 0,
                LangDef = 0,
                expiredate = 0,
                mobile = model.mobile,
                supervisor = 0,
                dtpasswordupdate = GeneralFun.GetCurrentTime(),// DateTime.Now,
                iSorted = 0,
                isDeleted = 0

            };
            _context.Employees.Add(DataEmp);
            _context.SaveChanges();

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult EditUser(string empid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var mySQL = "";

                mySQL = "SELECT * FROM VMUsers WHERE CompanyID='" + _CompanyID + "' AND EmpId='" + empid + "'";

                var UserData = _context.Employees.FromSqlRaw(mySQL).FirstOrDefault();

                if (UserData != null)
                {
                    return Json(new { success = true, returnData = UserData });
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
        public IActionResult SaveEditUser(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var isFound = false;
            Employee dataOk = JsonConvert.DeserializeObject<Employee>(dataObj);

            if (string.IsNullOrEmpty(dataOk.FirstName) == true)
            {
                return Json(new { success = false, returnData = "enter first name" });
            }

            if (string.IsNullOrEmpty(dataOk.LastName) == true)
            {
                return Json(new { success = false, returnData = "enter last name" });
            }

            if (string.IsNullOrEmpty(dataOk.sEmail) == true)
            {
                return Json(new { success = false, returnData = "enter e-mail address" });
            }
            else
            {
                isFound = _context.Employees.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.sEmail == dataOk.sEmail && x.EmpID != dataOk.EmpID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "e-mail already exists." });
                }
            }
            if (string.IsNullOrEmpty(dataOk.mobile) == true)
            {
                return Json(new { success = false, returnData = "enter mobile number" });
            }
            else
            {
                isFound = _context.Employees.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.mobile == dataOk.mobile && x.EmpID != dataOk.EmpID);
                if (isFound == true)
                {
                    return Json(new { success = false, returnData = "Arabic Name already exists." });
                }
            }

            var UserUpdate = _context.Employees.FirstOrDefault(i => i.CompanyID == _CompanyID && i.EmpID == dataOk.EmpID);

            if (UserUpdate != null)
            {

                //bPhoto: ''
                UserUpdate.FirstName = dataOk.FirstName;
                UserUpdate.LastName = dataOk.LastName;
                UserUpdate.sEmail = dataOk.sEmail;
                UserUpdate.mobile = dataOk.mobile;
                UserUpdate.DepartmentID = dataOk.DepartmentID;
                //UserUpdate.UserGroup = dataOk.UserGroup;
                UserUpdate.bActive = dataOk.bActive;
                UserUpdate.supervisor = dataOk.supervisor;


                _context.Employees.Update(UserUpdate);
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
        public IActionResult DeleteUser(string empid)
        {

            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var UserRemove = _context.Employees.FirstOrDefault(i => i.CompanyID == _CompanyID && i.EmpID == empid);

                if (UserRemove != null)
                {
                    UserRemove.isDeleted = 1;

                    _context.Employees.Update(UserRemove);
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
    }
}
