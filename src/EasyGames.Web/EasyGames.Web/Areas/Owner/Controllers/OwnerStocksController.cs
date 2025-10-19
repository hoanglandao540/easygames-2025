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

        public async Task<IActionResult> Index(string? search, string? source, string? sort)
        {
            var query = _db.OwnerStocks.Include(o => o.Product).AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(o =>
                    o.Product!.Name.ToLower().Contains(searchTerm) ||
                    o.Product!.Code.ToLower().Contains(searchTerm));
            }

            // Source filter
            if (!string.IsNullOrWhiteSpace(source) && source != "All")
            {
                query = query.Where(o => o.Source == source);
            }

            // Sorting
            query = sort switch
            {
                "name_asc" => query.OrderBy(o => o.Product!.Name),
                "name_desc" => query.OrderByDescending(o => o.Product!.Name),
                "qty_asc" => query.OrderBy(o => o.Qty),
                "qty_desc" => query.OrderByDescending(o => o.Qty),
                "profit_asc" => query.OrderBy(o => (double)(o.SellPrice - o.BuyPrice)),
                "profit_desc" => query.OrderByDescending(o => (double)(o.SellPrice - o.BuyPrice)),
                _ => query.OrderBy(o => o.Product!.Name)
            };

            var list = await query.ToListAsync();

            // Pass filter values
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentSource = source;
            ViewBag.CurrentSort = sort;
            ViewBag.Sources = await _db.OwnerStocks
                .Select(o => o.Source)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

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
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
                return View(m);
            }
            _db.OwnerStocks.Add(m);
            _db.SaveChanges();
            TempData["toast"] = "Owner stock created.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var m = _db.OwnerStocks.Find(id);
            if (m == null) return NotFound();
            ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(OwnerStock m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
                return View(m);
            }
            _db.OwnerStocks.Update(m);
            _db.SaveChanges();
            TempData["toast"] = "Owner stock updated.";
            return RedirectToAction(nameof(Index));
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
            var m = _db.OwnerStocks.Find(id);
            if (m == null) return NotFound();
            _db.OwnerStocks.Remove(m);
            _db.SaveChanges();
            TempData["toast"] = "Owner stock deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}