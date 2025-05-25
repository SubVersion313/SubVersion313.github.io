// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LodgeMasterWeb.Areas.Identity.Pages.Account
{
    [AllowAnonymous]

    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> UserManager, ApplicationDbContext context, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = UserManager;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                
                //_logger.LogInformation($"Login attempt result: {result.Succeeded}, RequiresTwoFactor: {result.RequiresTwoFactor}, IsLockedOut: {result.IsLockedOut}");
                //_logger.LogError("An error occurred during login attempt for user: {Email}", "azizmss@gmail.com");
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    //var Userchek=await _userManager.GetUserAsync(User);
                    //_logger.LogInformation($"User ID: {Userchek.Id}, Email: {Userchek.Email}");
                    //if (Userchek != null && Userchek.bActive == 1 && Userchek.IsDeleted==0)
                    //{
                        // var resultA = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, resultA.Principal);

                        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ExternalLogins[0].DisplayName);

                        //if (User.Identity.IsAuthenticated == true) {
                        //await SetUserInfoAsync()

                        //var StatusUserIdentity=await SetUserInfoAsync();

                        //if (StatusUserIdentity == true)
                        // {
                        // return LocalRedirect(returnUrl + "Splash/Splash");
                        return RedirectToAction("Splash", "Splash");
                        //}
                        //else
                        //{
                        //    return Page();
                        //}
                   // }
    
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {

                    
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    //var msg = ($"Login attempt result: {result.Succeeded}, RequiresTwoFactor: {result.RequiresTwoFactor}, IsLockedOut: {result.IsLockedOut}");
                    //ModelState.AddModelError(string.Empty, msg);
                   ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
        public async Task<bool> SetUserInfoAsync()
        {
            //string wherepoint = "";
            try
            {

                bool Result = false;
                if (User.Identity.IsAuthenticated == true)
                {
                    // var UserInfo = await _userManager.GetUserAsync(User);
                    //wherepoint = "IsAuthenticated| ";

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
                        HttpContext.Session.SetString("CompanyID", _CompanyID);
                        HttpContext.Session.SetString("CompanyFolder", _CompanyFolder);
                        HttpContext.Session.SetString("UserID", UserInfo.Id);
                        HttpContext.Session.SetString("UserFristName", UserInfo.FirstName);
                        HttpContext.Session.SetString("UserLastName", UserInfo.LastName);
                        HttpContext.Session.SetString("UserFullName", UserInfo.FirstName + " " + UserInfo.LastName);
                        HttpContext.Session.SetString("CompanyNameLogin", company.CompanyNameLogin);

                        //wherepoint += "UserID: " + UserInfo.Id ;
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
                // Console.WriteLine("Error");
                return false;
            }
        }
    }
}
