using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class IdentityUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public IdentityUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        //public async Task<IActionResult> Index()
        //{
        //    try
        //    {
        //        ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

        //        var users = await _userManager.Users
        //              .Select(user => new IdentityUserViewModel
        //              {
        //                  Id = user.Id,
        //                  UserName = user.UserName,
        //                  FirstName = user.FirstName,
        //                  LastName = user.LastName,
        //                  Email = user.Email,
        //                  //Roles = _userManager.GetRolesAsync(user).Result
        //              })
        //              .ToListAsync();
        //        if (users != null)
        //        {
        //            foreach (var item in users)
        //            {
        //                var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(item.Id));
        //                item.Roles = roles.ToList();
        //            }

        //        }
        //        return View(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return View();
        //    }

        //}

        #region "Managment Roles"
        public async Task<IActionResult> ManageRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var roles = await _roleManager.Roles
                .Where(r => r.CompanyID == user.CompanyID)
                .ToListAsync();

            var viewModel = new IdentityRolesListViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(role => new IdentityCheckBoxViewModel
                {
                    DisplayValue = role.Name.Split('^')[0],
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoles(IdentityRolesListViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var _CompanyNameLogin = HttpContext.Session.GetString("CompanyNameLogin");

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, model.Roles
                            .Where(r => r.IsSelected)
                            .Select(r => r.DisplayValue + "^" + _CompanyNameLogin));

            //foreach (var role in model.Roles)
            //{
            //    if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
            //        await _userManager.RemoveFromRoleAsync(user, role.RoleName);

            //    if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
            //        await _userManager.AddToRoleAsync(user, role.RoleName);
            //}

            return RedirectToAction(nameof(IdentityUsers));
        }
        #endregion

        #region "Users"
        public async Task<IActionResult> IdentityUsers()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartment()
        {
            try
            {

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var currentUser = await _userManager.FindByIdAsync(_UserID);


                // Check if the user has the desired claim
                var hasClaim = User.HasClaim(c => c.Type == "Permission" && c.Value == "Permissions.StaffManagement.AllDepartments");

                List<SelectListItem> itemsDep;

                if (hasClaim == true)
                {
                    itemsDep = _context.Departments
                          .AsNoTracking()
                          .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0)
                          .OrderBy(x => x.DepName_E)
                          .Select(x => new SelectListItem { Text = x.DepName_E, Value = x.DepartmentID })
                          .ToList();
                }
                else
                {

                    itemsDep = _context.Departments
                          .AsNoTracking()
                          .Where(x => x.CompanyID == _CompanyID && x.IsDeleted == 0 && x.DepartmentID == currentUser.DepartmentID)
                          .OrderBy(x => x.DepName_E)
                          .Select(x => new SelectListItem { Text = x.DepName_E, Value = x.DepartmentID })
                          .ToList();
                }

                var datajson = new { success = true, returnData = itemsDep };
                return Json(datajson);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, returnData = ex.Message });
            }
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

                if (department == "0")
                {
                    return Json(new { success = false, returnData = "Please Select Department" });
                }
                var UsersGrid = _userManager.Users
                            .Where(user => user.CompanyID == _CompanyID && user.IsDeleted == 0);

                if (!string.IsNullOrEmpty(department) && department != "0")
                {
                    UsersGrid = UsersGrid.Where(user => user.DepartmentID == department);
                }
                if (visor == 1 && bActive == 1)
                {
                    UsersGrid = UsersGrid.Where(user => user.supervisor == visor && user.bActive == bActive);
                }
                else if (visor == 1 && bActive == 0)
                {
                    UsersGrid = UsersGrid.Where(user => user.supervisor == visor);
                }
                else if (visor == 0 && bActive == 1)
                {
                    UsersGrid = UsersGrid.Where(user => user.bActive == bActive);
                }

                //&& user.DepartmentID==department)
                //.OrderBy($"{sortColumn} {sortColumnDirection}")
                var ResultUsersGrid = UsersGrid.Select(user => new IdentityUserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    bPhoto = user.bPhoto,
                    Photopath = user.Photopath,
                    DepartmentID = user.DepartmentID,
                    //DepartmentName = GeneralFun.GetDepartmentNameById(user.DepartmentID, _CompanyID, _context),
                    Email = user.Email,
                    bActive = user.bActive,
                    PhoneNumber = user.PhoneNumber,
                    supervisor = user.supervisor,
                    iSorted = user.iSorted,
                    IsDeleted = user.IsDeleted
                })
                .ToList();


                if (UsersGrid != null)
                {
                    foreach (var item in ResultUsersGrid)
                    {
                        var roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(item.Id).Result).Result;
                        var roleNames = roles.Select(role => role.Split('^')[0]); // Select the first part before the '^'
                        item.RoleList = string.Join(" , ", roleNames);
                        ////var roles =  _userManager.GetRolesAsync( _userManager.FindByIdAsync(item.Id).Result);
                        //item.RoleList = string.Join(" , ", _userManager.GetRolesAsync(_userManager.FindByIdAsync(item.Id).Result).Result);
                        ////string.Join(" , ", _userManager.GetRolesAsync(item.Id));
                    }

                }

                var recordsTotal = ResultUsersGrid.Count(); //UsersGrid.Count();
                var data = ResultUsersGrid.Skip(Skip).Take(pageSize).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };

                return Json(datajson);
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

                //CreateUserFormViewModel viewModel = new()
                //{
                //    DepartmentData = _context.Departments.AsNoTracking()
                //.Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                //.Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                //.OrderBy(o => o.Text)
                //.ToList(),
                //    sysRole = _context.SysRoles.AsNoTracking()
                //                //.Where(w =>  w.isDeleted == 0)
                //                .Select(s => new SelectListItem { Text = s.RollName_E, Value = s.RoleID })
                //                .OrderBy(o => o.Text)
                //                .ToList(),

                //};

                IdentityUserViewModel viewModel = new()
                {
                    LstDepartment = _context.Departments
                            .AsNoTracking()
                            .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                            .Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                            .OrderBy(o => o.Text)
                            .ToList(),
                    LstAllRoles = _roleManager.Roles
                            .Where(w => w.CompanyID == _CompanyID)
                            .ToList()
                            .Select(s => new SelectListItem { Text = s.Name.Split('^')[0], Value = s.Id })
                            .ToList()

                };

                ////.Where(w =>  w.isDeleted == 0)
                //                .Select(s => new SelectListItem { Text = s.RollName_E, Value = s.RoleID })
                //                .OrderBy(o => o.Text)
                //                .ToList(),
                return View(viewModel);// View(viewModel);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(IdentityUserViewModel model)//CreateUserFormViewModel model)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            var _CompanyNameLogin = HttpContext.Session.GetString("CompanyNameLogin");

            if (!ModelState.IsValid)
            {
                model.LstDepartment = _context.Departments
                            .AsNoTracking()
                            .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                            .Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                            .OrderBy(o => o.Text)
                            .ToList();
                model.LstAllRoles = _roleManager.Roles
                            .Where(w => w.CompanyID == _CompanyID)
                            .ToList()
                            .Select(s => new SelectListItem { Text = s.Name.Split('^')[0], Value = s.Id })
                            .ToList();

                return View(model);
            }

            //save data
            //save image in server

            //var empImage = $"{Guid.NewGuid()}{Path.GetExtension(model.EmployeeImage.FileName)}";
            //var pathImage = Path.Combine($"{_empImagePath}/Companies/{_CompanyFolder}/images/employee", empImage);
            //var pathImage = Path.Combine(_empImagePath, "Companies", _CompanyFolder, "images", "employee", empImage);
            //var pathImage = Path.Combine("~", "Companies", _CompanyFolder, "images", "employee", empImage);

            //using (var stream = new FileStream(pathImage, FileMode.Create))
            //{
            //    await model.EmployeeImage.CopyToAsync(stream);

            //}
            var isExists = _context.Users.Any(u => u.PhoneNumber == model.PhoneNumber);
            if (isExists== true)
            {
                model.LstDepartment = _context.Departments
                            .AsNoTracking()
                            .Where(w => w.CompanyID == _CompanyID && w.IsDeleted == 0)
                            .Select(s => new SelectListItem { Text = s.DepName_E, Value = s.DepartmentID })
                            .OrderBy(o => o.Text)
                            .ToList();
                model.LstAllRoles = _roleManager.Roles
                            .Where(w => w.CompanyID == _CompanyID)
                            .ToList()
                            .Select(s => new SelectListItem { Text = s.Name.Split('^')[0], Value = s.Id })
                            .ToList();

                return View(model);
            }


            ApplicationUser DataEmp = new()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                CompanyID = _CompanyID,
                DepartmentID = model.DepartmentID,
                Email = model.Email,
                UserName = model.Email.ToLower(),// model.UserName,
                //PasswordHash = model.Password,
                CreateEmpID = _UserID,
                bActive =model.bActive2 ==true?1:0, //1,//model.bActive,
                //bPhoto = empImage,
                Photopath = _CompanyFolder,
                expiredate = 0,
                PhoneNumber = model.PhoneNumber,
                supervisor = model.supervisor2 == true ? 1 : 0, //model.supervisor,
                dtpasswordupdate = GeneralFun.GetCurrentTime(),// DateTime.Now,
                iSorted = 0,
                IsDeleted = 0,

            };
            //RoleSelectedValues
            var user = await _userManager.FindByEmailAsync(DataEmp.Email);
            //var userLogin = await _userManager.FindByLoginAsync(DataEmp.UserName);

            if (user == null)
            {

                List<string> lstrole = new List<string>();
                if (model.RoleSelectedValues != null)
                {
                    foreach (var roleitem in model.RoleSelectedValues)
                    {

                        var roleData = _roleManager.Roles
                                    .FirstOrDefault(w => w.CompanyID == _CompanyID && w.Id == roleitem);
                        if (roleData != null)
                        {
                            lstrole.Add(roleData.Name);// enumRoles.Admin.ToString().ToUpper() + "^" + _CompanyNameLogin.ToUpper());

                        }
                    }
                }
                string standerPwd = "Az@123456";
                //await _userManager.CreateAsync(DataEmp, model.hd_Password);
                if (GeneralFun.IsFirstCharBetweenAandZ(model.FirstName) == true && GeneralFun.IsFirstCharBetweenAandZ(model.LastName) == true)
                {
                    standerPwd = model.FirstName[0].ToString().ToUpper() + model.FirstName[0].ToString().ToLower() + "@123456";
                }
                await _userManager.CreateAsync(DataEmp, standerPwd);
                await _userManager.AddToRolesAsync(DataEmp, lstrole);
                // _context.Employees.Add(DataEmp);
                // _context.SaveChanges();
            }
            return RedirectToAction(nameof(IdentityUsers));
        }
        
        [HttpPost]
        public JsonResult CheckMobile(string mobileNumber)
        {
            // استبدل هذا الاستعلام بالاستعلام المناسب حسب هيكل قاعدة بياناتك
            var isExists = _context.Users.Any(u => u.PhoneNumber == mobileNumber);

            return Json(new { exists = isExists });
        }

        [HttpPost]
        public IActionResult EditUser(string empid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                //mySQL = "SELECT * FROM VMUsers WHERE CompanyID='" + _CompanyID + "' AND Id='" + empid + "'";

                //var UserData = _context.Employees.FromSqlRaw(mySQL).FirstOrDefault();
                var UserData = _userManager.Users.FirstOrDefault(w => w.Id == empid);

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
        public async Task<IActionResult> SaveEditUser(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _UserID = HttpContext.Session.GetString("UserID");

            ApplicationUser dataOk = JsonConvert.DeserializeObject<ApplicationUser>(dataObj);

            // التحقق من الحقول المطلوبة
            if (string.IsNullOrEmpty(dataOk.FirstName))
            {
                return Json(new { success = false, returnData = "Enter first name" });
            }

            if (string.IsNullOrEmpty(dataOk.LastName))
            {
                return Json(new { success = false, returnData = "Enter last name" });
            }

            if (string.IsNullOrEmpty(dataOk.PhoneNumber))
            {
                return Json(new { success = false, returnData = "Enter mobile number" });
            }

            // التحقق من أن رقم الهاتف غير مكرر
            //var isFound = _context.Users.AsNoTracking().Any(x => x.CompanyID == _CompanyID && x.PhoneNumber == dataOk.PhoneNumber && x.Id != dataOk.Id);
            var isFound = _context.Users.AsNoTracking().Any(x => x.PhoneNumber == dataOk.PhoneNumber && x.Id != dataOk.Id);


            if (isFound)
            {
                return Json(new { success = false, returnData = "Phone number already exists." });
            }

            // العثور على المستخدم المراد تعديله باستخدام UserManager
            var user = await _userManager.FindByIdAsync(dataOk.Id);
            if (user != null)
            {
                // تحديث الحقول في AspNetUsers
                user.FirstName = dataOk.FirstName;
                user.LastName = dataOk.LastName;
                user.PhoneNumber = dataOk.PhoneNumber;
                user.DepartmentID = dataOk.DepartmentID;
                user.bActive = dataOk.bActive;
                user.supervisor = dataOk.supervisor;

                // حفظ التعديلات باستخدام UserManager
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true, returnData = "Saved" });
                }
                else
                {
                    return Json(new { success = false, returnData = "Update failed" });
                }
            }
            else
            {
                return Json(new { success = false, returnData = "User not found" });
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
        #endregion
    }
}
