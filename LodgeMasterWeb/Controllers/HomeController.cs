

using System.Configuration;

namespace LodgeMasterWeb.Controllers
{
    [AllowAnonymous]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public int TestErrorUnit(int a,int c )
        {

            return a * c ;
        }
    }
}