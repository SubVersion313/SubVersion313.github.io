using LodgeMasterWeb.Contants;
using Microsoft.AspNetCore.Identity;
using System.Linq.Dynamic.Core;

namespace LodgeMasterWeb.Seeds
{
    public class defaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager, string _CompanyID, string _CompanyNameLogin)
        {
            //if(!roleManager.Roles.any()
            //{
            //    await roleManager.CreateAsync(new IdentityRole(enumRoles.Admin.ToString()));
            //    await roleManager.CreateAsync(new IdentityRole(enumRoles.SuperAdmin.ToString()));
            //    await roleManager.CreateAsync(new IdentityRole(enumRoles.SuperVisor.ToString()));
            //    await roleManager.CreateAsync(new IdentityRole(enumRoles.User.ToString()));
            //    await roleManager.CreateAsync(new IdentityRole(enumRoles.Basic.ToString()));
            //}

            bool RoleSelect = false;
            ApplicationRole role = null;

            string Admin = enumRoles.Admin.ToString() + "^" + _CompanyNameLogin;
            //RoleSelect = roleManager.Roles.Any(c => c.Name == enumRoles.Admin.ToString() && c.CompanyID==_CompanyID);
            RoleSelect = roleManager.Roles.Any(c => c.Name == Admin && c.CompanyID == _CompanyID);
            if (RoleSelect == false)
            {
                role = new ApplicationRole
                {
                    Name = Admin,//enumRoles.Admin.ToString(),
                    CompanyID = _CompanyID,
                    Description = string.Empty,
                    bActive = 1,
                    isDefault = 1,
                    isDeleted = 0
                };

                var result = await roleManager.CreateAsync(role);// (new ApplicationRoles(enumRoles.Admin.ToString()));
                if (!result.Succeeded)
                {
                    // Handle errors here
                    foreach (var error in result.Errors)
                    {
                        // Log or handle the error accordingly
                        Console.WriteLine($"Error: {error.Description}");
                    }
                }
            }

            //RoleSelect = roleManager.Roles.Any(c => c.Name == enumRoles.SuperAdmin.ToString() && c.CompanyID == _CompanyID);
            //if (RoleSelect == false)
            //{
            //    role = new ApplicationRole
            //    {
            //        Name = enumRoles.SuperAdmin.ToString(),
            //        CompanyID = _CompanyID,
            //        Description = string.Empty,
            //        bActive = 1,
            //        isDefault = 1,
            //        isDeleted = 0
            //    };
            //    await roleManager.CreateAsync(role);//new IdentityRole(enumRoles.SuperAdmin.ToString()));
            //}

            string SuperVisor = enumRoles.SuperVisor.ToString() + "^" + _CompanyNameLogin;
            // RoleSelect = roleManager.Roles.Any(c => c.Name == enumRoles.SuperVisor.ToString() && c.CompanyID == _CompanyID);
            RoleSelect = roleManager.Roles.Any(c => c.Name == SuperVisor && c.CompanyID == _CompanyID);
            if (RoleSelect == false)
            {
                role = new ApplicationRole
                {
                    Name = SuperVisor,//enumRoles.SuperVisor.ToString(),
                    CompanyID = _CompanyID,
                    Description = string.Empty,
                    bActive = 1,
                    isDefault = 1,
                    isDeleted = 0
                };
                await roleManager.CreateAsync(role);// new IdentityRole(enumRoles.SuperVisor.ToString()));
            }

            //////
            string UserRole = enumRoles.User.ToString() + "^" + _CompanyNameLogin;
            //RoleSelect = roleManager.Roles.Any(c => c.Name == enumRoles.User.ToString() && c.CompanyID == _CompanyID);
            RoleSelect = roleManager.Roles.Any(c => c.Name == UserRole && c.CompanyID == _CompanyID);
            if (RoleSelect == false)
            {
                role = new ApplicationRole
                {
                    Name = UserRole,//enumRoles.User.ToString(),
                    CompanyID = _CompanyID,
                    Description = string.Empty,
                    bActive = 1,
                    isDefault = 1,
                    isDeleted = 0
                };
                await roleManager.CreateAsync(role);// new IdentityRole(enumRoles.User.ToString()));
            }

            //RoleSelect = roleManager.Roles.Any(c => c.Name == enumRoles.Basic.ToString() && c.CompanyID == _CompanyID);
            //if (RoleSelect == false)
            //{
            //    role = new ApplicationRole
            //    {
            //        Name = enumRoles.Basic.ToString(),
            //        CompanyID = _CompanyID,
            //        Description = string.Empty,
            //        bActive = 1,
            //        isDefault = 1,
            //        isDeleted = 0
            //    };
            //    await roleManager.CreateAsync(role);// new IdentityRole(enumRoles.Basic.ToString()));
            //}
        }
    }
}
