using EasyGames.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EasyGames.Web.Models;

namespace EasyGames.Web.Areas.Shop.Controllers
{
    [Area("Shop")]
    [Authorize(Roles = nameof(AppRole.Shop))]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) { _db = db; }

        public IActionResult Index()
        {
            // No direct user->shop mapping, so show a useful stock overview
            var model = _db.ShopStocks
                .Select(s => new ShopStockRow
                {
                    ShopName = _db.Shops.Where(x => x.Id == s.ShopId).Select(x => x.City + " (" + x.ShopCode + ")").FirstOrDefault() ?? "Shop",
                    ProductId = s.ProductId,
                    Qty = s.Qty,
                    Reorder = s.ReorderLevel
                })
                .OrderBy(x => x.ShopName).ThenBy(x => x.ProductId)
                .Take(20)
                .ToList();

            return View(model);
        }
    }

    public class ShopStockRow
    {
        public string ShopName { get; set; } = "";
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int Reorder { get; set; }
    }
}
