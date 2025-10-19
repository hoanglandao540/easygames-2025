using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    public class CatalogController : Controller
    {
        private readonly AppDbContext _db;
        public CatalogController(AppDbContext db) => _db = db;

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

            // Sorting - FIX: Cast Price to double for SQLite compatibility
            query = sort switch
            {
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price_asc" => query.OrderBy(p => (double)p.Price),
                "price_desc" => query.OrderByDescending(p => (double)p.Price),
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
    }
}