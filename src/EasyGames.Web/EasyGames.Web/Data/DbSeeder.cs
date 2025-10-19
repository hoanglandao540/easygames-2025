using EasyGames.Web.Models;
using EasyGames.Web.Services;
using System.Collections.Generic;
using System.Linq;

namespace EasyGames.Web.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db)
        {
            // --- Products with Categories ---
            if (!db.Products.Any())
            {
                var products = new List<Product>
                {
                    // Accessories
                    new Product { Code = "EG-001", Name = "Game Pad", Price = 29.99m, Category = "Accessories" },
                    new Product { Code = "EG-002", Name = "Arcade Token Pack (50)", Price = 9.99m, Category = "Accessories" },
                    new Product { Code = "EG-003", Name = "Gift Card $50", Price = 49.00m, Category = "Accessories" },
                    
                    // Games
                    new Product { Code = "EG-004", Name = "Classic Board Game", Price = 39.99m, Category = "Games" },
                    new Product { Code = "EG-016", Name = "Family Card Game", Price = 19.99m, Category = "Games" },
                    new Product { Code = "EG-017", Name = "Strategy Board: Conquest", Price = 54.99m, Category = "Games" },
                    new Product { Code = "EG-018", Name = "Co-op Adventure Game", Price = 44.99m, Category = "Games" },
                    new Product { Code = "EG-019", Name = "Trading Card Booster Pack", Price = 4.99m, Category = "Games" },
                    new Product { Code = "EG-020", Name = "Dice Set (12)", Price = 6.99m, Category = "Games" },
                    new Product { Code = "EG-028", Name = "Board Game Expansion Pack", Price = 24.99m, Category = "Games" },
                    new Product { Code = "EG-033", Name = "Magnetic Travel Game", Price = 12.50m, Category = "Games" },

                    // Toys
                    new Product { Code = "EG-005", Name = "Puzzle Set 500pc", Price = 19.99m, Category = "Toys" },
                    new Product { Code = "EG-011", Name = "Remote Car", Price = 49.99m, Category = "Toys" },
                    new Product { Code = "EG-012", Name = "Plush Dino", Price = 15.99m, Category = "Toys" },
                    new Product { Code = "EG-013", Name = "Building Blocks - 300pc", Price = 34.99m, Category = "Toys" },
                    new Product { Code = "EG-014", Name = "Action Figure Set", Price = 27.50m, Category = "Toys" },
                    new Product { Code = "EG-015", Name = "Wooden Train Set", Price = 59.99m, Category = "Toys" },
                    new Product { Code = "EG-029", Name = "Outdoor Lawn Game", Price = 79.99m, Category = "Toys" },
                    new Product { Code = "EG-032", Name = "Kids Art Set", Price = 16.99m, Category = "Toys" },
                    new Product { Code = "EG-034", Name = "STEM Kit: Robotics", Price = 89.99m, Category = "Toys" },
                    new Product { Code = "EG-040", Name = "Kids Puzzle 1000pc", Price = 24.99m, Category = "Toys" },

                    // Books
                    new Product { Code = "EG-006", Name = "Kids Storybook: Outback Adventures", Price = 12.99m, Category = "Books" },
                    new Product { Code = "EG-007", Name = "Beginner's Guide: Tabletop RPG", Price = 24.50m, Category = "Books" },
                    new Product { Code = "EG-008", Name = "Strategy Guide: Multiplayer Games", Price = 18.00m, Category = "Books" },
                    new Product { Code = "EG-009", Name = "Comics Bundle", Price = 14.99m, Category = "Books" },
                    new Product { Code = "EG-010", Name = "Educational Book: Coding for Kids", Price = 22.00m, Category = "Books" },
                    new Product { Code = "EG-030", Name = "Strategy Journal", Price = 9.99m, Category = "Books" },
                    new Product { Code = "EG-035", Name = "Scratch-Off Poster: 100 Games", Price = 19.50m, Category = "Books" },
                    new Product { Code = "EG-039", Name = "Collector's Poster", Price = 6.99m, Category = "Books" },

                    // More Accessories
                    new Product { Code = "EG-021", Name = "Controller Charger", Price = 18.99m, Category = "Accessories" },
                    new Product { Code = "EG-022", Name = "Headset", Price = 39.99m, Category = "Accessories" },
                    new Product { Code = "EG-023", Name = "Console Skin", Price = 14.50m, Category = "Accessories" },
                    new Product { Code = "EG-024", Name = "Card Sleeves (100)", Price = 7.99m, Category = "Accessories" },
                    new Product { Code = "EG-025", Name = "Collector's Box", Price = 29.99m, Category = "Accessories" },
                    new Product { Code = "EG-026", Name = "Sticker Pack", Price = 3.99m, Category = "Accessories" },
                    new Product { Code = "EG-027", Name = "Mini Puzzle Keyring", Price = 4.50m, Category = "Accessories" },
                    new Product { Code = "EG-031", Name = "Token Tube (200)", Price = 29.99m, Category = "Accessories" },
                    new Product { Code = "EG-036", Name = "Replacement Joystick", Price = 11.99m, Category = "Accessories" },
                    new Product { Code = "EG-037", Name = "Rechargeable Batteries (4)", Price = 9.50m, Category = "Accessories" },
                    new Product { Code = "EG-038", Name = "Board Game Repair Kit", Price = 8.99m, Category = "Accessories" }
                };

                db.Products.AddRange(products);
                db.SaveChanges();
            }

            // --- Demo users ---
            if (!db.AppUsers.Any())
            {
                db.AppUsers.AddRange(
                    new AppUser { Name = "Owner One", Email = "owner@example.com", Phone = "", PasswordHash = Password.Hash("owner123"), Role = AppRole.Owner },
                    new AppUser { Name = "Head Clerk", Email = "clerk@example.com", Phone = "", PasswordHash = Password.Hash("clerk123"), Role = AppRole.Shop },

                    // Shop staff
                    new AppUser { Name = "Sarah Chen", Email = "sarah.chen@easygames.com.au", Phone = "0438-555-0101", PasswordHash = Password.Hash("shop123"), Role = AppRole.Shop },
                    new AppUser { Name = "James O'Connor", Email = "james.oconnor@easygames.com.au", Phone = "0438-555-0102", PasswordHash = Password.Hash("shop123"), Role = AppRole.Shop },
                    new AppUser { Name = "Priya Patel", Email = "priya.patel@easygames.com.au", Phone = "0438-555-0103", PasswordHash = Password.Hash("shop123"), Role = AppRole.Shop },

                    // Customers
                    new AppUser { Name = "Alice", Email = "alice@example.com", Phone = "+61400111222", PasswordHash = Password.Hash("alice123"), Role = AppRole.Customer },
                    new AppUser { Name = "Bob", Email = "bob@example.com", Phone = "+61400111223", PasswordHash = Password.Hash("bob123"), Role = AppRole.Customer }
                );
                db.SaveChanges();
            }

            // --- Multiple shops in Darwin area ---
            if (!db.Shops.Any())
            {
                var clerkUser = db.AppUsers.FirstOrDefault(u => u.Email == "clerk@example.com");

                db.Shops.AddRange(
                    new ShopLocation
                    {
                        ShopCode = "DRW-01",
                        City = "Darwin",
                        Country = "AU",
                        Phone = "+61-8-0000-0000",
                        ProprietorUserId = clerkUser?.Id
                    },
                    new ShopLocation
                    {
                        ShopCode = "DRW-02",
                        City = "Casuarina",
                        Country = "AU",
                        Phone = "+61-8-0000-0001",
                        ProprietorUserId = clerkUser?.Id
                    },
                    new ShopLocation
                    {
                        ShopCode = "PAL-01",
                        City = "Palmerston",
                        Country = "AU",
                        Phone = "+61-8-0000-0002",
                        ProprietorUserId = clerkUser?.Id
                    },
                    new ShopLocation
                    {
                        ShopCode = "ALC-01",
                        City = "Alice Springs",
                        Country = "AU",
                        Phone = "+61-8-0000-0003",
                        ProprietorUserId = clerkUser?.Id
                    },
                    new ShopLocation
                    {
                        ShopCode = "MKT-01",
                        City = "Darwin Mall",
                        Country = "AU",
                        Phone = "+61-8-0000-0004",
                        ProprietorUserId = clerkUser?.Id
                    }
                );
                db.SaveChanges();
            }

            // --- Owner inventory (bulk) ---
            if (!db.OwnerStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();
                if (p.Count >= 40)
                {
                    var ownerStocks = new List<OwnerStock>
                    {
                        new OwnerStock { ProductId = p[0].Id, Qty = 500, Source = "HQ Shipment", BuyPrice = 20.00m, SellPrice = 29.99m },
                        new OwnerStock { ProductId = p[1].Id, Qty = 1000, Source = "Token Vendor", BuyPrice = 5.00m, SellPrice = 9.99m },
                        new OwnerStock { ProductId = p[2].Id, Qty = 300, Source = "Gift Supplier", BuyPrice = 35.00m, SellPrice = 49.00m }
                    };

                    // Add remaining SKUs with large quantities
                    for (int i = 3; i < p.Count; i++)
                    {
                        ownerStocks.Add(new OwnerStock
                        {
                            ProductId = p[i].Id,
                            Qty = 200 + (i * 10),
                            Source = "Central Warehouse",
                            BuyPrice = decimal.Round(p[i].Price * 0.6m, 2),
                            SellPrice = p[i].Price
                        });
                    }

                    db.OwnerStocks.AddRange(ownerStocks);
                    db.SaveChanges();
                }
            }

            // --- Shop stocks (initial loads) ---
            var shops = db.Shops.ToList();
            if (shops.Any() && !db.ShopStocks.Any())
            {
                var p = db.Products.OrderBy(x => x.Id).ToList();

                foreach (var shop in shops)
                {
                    var shopStockList = new List<ShopStock>();

                    for (int i = 0; i < p.Count; i++)
                    {
                        var baseQty = 0;
                        if (i < 5) baseQty = 80;
                        else if (i < 15) baseQty = 40;
                        else if (i < 30) baseQty = 20;
                        else baseQty = 10;

                        var qty = baseQty + (shop.Id % 5) * 5;

                        shopStockList.Add(new ShopStock
                        {
                            ShopId = shop.Id,
                            ProductId = p[i].Id,
                            Qty = qty,
                            ReorderLevel = Math.Max(3, qty / 10),
                            Source = "Initial Load",
                            BuyPrice = decimal.Round(p[i].Price * 0.6m, 2),
                            SellPrice = p[i].Price
                        });
                    }

                    db.ShopStocks.AddRange(shopStockList);
                }

                db.SaveChanges();
            }
        }
    }
}