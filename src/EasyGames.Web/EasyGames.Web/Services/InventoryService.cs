using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Services
{
    // student-style: safe inventory operations (never negative)
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _db;
        public InventoryService(AppDbContext db) => _db = db;

        public async Task IncreaseAsync(int shopId, int productId, int qty)
        {
            var row = await _db.ShopStocks
                .FirstOrDefaultAsync(x => x.ShopId == shopId && x.ProductId == productId);
            if (row == null)
            {
                row = new ShopStock { ShopId = shopId, ProductId = productId, Qty = 0, ReorderLevel = 3 };
                _db.ShopStocks.Add(row);
            }
            row.Qty += qty;
            await _db.SaveChangesAsync();
        }

        public async Task DecreaseAsync(int shopId, int productId, int qty)
        {
            var row = await _db.ShopStocks
                .FirstOrDefaultAsync(x => x.ShopId == shopId && x.ProductId == productId);

            if (row == null || row.Qty < qty)
                throw new InvalidOperationException("Not enough stock to decrease.");

            row.Qty -= qty;
            await _db.SaveChangesAsync();
        }

        public async Task<List<StockRowVM>> GetStockAsync(int shopId)
        {
            var query = from s in _db.ShopStocks
                        join p in _db.Products on s.ProductId equals p.Id
                        where s.ShopId == shopId
                        orderby p.Name
                        select new StockRowVM
                        {
                            ProductId = p.Id,
                            ProductName = p.Name,
                            Qty = s.Qty,
                            ReorderLevel = s.ReorderLevel
                        };

            return await query.ToListAsync();
        }
    }
}
