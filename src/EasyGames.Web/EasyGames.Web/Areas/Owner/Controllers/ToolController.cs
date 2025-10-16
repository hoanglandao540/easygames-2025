using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ToolsController : Controller
    {
        private readonly AppDbContext _db;
        public ToolsController(AppDbContext db) { _db = db; }

        [HttpGet]
        public IActionResult SeedOwnerInventory() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult SeedOwnerInventoryNow()
        {
            if (_db.OwnerStocks.Any())
            {
                TempData["toast"] = "Owner inventory already has rows — nothing to do.";
                return RedirectToAction(nameof(SeedOwnerInventory));
            }

            var prods = _db.Products.OrderBy(p => p.Id).ToList();
            if (prods.Count == 0)
            {
                TempData["toast"] = "No products exist. Create products first.";
                return RedirectToAction(nameof(SeedOwnerInventory));
            }

            _db.OwnerStocks.AddRange(
                new OwnerStock { ProductId = prods[0].Id, Qty = 50, Source = "HQ Shipment", BuyPrice = 20.00m, SellPrice = 29.99m },
                new OwnerStock { ProductId = prods[1].Id, Qty = 100, Source = "Token Vendor", BuyPrice = 5.00m, SellPrice = 9.99m },
                new OwnerStock { ProductId = prods[2].Id, Qty = 30, Source = "Gift Supplier", BuyPrice = 35.00m, SellPrice = 49.00m }
            );
            _db.SaveChanges();

            TempData["toast"] = "Owner inventory seeded.";
            return RedirectToAction(nameof(SeedOwnerInventory));
        }
    }
}


