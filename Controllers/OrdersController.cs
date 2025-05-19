using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

[Authorize]
public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;
    public OrdersController(ApplicationDbContext context) { _context = context; }

    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var orders = _context.Orders.Where(o => o.UserId == userId).ToList();
        return View(orders);
    }

    public IActionResult Details(int id)
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var order = _context.Orders.FirstOrDefault(o => o.Id == id && o.UserId == userId);
        if (order == null) return NotFound();
        return View(order);
    }

    [HttpPost]
    public IActionResult Checkout()
    {
        var userId = int.Parse(User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier));
        var cartItems = _context.CartItems.Where(ci => ci.UserId == userId).ToList();
        if (!cartItems.Any()) return RedirectToAction("Index", "Cart");

        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            Status = "Pending",
            TotalPrice = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product.Price
            }).ToList()
        };
        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        _context.SaveChanges();

        return RedirectToAction("Details", new { id = order.Id });
    }
}