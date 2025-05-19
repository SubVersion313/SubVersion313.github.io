using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    public CartController(ApplicationDbContext context) { _context = context; }

    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var cart = _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Select(ci => new { ci, ci.Product })
            .ToList();
        return View(cart);
    }

    [HttpPost]
    public IActionResult Add(int productId, int quantity)
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var existing = _context.CartItems.FirstOrDefault(ci => ci.ProductId == productId && ci.UserId == userId);
        if (existing != null)
            existing.Quantity += quantity;
        else
            _context.CartItems.Add(new CartItem { ProductId = productId, UserId = userId, Quantity = quantity });
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int id)
    {
        var item = _context.CartItems.FirstOrDefault(ci => ci.Id == id);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Clear()
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var items = _context.CartItems.Where(ci => ci.UserId == userId);
        _context.CartItems.RemoveRange(items);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}