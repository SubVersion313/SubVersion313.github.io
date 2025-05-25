using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LodgeMasterWeb.Seeds
{
    public static class defaultUsers
    {

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, string _CompanyID)
        {
            try
            {


                var defaultUser = new ApplicationUser
                {
                    UserName = "basic@domain.com",
                    Email = "basic@domain.com",
                    EmailConfirmed = true,
                    CompanyID = _CompanyID,//string.Empty,
                    FirstName = "basic",
                    LastName = "basic",
                    DepartmentID = string.Empty,
                    CreateEmpID = string.Empty,
                    bActive = 1,
                    //bytePhoto= null,
                    bPhoto = string.Empty,
                    Photopath = string.Empty,
                    expiredate = 0,
                    supervisor = 1,
                    dtpasswordupdate = GeneralFun.GetCurrentTime(),
                    iSorted = 2,
                    IsDeleted = 0
                };
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword123");
                    await userManager.AddToRoleAsync(defaultUser, enumRoles.Basic.ToString());

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, string _CompanyID, string _CompanyNameLogin)
        {
            try
            {
                var defaultUser = new ApplicationUser
                {
                    UserName = "Admin@lodgemaster.com",
                    Email = "admin@lodgemaster.com",
                    EmailConfirmed = true,
                    CompanyID = _CompanyID,//string.Empty,
                    FirstName = "admin",
                    LastName = "admin",
                    DepartmentID = string.Empty,
                    CreateEmpID = string.Empty,
                    bActive = 1,
                    //bytePhoto=null,
                    bPhoto = string.Empty,
                    Photopath = string.Empty,
                    expiredate = 0,
                    supervisor = 1,
                    dtpasswordupdate = DateTime.Now,
                    iSorted = 1,
                    IsDeleted = 0


                };
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "P@ssword123");
                    await userManager.AddToRolesAsync(defaultUser, new List<string> {
                    enumRoles.Admin.ToString() + "^" + _CompanyNameLogin,
                    //enumRoles.SuperAdmin.ToString(),
                    enumRoles.SuperVisor.ToString() + "^" + _CompanyNameLogin,
                    enumRoles.User.ToString() + "^" + _CompanyNameLogin,
                   // enumRoles.Basic.ToString(),

                });
                }
                //TODO: seed claims
                await roleManager.SeedClaimsForAdmin(_CompanyNameLogin);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }
        private static async Task SeedClaimsForAdmin(this RoleManager<ApplicationRole> roleManager, string _CompanyNameLogin)
        {
            try
            {


                var adminRole = await roleManager.FindByNameAsync(enumRoles.Admin.ToString() + "^" + _CompanyNameLogin);

                await roleManager.AddPermissionClaims(adminRole, moduleScreen.CreateOrders.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Dashboard.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Department.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Home.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.InspectionOrder.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.InspectionSetup.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Items.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Notifications.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Organization.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Profile.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Report.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Role.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.ServiceOrders.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Settings.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.StaffManagement.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Users.ToString());
                
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.Units.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.ShiftWork.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.SetupRooms.ToString());
                await roleManager.AddPermissionClaims(adminRole, moduleScreen.TmTask.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static async Task AddPermissionClaims(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
        {
            try
            {
                var allClaims = await roleManager.GetClaimsAsync(role);
                var allPermissions = Permissions.GeneratePermissionsList(module);

                foreach (var permission in allPermissions)
                {
                    if (!allClaims.Any(c => c.Type == "Permission" && c.Value == permission))
                    {
                        await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                //return Json(new { ex.Message });
            }
        }
    }
}
