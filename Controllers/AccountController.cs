using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly ApplicationDbContext _context;

    public AccountController(ILogger<AccountController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login attempt for user {Email}", model.Email);
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !user.VerifyPassword(model.Password))
            {
                _logger.LogWarning("Failed login attempt for user {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Create authentication cookie
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.NameUser),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            _logger.LogInformation("User {Email} logged in successfully", user.Email);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            return View(model);
        }
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register() => View();

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration attempt for user {Email}", model.Email);
                return View(model);
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            var user = new User
            {
                NameUser = model.Name,
                Email = model.Email,
                Address = model.Address,
                CreatedAt = DateTime.UtcNow
            };

            user.SetPassword(model.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New user registered: {Email}", user.Email);
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user {Email}", model.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("User logged out");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return RedirectToAction("Index", "Home");
        }
    }

    [Authorize]
    public IActionResult MyAccount()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        return View(user);
    }
}