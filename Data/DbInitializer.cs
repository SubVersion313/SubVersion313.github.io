public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
        if (!context.Users.Any(u => u.Email == "admin@site.com"))
        {
            context.Users.Add(new User
            {
                NameUser = "Admin",
                Email = "admin@site.com",
                PasswordUser = PasswordHasher.Hash("adminpass"),
                IsAdmin = true,
                CreatedAt = DateTime.Now
            });
        }
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "homeDecor", Description = "Home Decor" },
                new Category { Name = "artCollectibles", Description = "Art & Collectibles" },
                new Category { Name = "accessories", Description = "Accessories" }
            );
        }
        context.SaveChanges();
    }
}