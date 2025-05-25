using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LodgeMasterWeb.Services;

public class PermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public PermissionService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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

    public async Task<List<KeyValuePair<string, string>>> GetAllUserPermissions(ApplicationUser user)
    {
        // قائمة لتجميع جميع الصلاحيات
        var allPermissions = new List<KeyValuePair<string, string>>();

        // 1. جلب جميع الصلاحيات المباشرة المسندة للمستخدم
        var userClaims = await _userManager.GetClaimsAsync(user);
        allPermissions.AddRange(userClaims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)));

        // 2. جلب جميع الأدوار المرتبطة بالمستخدم
        var roles = await _userManager.GetRolesAsync(user);

        // 3. جلب الصلاحيات المرتبطة بكل دور
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                allPermissions.AddRange(roleClaims.Select(c => new KeyValuePair<string, string>(c.Type, c.Value)));
            }
        }

        return allPermissions.Distinct().ToList(); // إرجاع جميع الصلاحيات مع إزالة التكرارات
    }
}
