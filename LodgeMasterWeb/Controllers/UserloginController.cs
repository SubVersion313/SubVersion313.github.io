namespace LodgeMasterWeb.Controllers
{
    public class UserloginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string _CompanyID = string.Empty;
        private string _CompanyFolder = string.Empty;

        public UserloginController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            // return RedirectToAction("Splash", "Splash");
            if (!ModelState.IsValid)
                return View(nameof(Index), model);

            var company = _context.Companies.Where(c => c.Companylogin == model.Organization).ToList();
            if (company == null)
            {
                return View(nameof(Index), model);
            }
            else
            {
                foreach (var c in company)
                {
                    _CompanyID = c.CompanyID;
                    _CompanyFolder = c.CompanyFolder;

                    HttpContext.Session.SetString("CompanyID", _CompanyID);
                    HttpContext.Session.SetString("CompanyFolder", _CompanyFolder);

                }
                if (string.IsNullOrEmpty(_CompanyID))
                    return View(nameof(Index), model);

                var UserInfo = _context.Employees
                    .Any(c => c.CompanyID == _CompanyID
                    && c.UserLogin == model.Loginemail
                    && c.UserPassword == model.Password
                    );
                if (UserInfo == true)
                {
                    if (_context != null && _CompanyID != null && model != null && model.Loginemail != null && model.Password != null)
                    {
                        var UserData = _context.Employees
                                    .FirstOrDefault(c => c.CompanyID == _CompanyID && c.UserLogin == model.Loginemail && c.UserPassword == model.Password);
                        //.Select(c => new
                        //{
                        //    UserNameF = c.sFullName
                        //}).ToList();

                        //var c = UserData.ToQueryString();
                        //foreach (var dataInfo in UserData)
                        //{
                        //    HttpContext.Session.SetString("UserName", dataInfo.UserNameF);
                        //}
                        if (UserData != null)
                        {

                            HttpContext.Session.SetString("UserID", UserData.EmpID);
                            HttpContext.Session.SetString("UserFristName", UserData.FirstName);
                            HttpContext.Session.SetString("UserLastName", UserData.LastName);
                            HttpContext.Session.SetString("UserFullName", UserData.FirstName + " " + UserData.LastName);

                        }
                    }
                    return RedirectToAction("Splash", "Splash");
                }
                else
                {

                    return View(nameof(Index), model);
                }

            }

        }
    }
}
