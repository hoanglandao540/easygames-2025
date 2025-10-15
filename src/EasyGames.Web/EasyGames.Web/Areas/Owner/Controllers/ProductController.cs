using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db) => _db = db;

        // GET: /Owner/Products
        public IActionResult Index()
        {
            var items = _db.Products.OrderBy(x => x.Id).ToList();
            return View(items);
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

