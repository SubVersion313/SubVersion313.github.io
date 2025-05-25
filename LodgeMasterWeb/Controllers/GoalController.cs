using Microsoft.AspNetCore.Mvc;

namespace LodgeMasterWeb.Controllers;
public class GoalController : Controller
{
    public IActionResult Goalstaff()
    {
        return View();
    }
}
