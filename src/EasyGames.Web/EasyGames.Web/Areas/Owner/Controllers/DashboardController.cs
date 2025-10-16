using EasyGames.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EasyGames.Web.Models;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var model = new OwnerDashVM
            {
                ProductCount = _db.Products.Count(),
                ShopCount = _db.Shops.Count(),
                UserCount = _db.AppUsers.Count(),
                TotalStockQty = _db.ShopStocks.Sum(s => (int?)s.Qty) ?? 0,
                LowStockItems = _db.ShopStocks.Where(s => s.Qty <= s.ReorderLevel).Count()
            };
            return View(model);
        }
    }

    public class OwnerDashVM
    {
        public int ProductCount { get; set; }
        public int ShopCount { get; set; }
        public int UserCount { get; set; }
        public int TotalStockQty { get; set; }
        public int LowStockItems { get; set; }
    }
}
