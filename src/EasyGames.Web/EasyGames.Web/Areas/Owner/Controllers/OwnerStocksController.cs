using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class OwnerStocksController : Controller
    {
        private readonly AppDbContext _db;
        public OwnerStocksController(AppDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var list = _db.OwnerStocks.Include(o => o.Product)
                .OrderBy(o => o.Product!.Name).ToList();
            return View(list);
        }

        public IActionResult Create()
        {
            ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
            return View(new OwnerStock());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(OwnerStock m)
        {
            if (!ModelState.IsValid) { ViewBag.Products = _db.Products.ToList(); return View(m); }
            _db.OwnerStocks.Add(m); _db.SaveChanges();
            TempData["toast"] = "Owner stock created."; return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var m = _db.OwnerStocks.Find(id); if (m == null) return NotFound();
            ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(OwnerStock m)
        {
            if (!ModelState.IsValid) { ViewBag.Products = _db.Products.ToList(); return View(m); }
            _db.OwnerStocks.Update(m); _db.SaveChanges();
            TempData["toast"] = "Owner stock updated."; return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var m = _db.OwnerStocks.Include(x => x.Product).FirstOrDefault(x => x.Id == id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.OwnerStocks.Find(id); if (m == null) return NotFound();
            _db.OwnerStocks.Remove(m); _db.SaveChanges();
            TempData["toast"] = "Owner stock deleted."; return RedirectToAction(nameof(Index));
        }
    }
}
