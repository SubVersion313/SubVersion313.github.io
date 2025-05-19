using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;
    public AdminController(ApplicationDbContext context) { _context = context; }

    public IActionResult Dashboard()
    {
        var users = _context.Users.ToList();
        var products = _context.Products.ToList();
        var orders = _context.Orders.ToList();
        return View(new AdminDashboardViewModel
        {
            Users = users,
            Products = products,
            Orders = orders
        });
    }
}

public class AdminDashboardViewModel
{
    public List<User> Users { get; set; }
    public List<Product> Products { get; set; }
    public List<Order> Orders { get; set; }
}