namespace LodgeMasterWeb.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {

            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
    }
}
