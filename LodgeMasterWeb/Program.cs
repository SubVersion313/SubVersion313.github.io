using LodgeMasterWeb.Filters;
using LodgeMasterWeb.Hubs;
using LodgeMasterWeb.Seeds;
using LodgeMasterWeb.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddHttpClient();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
builder.Host.UseSerilog((context, configration) =>

        configration.ReadFrom.Configuration(context.Configuration)
    //configration
    //.MinimumLevel.Warning()
    //.WriteTo.Console();
    //// .WriteTo.File(context.Configuration["Serilog:FilePath"], rollingInterval: RollingInterval.Day);
    );


var connectionString = builder.Configuration.GetConnectionString("SQLConn");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<ApplicationUser>()
//    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultUI()
//    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

});

//builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
// .AddEntityFrameworkStores<ApplicationDbContext>()
// .AddDefaultUI()
// .AddDefaultTokenProviders();

//  ”ÃÌ· Œœ„«  Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ≈÷«›… «·Œœ„«  ≈·Ï «·Õ«ÊÌ…
builder.Services.AddTransient<PermissionService>(); //  ”ÃÌ· «·Œœ„… Â‰«

//ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.Configure<SecurityStampValidatorOptions>(Option =>
    {
        Option.ValidationInterval = TimeSpan.Zero;
    });

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WhatsappOrder>();
builder.Services.AddControllersWithViews();
//builder.Services.AddControllers();

builder.Services.AddSession(options =>
{
    //Set whatever options you want here
    options.Cookie.IsEssential = true;
    //options.IdleTimeout = TimeSpan.FromDays(365);
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});
builder.Services.AddDistributedMemoryCache();

builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeAreaPage("Identity", "/Account/ForgotPassword");
});

//builder.Services.AddAntiforgery(options => {
//    options.HeaderName = "X-CSRF-TOKEN"; // √Ê «·«”„ «·–Ì  —ÌœÂ
//});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login";
//        options.LogoutPath = "/Splash/SignOut";
//    });

//builder.Services.AddAuthentication();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow4Whats",
        builder =>
        {
            builder.WithOrigins("https://4whats.net")
            .WithOrigins("https://user.4whats.net/api/")
            .WithOrigins("https://user.4whats.net/api/sendMessage")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// ›Ì ConfigureServices
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // “„‰ «‰ Â«¡ «·Ã·”…
    options.LoginPath = "/Account/Login"; // „”«— ’›Õ…  ”ÃÌ· «·œŒÊ·
    options.AccessDeniedPath = "/Account/AccessDenied"; // „”«— ’›Õ… —›÷ «·Ê’Ê·
    options.SlidingExpiration = true; //  ÕœÌÀ «·Ã·”… „⁄ ﬂ· ÿ·»
});

builder.Services.AddSignalR();

var app = builder.Build();

//Aziz

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
var logger = loggerFactory.CreateLogger("app");

try
{
    ////ApplicationDbContext _context;
    //var _context = services.GetRequiredService<ApplicationDbContext>();

    //var userManger = services.GetRequiredService<UserManager<ApplicationUser>>();
    ////var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();
    //var roleManger = services.GetRequiredService<RoleManager<ApplicationRole>>();

    //var _CompanyId = "6bbf80de-8d72-4884-8860-fc518d09bf5c";// await defaultCompany.SeedCompanyAndBrancheAsync(_context);
    //var _CompanyNameLogin = "LodgeMaster";
    //await defaultRoles.SeedAsync(roleManger, _CompanyId, _CompanyNameLogin);
    ////await defaultUsers.SeedBasicUserAsync(userManger, _CompanyId);
    //await defaultUsers.SeedAdminUserAsync(userManger, roleManger, _CompanyId, _CompanyNameLogin);

    logger.LogInformation("Data seeded");
    logger.LogInformation("Application Started");
}
catch (Exception ex)
{

    logger.LogWarning(ex, "Error when start seeds");
}
//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("Allow4Whats");
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    // Map Webhook without requiring authentication
    endpoints.MapControllerRoute(
        name: "webhook",
        pattern: "api/webhook/{action=ReceiveMessage}",
        defaults: new { controller = "Webhook" })
        .AllowAnonymous(); // Explicitly allow anonymous access to the Webhook

    // Map MVC Controllers
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    // Map Razor Pages
    endpoints.MapRazorPages();

    // Map API Controllers
    endpoints.MapControllers(); // This will map attribute-routed API controllers

});

app.MapHub<OrderHub>("/Hubs/SendMessage");
app.MapControllers();

app.Run();
