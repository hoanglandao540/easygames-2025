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
                    new Product { Code = "EG-003", Name = "Gift Card", Price = 49.00m },
                    new Product { Code = "EG-004", Name = "Board Game", Price = 39.99m },
                    new Product { Code = "EG-005", Name = "Puzzle Set", Price = 19.99m }
                );
                db.SaveChanges();
            }

            // --- Demo users ---
            if (!db.AppUsers.Any())
            {
                db.AppUsers.AddRange(
                    new AppUser { Name = "Owner One", Email = "owner@example.com", Phone = "", PasswordHash = Password.Hash("owner123"), Role = AppRole.Owner },
                    new AppUser { Name = "Shop Clerk", Email = "clerk@example.com", Phone = "", PasswordHash = Password.Hash("clerk123"), Role = AppRole.Shop },
                    new AppUser { Name = "Alice", Email = "alice@example.com", Phone = "+1234567890", PasswordHash = Password.Hash("alice123"), Role = AppRole.Customer },
                    new AppUser { Name = "Bob", Email = "bob@example.com", Phone = "+1234567891", PasswordHash = Password.Hash("bob123"), Role = AppRole.Customer }
                );
                db.SaveChanges();
            }

            // --- One demo shop ---
            if (!db.Shops.Any())
            {
                var clerkUser = db.AppUsers.FirstOrDefault(u => u.Email == "clerk@example.com");
                db.Shops.Add(new ShopLocation  // ← CHANGED
                {
                    ShopCode = "DRW-01",
                    City = "Darwin",
                    Country = "AU",
                    Phone = "+61-8-0000-0000",
                    ProprietorUserId = clerkUser?.Id
                });
                db.SaveChanges();
            }

            // --- Owner inventory ---
            if (!db.OwnerStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                if (p.Count >= 5)
                {
                    db.OwnerStocks.AddRange(
                        new OwnerStock { ProductId = p[0].Id, Qty = 50, Source = "HQ Shipment", BuyPrice = 20.00m, SellPrice = 29.99m },
                        new OwnerStock { ProductId = p[1].Id, Qty = 100, Source = "Token Vendor", BuyPrice = 5.00m, SellPrice = 9.99m },
                        new OwnerStock { ProductId = p[2].Id, Qty = 30, Source = "Gift Supplier", BuyPrice = 35.00m, SellPrice = 49.00m },
                        new OwnerStock { ProductId = p[3].Id, Qty = 40, Source = "Game Distributor", BuyPrice = 25.00m, SellPrice = 39.99m },
                        new OwnerStock { ProductId = p[4].Id, Qty = 60, Source = "Toy Warehouse", BuyPrice = 12.00m, SellPrice = 19.99m }
                    );
                    db.SaveChanges();
                }
            }

            // --- Shop stock ---
            var shop = db.Shops.FirstOrDefault();
            if (shop != null && !db.ShopStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                if (p.Count >= 5)
                {
                    db.ShopStocks.AddRange(
                        new ShopStock { ShopId = shop.Id, ProductId = p[0].Id, Qty = 10, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 20.00m, SellPrice = 29.99m },
                        new ShopStock { ShopId = shop.Id, ProductId = p[1].Id, Qty = 8, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 5.00m, SellPrice = 9.99m },
                        new ShopStock { ShopId = shop.Id, ProductId = p[2].Id, Qty = 5, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 35.00m, SellPrice = 49.00m },
                        new ShopStock { ShopId = shop.Id, ProductId = p[3].Id, Qty = 7, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 25.00m, SellPrice = 39.99m },
                        new ShopStock { ShopId = shop.Id, ProductId = p[4].Id, Qty = 12, ReorderLevel = 3, Source = "Initial Load", BuyPrice = 12.00m, SellPrice = 19.99m }
                    );
                    db.SaveChanges();
                }
            }
        }
    }
}