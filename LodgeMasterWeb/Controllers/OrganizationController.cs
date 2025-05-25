namespace LodgeMasterWeb.Controllers
{
    public class OrganizationController : Controller
    {
        public IActionResult Organization()
        {
            ViewBag.DisplayCurrntDate = GeneralFun.ShowDate();
            return View();
        }
    }
}
