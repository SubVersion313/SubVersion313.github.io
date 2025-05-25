//using AspNetCore;
using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Security.Claims;

namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    //[Authorize(Roles =Permissions. "Admin")]
    public class IdentityRoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityRoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (!GeneralFun.CheckLoginUser(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { Area = "Identity" });
            }

            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            //var roles = await _roleManager.Roles.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(IdentityRoleFormViewModel model)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyNameLogin = HttpContext.Session.GetString("CompanyNameLogin");

            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");
            if (!GeneralFun.CheckLoginUser(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { Area = "Identity" });
            }
            if (!ModelState.IsValid)
                return View();// "Index", await _roleManager.Roles.Where(c => c.CompanyID == _CompanyID).ToListAsync());

            var AllRoles = await _roleManager.Roles.Where(c => c.CompanyID == _CompanyID && c.isDeleted == 0).ToListAsync();

            string NewRoleName = model.Name + "^" + _CompanyNameLogin;

            if (AllRoles.Any())
            {

                foreach (var rl in AllRoles)
                {
                    if (rl.Name.ToUpper() == NewRoleName.ToUpper())//model.Name.ToUpper())
                    {
                        ModelState.AddModelError("Name", "Role is exists!");
                        return View();// "Index", await _roleManager.Roles.Where(c => c.CompanyID == _CompanyID).ToListAsync());
                    }
                }
            }
            //if (await _roleManager.Roles.RoleExistsAsync( model.Name))
            //{
            //    ModelState.AddModelError("Name", "Role is exists!");
            //    return View("Index", await _roleManager.Roles.ToListAsync());
            //}



            //await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            var role = new ApplicationRole
            {
                Name = NewRoleName,//model.Name,
                CompanyID = _CompanyID,
                isDefault = 0,
                isDeleted = 0,
                bActive = 1,
                Description = string.Empty
            };
            await _roleManager.CreateAsync(role);

            return View();// RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> SaveAddRole(string dataObj)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyNameLogin = HttpContext.Session.GetString("CompanyNameLogin");

            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");

            if (!GeneralFun.CheckLoginUser(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { Area = "Identity" });
            }

            var dataOk = JsonConvert.DeserializeObject<ApplicationRole>(dataObj);

            if (string.IsNullOrEmpty(dataOk.Name) == true)
            {
                return Json(new { success = false, returnData = "enter role name" });
            }

            //var AllRoles = await _roleManager.Roles.Where(c => c.CompanyID == _CompanyID && c.isDeleted == 0).ToListAsync();
            var AllRoles = await _roleManager.Roles.Where(c => c.CompanyID == _CompanyID && c.isDeleted == 0).ToListAsync();

            string NewRoleName = dataOk.Name + "^" + _CompanyNameLogin;

            if (AllRoles.Any())
            {
                foreach (var rl in AllRoles)
                {
                    if (rl.Name.ToUpper() == NewRoleName)//dataOk.Name.ToUpper())
                    {
                        return Json(new { success = false, returnData = "Role is exists!" });
                    }
                }
            }
            //if (await _roleManager.Roles.RoleExistsAsync( model.Name))
            //{
            //    ModelState.AddModelError("Name", "Role is exists!");
            //    return View("Index", await _roleManager.Roles.ToListAsync());
            //}

            if (string.IsNullOrEmpty(dataOk.Description))
            {
                dataOk.Description = string.Empty;
            }

            //await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
            var role = new ApplicationRole
            {
                Name = NewRoleName,// dataOk.Name,
                CompanyID = _CompanyID,
                isDefault = 0,
                isDeleted = 0,
                bActive = dataOk.bActive,
                Description = dataOk.Description
            };
            await _roleManager.CreateAsync(role);

            return Json(new { success = true, returnData = "Saved" });
        }
        public async Task<IActionResult> ManagePermissions(string roleId)
        {

            if (!GeneralFun.CheckLoginUser(HttpContext))
            {
                return RedirectToPage("/Account/Login", new { Area = "Identity" });
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return NotFound();

            var roleClaims = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToList();
            var allClaims = Permissions.GenerateAllPermissions();
            var allPermissions = allClaims.Select(p => new IdentityCheckBoxViewModel { DisplayValue = p }).ToList();

            foreach (var permission in allPermissions)
            {
                if (roleClaims.Any(c => c == permission.DisplayValue))
                    permission.IsSelected = true;
            }

            var viewModel = new IdentityPermissionsFormVM
            {
                RoleId = roleId,
                RoleName = role.Name.Split("^")[0],
                RoleCalims = allPermissions
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagePermissions(IdentityPermissionsFormVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
                return NotFound();

            var roleClaims = await _roleManager.GetClaimsAsync(role);

            foreach (var claim in roleClaims)
                await _roleManager.RemoveClaimAsync(role, claim);

            var selectedClaims = model.RoleCalims.Where(c => c.IsSelected).ToList();

            foreach (var claim in selectedClaims)
                await _roleManager.AddClaimAsync(role, new Claim("Permission", claim.DisplayValue));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> GetRoles(int bActive)
        {
            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            //var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            //var _UserID = HttpContext.Session.GetString("UserID");

            try
            {
                var Skip = int.Parse(Request.Form["start"]);
                var pageSize = int.Parse(Request.Form["length"]);

                var sortColumnIndex = Request.Form["order[0][column]"];
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"];
                var sortColumnDirection = Request.Form["order[0][dir]"];

                IQueryable<ApplicationRole> RoleCompany;

                if (bActive == -1)
                {
                    RoleCompany = _roleManager.Roles
                              .AsNoTracking()
                              .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0)
                              .OrderBy(x => x.Name);
                    //.ToList();
                }
                else
                {
                    RoleCompany = _roleManager.Roles
                              .AsNoTracking()
                              .Where(x => x.CompanyID == _CompanyID && x.isDeleted == 0 && x.bActive == bActive)
                              .OrderBy(x => x.Name);
                    //.ToList();
                }


                //var recordsTotal = RoleCompany.Count();
                //var data = RoleCompany.Skip(Skip).Take(pageSize).ToList();
                var recordsTotal = await RoleCompany.CountAsync();
                var data = await RoleCompany
                    .Skip(Skip)
                    .Take(pageSize)
                    .ToListAsync();

                var transformedRoles = data.Select(role => new
                {
                    Id = role.Id,
                    Name = role.Name.Split('^')[0],
                    role.Description,
                    role.isDefault,
                    role.bActive,
                    role.CompanyID,
                    role.isDeleted

                }).ToList();

                var datajson = new { recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = transformedRoles };// data };

                return Json(datajson);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, returnData = ex.Message });
            }

        }

        [HttpPost]
        public IActionResult EditRole(string roleId)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                // var _UserID = HttpContext.Session.GetString("UserID");

                var RoleCompany = _roleManager.Roles
                           .AsNoTracking()
                           .FirstOrDefault(x => x.CompanyID == _CompanyID && x.isDeleted == 0 && x.Id == roleId);



                if (RoleCompany != null)
                {
                    RoleCompany.Name = RoleCompany.Name.Split("^")[0];
                    return Json(new { success = true, returnData = RoleCompany });
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
        public async Task<IActionResult> SaveEditRole(string dataObj)
        {

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");
            var _CompanyNameLogin = HttpContext.Session.GetString("CompanyNameLogin");

            try
            {

                var dataOk = JsonConvert.DeserializeObject<ApplicationRole>(dataObj);

                string NewRoleName = "";

                if (string.IsNullOrEmpty(dataOk.Name) == true)
                {
                    return Json(new { success = false, returnData = "enter role name" });
                }

                else
                {
                    NewRoleName = dataOk.Name + "^" + _CompanyNameLogin;

                    var isDuplicate = _roleManager.Roles
                            .AsNoTracking()
                            .Any(c => c.CompanyID == _CompanyID
                                && c.Name.ToUpper() == NewRoleName.ToUpper()
                                && c.Id != dataOk.Id);

                    if (isDuplicate == true)
                    {
                        return Json(new { success = false, returnData = "role name already exists." });
                    }
                }

                var RoleUpdate = await _roleManager.FindByIdAsync(dataOk.Id);
                RoleUpdate.Name = NewRoleName;// dataOk.Name;
                RoleUpdate.Description = dataOk.Description;
                RoleUpdate.bActive = dataOk.bActive;

                await _roleManager.UpdateAsync(RoleUpdate);

                var resJson = new { success = true, returnData = "Saved" };
                return Json(resJson);
            }
            catch (Exception)
            {

                var resJson = new { success = false, returnData = "not saved" };
                return Json(resJson);
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleid)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                var RoleDelete = await _roleManager.FindByIdAsync(roleid);

                if (RoleDelete != null)
                {

                    var result = await _roleManager.DeleteAsync(RoleDelete);


                    var resJson = new { success = true, returnData = "Delete" };
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
