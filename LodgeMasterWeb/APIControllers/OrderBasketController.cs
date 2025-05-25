namespace LodgeMasterWeb.APIControllers
{
    public class OrderBasketController : Controller
    {
        // GET: OrderBasketController
        public ActionResult Index()
        {
            return View();
        }

        // GET: OrderBasketController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderBasketController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderBasketController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderBasketController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderBasketController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderBasketController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderBasketController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
