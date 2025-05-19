using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

[Authorize(Roles = "Admin")]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;
    public CategoriesController(ApplicationDbContext context) { _context = context; }

    public IActionResult Index()
    {
        return View(_context.Categories.ToList());
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();
        return View(category);
    }

    [HttpPost]
    public IActionResult Edit(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return NotFound();
        _context.Categories.Remove(category);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}