using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Shop.Controllers
{
    [Area("Shop")]
    [Authorize(Roles = nameof(AppRole.Shop))]
    public class TransferController : Controller
    {
        private readonly AppDbContext _db;
        public TransferController(AppDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var owner = _db.OwnerStocks.Include(o => o.Product)
                .OrderBy(o => o.Product!.Name).ToList();
            ViewBag.Shops = _db.Shops.OrderBy(s => s.City).ToList();
            return View(owner);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Pull(int shopId, int ownerStockId, int qty)
        {
            if (qty < 1) qty = 1;
            var os = _db.OwnerStocks.Include(o => o.Product).FirstOrDefault(o => o.Id == ownerStockId);
            if (os == null) { TempData["toast"] = "Owner stock not found."; return RedirectToAction(nameof(Index)); }
            if (os.Qty < qty) { TempData["toast"] = $"Owner has only {os.Qty}."; return RedirectToAction(nameof(Index)); }

            // reduce owner stock
            os.Qty -= qty;

            // find or create ShopStock
            var ss = _db.ShopStocks.FirstOrDefault(s => s.ShopId == shopId && s.ProductId == os.ProductId);
            if (ss == null)
            {
                ss = new ShopStock
                {
                    ShopId = shopId,
                    ProductId = os.ProductId,
                    Qty = 0,
                    ReorderLevel = 3,
                    Source = os.Source,
                    BuyPrice = os.BuyPrice,
                    SellPrice = os.SellPrice
                };
                _db.ShopStocks.Add(ss);
            }

            // inherit fields + increase qty
            ss.Source = os.Source;
            ss.BuyPrice = os.BuyPrice;
            ss.SellPrice = os.SellPrice;
            ss.Qty += qty;

            _db.SaveChanges();
            TempData["toast"] = $"Pulled {qty} × {os.Product?.Name} into shop.";
            return RedirectToAction(nameof(Index));
        }
    }
}
