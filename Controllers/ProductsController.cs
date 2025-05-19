using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProductsController(ApplicationDbContext context) { _context = context; }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(_context.Products.ToList());
    }

    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }
        product.CreatedAt = DateTime.Now;
        _context.Products.Add(product);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Edit(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }
        _context.Products.Update(product);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}