using System.Threading.Tasks;
using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EasyGames.Tests
{
    public class InventoryServiceTests
    {
        private static AppDbContext CreateDb(out SqliteConnection conn)
        {
            conn = new SqliteConnection("Filename=:memory:");
            conn.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(conn)
                .Options;

            var db = new AppDbContext(options);
            db.Database.EnsureCreated();

            // seed minimal data
            var p = new Product { Code = "EG-TEST", Name = "Test Item", Price = 9.99m };
<<<<<<< HEAD
            var s = new ShopLocation { ShopCode = "DRW-01", City = "Darwin", Country = "AU", Phone = "+61" };
=======
            var s = new Shop { ShopCode = "DRW-01", City = "Darwin", Country = "AU", Phone = "+61" };
>>>>>>> feature/akshata/data-shops
            db.Products.Add(p);
            db.Shops.Add(s);
            db.SaveChanges();
            db.ShopStocks.Add(new ShopStock { ShopId = s.Id, ProductId = p.Id, Qty = 5, ReorderLevel = 1 });
            db.SaveChanges();

            return db;
        }

        [Fact]
        public async Task IncreaseAsync_adds_quantity()
        {
            var db = CreateDb(out var conn);
            var svc = new InventoryService(db);

            var shop = await db.Shops.FirstAsync();
            var prod = await db.Products.FirstAsync();

            await svc.IncreaseAsync(shop.Id, prod.Id, 3);

            var stock = await db.ShopStocks.FirstAsync();
            stock.Qty.Should().Be(8);

            await conn.DisposeAsync();
        }

        [Fact]
        public async Task DecreaseAsync_subtracts_quantity()
        {
            var db = CreateDb(out var conn);
            var svc = new InventoryService(db);

            var shop = await db.Shops.FirstAsync();
            var prod = await db.Products.FirstAsync();

            await svc.DecreaseAsync(shop.Id, prod.Id, 2);

            var stock = await db.ShopStocks.FirstAsync();
            stock.Qty.Should().Be(3);

            await conn.DisposeAsync();
        }

        [Fact]
        public async Task DecreaseAsync_throws_if_negative()
        {
            var db = CreateDb(out var conn);
            var svc = new InventoryService(db);

            var shop = await db.Shops.FirstAsync();
            var prod = await db.Products.FirstAsync();

            var act = async () => await svc.DecreaseAsync(shop.Id, prod.Id, 10);
            await act.Should().ThrowAsync<System.InvalidOperationException>();

            var stock = await db.ShopStocks.FirstAsync();
            stock.Qty.Should().Be(5);

            await conn.DisposeAsync();
        }
    }
}
