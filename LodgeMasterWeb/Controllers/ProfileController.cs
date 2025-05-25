using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace LodgeMasterWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _empImagePath;


        public ProfileController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> UserManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = UserManager;
            _signInManager = signInManager;

            _webHostEnvironment = webHostEnvironment;
            _empImagePath = _webHostEnvironment.WebRootPath;

        }
        public IActionResult Profile()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();

            var _CompanyID = HttpContext.Session.GetString("CompanyID");
            var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
            var _UserID = HttpContext.Session.GetString("UserID");

            var EmpPhoto = "../../Images/avatar/bg-man.jpg";

            var emp = _userManager.GetUserAsync(User).Result;
            //.FirstOrDefault(x => x.CompanyID == _CompanyID && x.EmpID == _);
            if (emp != null)
            {
                if (emp.bPhoto != null || emp.bPhoto != "")
                {
                    //EmpPhoto = Path.Combine(_empImagePath, "Companies", emp.Photopath, "images", "employee", emp.bPhoto);
                    EmpPhoto = $"../../Companies/{emp.Photopath}/images/employee/{emp.bPhoto}";
                }

                ProfileViewModel model = new ProfileViewModel
                {
                    EmpID = _UserID,
                    FirstName = emp.FirstName,
                    LastName = emp.LastName,
                    mobile = emp.PhoneNumber,
                    //sEmail = emp.Email,
                    bPhoto = EmpPhoto,
                };

                return View(model);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public async Task<IActionResult> SaveProfile(string dataObj, IFormFile physImage)
        {
            try
            {


                ProfileViewModel dataOk = JsonConvert.DeserializeObject<ProfileViewModel>(dataObj);

                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                if (string.IsNullOrEmpty(dataOk.EmpID) == true)
                {
                    return Json(new { success = false, returnData = "Error saved" });
                }

                if (string.IsNullOrEmpty(dataOk.FirstName) == true)
                {
                    return Json(new { success = false, returnData = "enter first name" });
                }

                if (string.IsNullOrEmpty(dataOk.LastName) == true)
                {
                    return Json(new { success = false, returnData = "enter last name" });
                }

                // التحقق من أن رقم الهاتف غير مكرر
                
                var isFound = _context.Users.AsNoTracking().Any(x => x.PhoneNumber == dataOk.mobile && x.Id != dataOk.EmpID);
                if (isFound)
                {
                    return Json(new { success = false, returnData = "Phone number already exists." });
                }

                var userUpdate = await _userManager.FindByIdAsync(_UserID);

                if (userUpdate != null)
                {

                    if (physImage != null && physImage.Length > 0)
                    {
                        // Save the file to the server or perform other actions
                        string filePathDelete = "";
                        string PrevisFilename = userUpdate.bPhoto ?? "";

                        if (!string.IsNullOrEmpty(PrevisFilename))
                        {
                            filePathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "employee", PrevisFilename);
                            if (System.IO.File.Exists(filePathDelete))
                            {
                                System.IO.File.Delete(filePathDelete);
                            }
                        }
                        var fileExtension = Path.GetExtension(physImage.FileName);
                        var fileName = Guid.NewGuid().ToString() + fileExtension;
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Companies", _CompanyFolder, "images", "employee", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await physImage.CopyToAsync(stream);
                            userUpdate.bPhoto = fileName;
                        }
                    }

                    //////////////////////////////
                    userUpdate.FirstName = dataOk.FirstName;
                    userUpdate.LastName = dataOk.LastName;
                    userUpdate.PhoneNumber = dataOk.mobile;
                    //userUpdate.Email = dataOk.sEmail;


                    await _userManager.UpdateAsync(userUpdate);

                    var resJson = new { success = true, returnData = "Saved" };
                    return Json(resJson);
                }
                else
                {

                    var resJson = new { success = false, returnData = "Error" };
                    return Json(resJson);
                }
            }
            catch (Exception ex)
            {


                var resJson = new { success = false, returnData = ex.Message };
                return Json(resJson);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(string dataObj)
        {
            try
            {
                var _CompanyID = HttpContext.Session.GetString("CompanyID");
                var _CompanyFolder = HttpContext.Session.GetString("CompanyFolder");
                var _UserID = HttpContext.Session.GetString("UserID");

                ProfileViewModel dataOk = JsonConvert.DeserializeObject<ProfileViewModel>(dataObj);

                if (String.IsNullOrEmpty(dataOk.CurrentPassword))
                {

                    return Json(new { success = false, typeError = 1, message = "Please enter currnt password" });
                }

                if (String.IsNullOrEmpty(dataOk.NewPassword))
                {
                    return Json(new { success = false, typeError = 2, message = "Please enter new password" });
                }

                if (String.IsNullOrEmpty(dataOk.ConfirmPassword))
                {
                    return Json(new { success = false, typeError = 3, message = "Please enter confirm password" });
                }

                //compare
                //if (String.IsNullOrEmpty(dataOk.CurrentPassword))
                //{
                //    return Json(new { success = false, typeError = 0, message = "تم الحفظ بنجاح" });
                //}

                //chec currnt pwd
                var userData = await _userManager.GetUserAsync(User);

                if (userData == null)
                {
                    return Json(new { success = false, typeError = 1, message = "Error in user passowrd" });
                }


                string ErrorUpdatePassword = "";

                var changePasswordResult = await _userManager.ChangePasswordAsync(userData, dataOk.CurrentPassword, dataOk.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        //ModelState.AddModelError(string.Empty, error.Description);
                        ErrorUpdatePassword += "-" + error.Description + "\n";
                    }
                    return Json(new { success = false, typeError = 2, message = ErrorUpdatePassword });
                }

                await _signInManager.RefreshSignInAsync(userData);

                return Json(new { success = true, typeError = 0, message = "Success change password" });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, typeError = 1, message = ex.Message });
            }

        }
    }
}
