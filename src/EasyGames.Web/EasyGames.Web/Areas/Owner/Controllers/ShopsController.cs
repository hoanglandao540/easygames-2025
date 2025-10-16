using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EasyGames.Web.Models;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ShopsController : Controller
    {
        private readonly AppDbContext _db;
        public ShopsController(AppDbContext db) => _db = db;

        // GET: /Owner/Shops
        public IActionResult Index()
        {
            var shops = _db.Shops
                .Include(s => s.Proprietor)
                .OrderBy(s => s.City)
                .ToList();
            return View(shops);
        }

        // GET: /Owner/Shops/Create
        public IActionResult Create()
        {
            ViewBag.Proprietors = _db.AppUsers
                .Where(u => u.Role == AppRole.Shop)
                .OrderBy(u => u.Name)
                .ToList();
            return View(new ShopLocation());
        }

        // POST: /Owner/Shops/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(ShopLocation m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proprietors = _db.AppUsers.Where(u => u.Role == AppRole.Shop).OrderBy(u => u.Name).ToList();
                return View(m);
            }

            _db.Shops.Add(m);
            _db.SaveChanges();
            TempData["toast"] = "Shop created.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Owner/Shops/Edit/5
        public IActionResult Edit(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();

            ViewBag.Proprietors = _db.AppUsers
                .Where(u => u.Role == AppRole.Shop)
                .OrderBy(u => u.Name)
                .ToList();
            return View(m);
        }

        // POST: /Owner/Shops/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ShopLocation m)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Proprietors = _db.AppUsers.Where(u => u.Role == AppRole.Shop).OrderBy(u => u.Name).ToList();
                return View(m);
            }

            _db.Shops.Update(m);
            _db.SaveChanges();
            TempData["toast"] = "Shop updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Owner/Shops/Delete/5
        public IActionResult Delete(int id)
        {
            var m = _db.Shops.Include(s => s.Proprietor).FirstOrDefault(s => s.Id == id);
            if (m == null) return NotFound();
            return View(m);
        }

        // POST: /Owner/Shops/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();

            _db.Shops.Remove(m);
            _db.SaveChanges();
            TempData["toast"] = "Shop deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}