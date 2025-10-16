using EasyGames.Web.Models;
using EasyGames.Web.Services;

namespace EasyGames.Web.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // --- Products ---
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Code = "EG-001", Name = "Game Pad", Price = 29.99m },
                    new Product { Code = "EG-002", Name = "Arcade Token", Price = 9.99m },
                    new Product { Code = "EG-003", Name = "Gift Card", Price = 49.00m }
                );
                db.SaveChanges();
            }

            // --- One demo shop ---
            if (!db.Shops.Any())
            {
                db.Shops.Add(new Shop { ShopCode = "DRW-01", City = "Darwin", Country = "AU", Phone = "+61-8-0000-0000" });
                db.SaveChanges();
            }

            // --- Owner "warehouse" inventory (★ what Transfer page needs) ---
            if (!db.OwnerStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                db.OwnerStocks.AddRange(
                    new OwnerStock { ProductId = p[0].Id, Qty = 50, Source = "HQ Shipment", BuyPrice = 20.00m, SellPrice = 29.99m },
                    new OwnerStock { ProductId = p[1].Id, Qty = 100, Source = "Token Vendor", BuyPrice = 5.00m, SellPrice = 9.99m },
                    new OwnerStock { ProductId = p[2].Id, Qty = 30, Source = "Gift Supplier", BuyPrice = 35.00m, SellPrice = 49.00m }
                );
                db.SaveChanges();
            }

            // --- Initial shop on-hand (optional) ---
            var shopId = db.Shops.First().Id;
            if (!db.ShopStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                db.ShopStocks.AddRange(
                    new ShopStock { ShopId = shopId, ProductId = p[0].Id, Qty = 10, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 20.00m, SellPrice = 29.99m },
                    new ShopStock { ShopId = shopId, ProductId = p[1].Id, Qty = 8, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 5.00m, SellPrice = 9.99m },
                    new ShopStock { ShopId = shopId, ProductId = p[2].Id, Qty = 5, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 35.00m, SellPrice = 49.00m }
                );
                db.SaveChanges();
            }

            // --- Demo users: Owner / Shop / Customer ---
            if (!db.AppUsers.Any())
            {
                db.AppUsers.AddRange(
                    new AppUser { Name = "Owner One", Email = "owner@example.com", PasswordHash = Password.Hash("owner123"), Role = AppRole.Owner },
                    new AppUser { Name = "Shop Clerk", Email = "clerk@example.com", PasswordHash = Password.Hash("clerk123"), Role = AppRole.Shop },
                    new AppUser { Name = "Alice", Email = "alice@example.com", PasswordHash = Password.Hash("alice123"), Role = AppRole.Customer }
                );
                db.SaveChanges();
            }
        }
    }
}


