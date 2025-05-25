using LodgeMasterWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Security.Claims;


namespace LodgeMasterWeb.Controllers
{
    [Authorize]
    public class SplashController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;

        private readonly PermissionService _permissionService;

        public SplashController(PermissionService permissionService, RoleManager<ApplicationRole> RoleManager, UserManager<ApplicationUser> UserManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = UserManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = RoleManager;

            _permissionService = permissionService;
        }
        public async Task<IActionResult> Splash()
        {

            //if (GeneralFun.CheckLoginUser(HttpContext) == false)
            //{
            //    return RedirectToAction("index", "UserLogin");
            //}


            var statusUserIdentity = await SetUserInfoAsync();
            //var UserFullName = HttpContext.Session.GetString("UserFullName");
            // if (User.Identity.IsAuthenticated == true)
            if (statusUserIdentity == true)
            {
                //var UserInfo = await _userManager.GetUserAsync(User);
                //var UserFullName = UserInfo.FirstName + " " + UserInfo.LastName; 

                var UserFullName = HttpContext.Session.GetString("UserFullName") ?? "";

                ViewBag.UserFullName = UserFullName;// User.Identity.Name;
                ViewBag.UserIdData = HttpContext.Session.GetString("UserID");
                ViewBag.UserPhoto = HttpContext.Session.GetString("UserPhoto");

                var user = await _userManager.GetUserAsync(User);

                //ViewBag.HasReportPermission = userClaims.Any(c => c.Type == "Permission" && c.Value == "Permissions.ServiceOrders.View");
                // // جلب جميع الصلاحيات للمستخدم

                var permissions = await _permissionService.GetAllUserPermissions(user);

                // // تخزين الصلاحيات في الجلسة
                // HttpContext.Session.SetObjectAsJson("UserPermissions", permissions);
                //// تحويل الصلاحيات إلى JSON وتخزينها في الجلسة
               // HttpContext.Session.SetString("UserPermissions", JsonConvert.SerializeObject(permissions));
                //// تحويل الصلاحيات إلى JSON وتخزينها في الجلسة

                //bool hasPermissionCreateOrdersView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.CreateOrders.View");
                //bool hasPermissionDashboardView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Dashboard.View");
                //bool hasPermissionDepartmentView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Department.View");

                //bool hasPermissionInspectionOrderView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.InspectionOrder.View");
                //bool hasPermissionInspectionSetupView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.InspectionSetup.View");
                //bool hasPermissionItemsView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Items.View");

                //bool hasPermissionNotificationsView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Notifications.View");
                //bool hasPermissionOrganizationView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Organization.View");
                //bool hasPermissionProfileView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Profile.View");
                //bool hasPermissionReportAnalysisView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Report.Analysis");
                //bool hasPermissionReportOrdersView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Report.Orders");
                //bool hasPermissionReportItemsView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Report.Items");
                //bool hasPermissionReportInspectionView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Report.Inspection");

                //bool hasPermissionRoleView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Role.View");
                //bool hasPermissionServiceOrdersView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.ServiceOrders.View");

                //bool hasPermissionSettingsView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Settings.View");
                //bool hasPermissionStaffManagementView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.StaffManagement.View");
                //bool hasPermissionUsersView = permissions.Any(c => c.Type == "Permission" && c.Value == "Permissions.Users.View");
                //Type >= Key


                HttpContext.Session.SetString("UserPermissions", JsonConvert.SerializeObject(permissions));
                bool hasPermissionCreateOrdersView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.CreateOrders.View");
                bool hasPermissionDashboardView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Dashboard.View");
                bool hasPermissionDepartmentView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Department.View");

                bool hasPermissionInspectionOrderView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.InspectionOrder.View");
                bool hasPermissionInspectionSetupView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.InspectionSetup.View");
                bool hasPermissionItemsView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Items.View");

                bool hasPermissionNotificationsView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Notifications.View");
                bool hasPermissionOrganizationView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Organization.View");
                bool hasPermissionProfileView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Profile.View");
                bool hasPermissionReportAnalysisView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Report.Analysis");
                bool hasPermissionReportOrdersView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Report.Orders");
                bool hasPermissionReportItemsView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Report.Items");
                bool hasPermissionReportInspectionView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Report.Inspection");

                bool hasPermissionRoleView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Role.View");
                bool hasPermissionServiceOrdersView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.ServiceOrders.View");

                bool hasPermissionSettingsView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Settings.View");
                bool hasPermissionStaffManagementView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.StaffManagement.View");
                bool hasPermissionUsersView = permissions.Any(c => c.Key == "Permission" && c.Value == "Permissions.Users.View");

                ViewBag.HasPermissionCreateOrdersView = hasPermissionCreateOrdersView;
                ViewBag.HasPermissionDashboardView = hasPermissionDashboardView;
                ViewBag.HasPermissionDepartmentView = hasPermissionDepartmentView;
                ViewBag.HasPermissionInspectionOrderView = hasPermissionInspectionOrderView;
                ViewBag.HasPermissionInspectionSetupView = hasPermissionInspectionSetupView;

                ViewBag.HasPermissionItemsView = hasPermissionItemsView;
                ViewBag.HasPermissionNotificationsView = hasPermissionNotificationsView;
                ViewBag.HasPermissionOrganizationView = hasPermissionOrganizationView;
                ViewBag.HasPermissionProfileView = hasPermissionProfileView;

                ViewBag.HasPermissionReportAnalysisView = hasPermissionReportAnalysisView;
                ViewBag.HasPermissionReportOrdersView = hasPermissionReportOrdersView;
                ViewBag.HasPermissionReportItemsView = hasPermissionReportItemsView;
                ViewBag.HasPermissionReportInspectionView = hasPermissionReportInspectionView;

                ViewBag.HasPermissionRoleView = hasPermissionRoleView;
                ViewBag.HasPermissionServiceOrdersView = hasPermissionServiceOrdersView;
                ViewBag.HasPermissionSettingsView = hasPermissionSettingsView;
                ViewBag.HasPermissionStaffManagementView = hasPermissionStaffManagementView;
                ViewBag.HasPermissionUsersView = hasPermissionUsersView;

                return View();
            }

            else
            {
                //ViewBag.UserName = "";
                return RedirectToAction("Login", "Account");
            }

        }

        public async Task<bool> SetUserInfoAsync()
        {
            try
            {
                bool Result = false;
                if (User.Identity.IsAuthenticated == true)
                {
                    // var UserInfo = await _userManager.GetUserAsync(User);


                    var UserInfo = await _userManager.GetUserAsync(User);

                    if (UserInfo != null)
                    {
                        //wherepoint += "UserInfo| ";
                        var _CompanyID = UserInfo.CompanyID;
                        //wherepoint += _CompanyID + "| ";

                        string _CompanyFolder = "";
                        var company = _context.Companies.FirstOrDefault(c => c.CompanyID == _CompanyID);

                        if (company != null)
                        {
                            _CompanyFolder = company.CompanyFolder;
                        }

                        //user photot
                        string UserPhoto = "../../Images/avatar/bg-man.jpg"; //defaut

                        if (!string.IsNullOrEmpty(UserInfo.bPhoto))
                        {
                            UserPhoto = $"../../Companies/{_CompanyFolder}/images/employee/{UserInfo.bPhoto}";

                        }
                        //
                        HttpContext.Session.SetString("CompanyID", _CompanyID);
                        HttpContext.Session.SetString("CompanyFolder", _CompanyFolder);
                        HttpContext.Session.SetString("UserID", UserInfo.Id);
                        HttpContext.Session.SetString("UserFristName", UserInfo.FirstName.ToLower());
                        HttpContext.Session.SetString("UserLastName", UserInfo.LastName.ToLower());
                        HttpContext.Session.SetString("UserFullName", UserInfo.FirstName.ToLower() + " " + UserInfo.LastName.ToLower());
                        HttpContext.Session.SetString("UserPhoto", UserPhoto);
                        HttpContext.Session.SetString("CompanyNameLogin", company.CompanyNameLogin);


                        Result = true;
                    }

                }
                else
                {

                    RedirectToPage("Login", "Account");
                }
                return Result;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            //try
            //{
            //    await _signInManager.SignOutAsync();
            //    return RedirectToAction("Login", "Account");
            //}
            //catch (Exception)
            //{
            //    return RedirectToAction("Login", "Account");
            //    // يمكنك هنا تسجيل الخطأ إذا كنت ترغب في ذلك
            //}
            try
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.Clear(); // تأكد من مسح الجلسة
                //return RedirectToAction("Login", "Account");
                return RedirectToPage("/Account/Login", new { area = "Identity" });

                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.

            }
            catch (Exception ex)
            {
                throw;
              //  return RedirectToPage();
            }

        }

        //public async Task<List<Claim>> GetAllUserPermissions(ApplicationUser user)
        //{
        //    // قائمة لتجميع جميع الصلاحيات
        //    var allClaims = new List<Claim>();

        //    // 1. جلب جميع الصلاحيات المباشرة المسندة للمستخدم
        //    var userClaims = await _userManager.GetClaimsAsync(user);
        //    allClaims.AddRange(userClaims);

        //    // 2. جلب جميع الأدوار المرتبطة بالمستخدم
        //    var roles = await _userManager.GetRolesAsync(user);

        //    // 3. جلب الصلاحيات المرتبطة بكل دور
        //    foreach (var roleName in roles)
        //    {
        //        var role = await _roleManager.FindByNameAsync(roleName);
        //        if (role != null)
        //        {
        //            var roleClaims = await _roleManager.GetClaimsAsync(role);
        //            allClaims.AddRange(roleClaims);
        //        }
        //    }

        //    return allClaims.Distinct().ToList(); // إرجاع جميع الصلاحيات مع إزالة التكرارات
        //}
    }
}