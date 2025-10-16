using EasyGames.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Shop.Controllers
{
    [Area("Shop")]
    [Authorize(Roles = "Shop")]
    public class StockController : Controller
    {
        private readonly AppDbContext _db;
        public StockController(AppDbContext db) { _db = db; }

        public IActionResult Index(int? shopId)
        {
            // simple: if not specified, show all; otherwise filter by shop
            var q = _db.ShopStocks
                .Include(s => s.Product)
                .Include(s => s.Shop)
                .AsNoTracking();

            if (shopId.HasValue) q = q.Where(s => s.ShopId == shopId.Value);

            var rows = q.OrderBy(s => s.Shop!.City).ThenBy(s => s.Product!.Name).ToList();
            ViewBag.Shops = _db.Shops.OrderBy(s => s.City).ToList();
            ViewBag.ShopId = shopId;
            return View(rows);
        }
    }
}

