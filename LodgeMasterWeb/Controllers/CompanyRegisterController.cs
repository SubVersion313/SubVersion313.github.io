namespace LodgeMasterWeb.Controllers;
public class CompanyRegisterController : Controller
{
    public IActionResult CompanyRegister()
    {
        return View();
    }
    [HttpPost]
    public IActionResult CompanyRegister(CompanyRegisterViewModel model)
    {

        if (ModelState.IsValid)
        {

        }
        return View();
    }
}
