using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyGames.Web.Models;


namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ShopsController : Controller
    {
        private readonly AppDbContext _db;
        public ShopsController(AppDbContext db) => _db = db;

        // student-style: simple list page to prove DB+seed
        public IActionResult Index()
        {
            var shops = _db.Shops.OrderBy(s => s.City).ToList();
            return View(shops);
        }

        public IActionResult Create() => View(new Shop());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Shop m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Shops.Add(m); _db.SaveChanges();
            TempData["toast"] = "Shop created.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Shop m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Shops.Update(m); _db.SaveChanges();
            TempData["toast"] = "Shop updated.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
            _db.Shops.Remove(m); _db.SaveChanges();
            TempData["toast"] = "Shop deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}

