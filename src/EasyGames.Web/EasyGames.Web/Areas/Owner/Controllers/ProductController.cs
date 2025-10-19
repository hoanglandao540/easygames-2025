using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) => _db = db;

        // GET: /Owner/Products with Search and Filter
        public async Task<IActionResult> Index(string? search, string? category, string? sort)
        {
            var query = _db.Products.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Code.ToLower().Contains(searchTerm));
            }

            // Category filter
            if (!string.IsNullOrWhiteSpace(category) && category != "All")
            {
                query = query.Where(p => p.Category == category);
            }

            // Sorting
            query = sort switch
            {
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price_asc" => query.OrderBy(p => (double)p.Price),
                "price_desc" => query.OrderByDescending(p => (double)p.Price),
                "code_asc" => query.OrderBy(p => p.Code),
                _ => query.OrderBy(p => p.Id)
            };

            var products = await query.ToListAsync();

            // Pass filter values to view
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = category;
            ViewBag.CurrentSort = sort;
            ViewBag.Categories = await _db.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            return View(products);
        }

        // GET: /Owner/Products/Create
        public IActionResult Create() => View(new Product());

        // POST: /Owner/Products/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Product m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Products.Add(m);
            _db.SaveChanges();
            TempData["msg"] = "Product created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Owner/Products/Edit/5
        public IActionResult Edit(int id)
        {
            var m = _db.Products.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // POST: /Owner/Products/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Product m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Products.Update(m);
            _db.SaveChanges();
            TempData["msg"] = "Product updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Owner/Products/Delete/5
        public IActionResult Delete(int id)
        {
            var m = _db.Products.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // POST: /Owner/Products/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.Products.Find(id);
            if (m == null) return NotFound();
            _db.Products.Remove(m);
            _db.SaveChanges();
            TempData["msg"] = "Product deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}