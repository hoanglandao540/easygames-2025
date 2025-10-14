using EasyGames.Web.Models;

namespace EasyGames.Web.Data
{
    // student-style: insert sample rows if DB is empty
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Code = "EG-001", Name = "Game Pad", Price = 29.99m },
                    new Product { Code = "EG-002", Name = "Arcade Token", Price = 9.99m },
                    new Product { Code = "EG-003", Name = "Gift Card", Price = 49.00m }
                );
                db.SaveChanges();
            }

            if (!db.Shops.Any())
            {
                db.Shops.Add(new Shop { ShopCode = "DRW-01", City = "Darwin", Country = "AU", Phone = "+61-8-0000-0000" });
                db.SaveChanges();
            }

            var shopId = db.Shops.First().Id;
            if (!db.ShopStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                db.ShopStocks.AddRange(
                    new ShopStock { ShopId = shopId, ProductId = p[0].Id, Qty = 10, ReorderLevel = 3 },
                    new ShopStock { ShopId = shopId, ProductId = p[1].Id, Qty = 8, ReorderLevel = 3 },
                    new ShopStock { ShopId = shopId, ProductId = p[2].Id, Qty = 5, ReorderLevel = 3 }
                );
                db.SaveChanges();
            }

            if (!db.Customers.Any())
            {
                db.Customers.AddRange(
                    new Customer { Name = "Alice", Email = "alice@example.com" },
                    new Customer { Name = "Bob", Email = "bob@example.com" }
                );
                db.SaveChanges();
            }
        }
    }
}
