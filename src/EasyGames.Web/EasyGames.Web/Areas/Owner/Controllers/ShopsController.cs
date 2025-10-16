using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
<<<<<<< HEAD
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using EasyGames.Web.Models;

=======
using EasyGames.Web.Models;


>>>>>>> feature/akshata/data-shops
=======
using Microsoft.EntityFrameworkCore;
using EasyGames.Web.Models;

>>>>>>> origin/feature/hoang/pos-tier-email
namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ShopsController : Controller
    {
        private readonly AppDbContext _db;
        public ShopsController(AppDbContext db) => _db = db;

<<<<<<< HEAD
<<<<<<< HEAD
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
=======
        // student-style: simple list page to prove DB+seed
=======
        // GET: /Owner/Shops
>>>>>>> origin/feature/hoang/pos-tier-email
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
<<<<<<< HEAD
            if (!ModelState.IsValid) return View(m);
            _db.Shops.Add(m); _db.SaveChanges();
>>>>>>> feature/akshata/data-shops
=======
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
>>>>>>> origin/feature/hoang/pos-tier-email
            TempData["toast"] = "Shop created.";
            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
<<<<<<< HEAD
        // GET: /Owner/Shops/Edit/5
=======
>>>>>>> feature/akshata/data-shops
=======
        // GET: /Owner/Shops/Edit/5
>>>>>>> origin/feature/hoang/pos-tier-email
        public IActionResult Edit(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> origin/feature/hoang/pos-tier-email

            ViewBag.Proprietors = _db.AppUsers
                .Where(u => u.Role == AppRole.Shop)
                .OrderBy(u => u.Name)
                .ToList();
<<<<<<< HEAD
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
=======
=======
>>>>>>> origin/feature/hoang/pos-tier-email
            return View(m);
        }

        // POST: /Owner/Shops/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(ShopLocation m)
        {
<<<<<<< HEAD
            if (!ModelState.IsValid) return View(m);
            _db.Shops.Update(m); _db.SaveChanges();
>>>>>>> feature/akshata/data-shops
=======
            if (!ModelState.IsValid)
            {
                ViewBag.Proprietors = _db.AppUsers.Where(u => u.Role == AppRole.Shop).OrderBy(u => u.Name).ToList();
                return View(m);
            }

            _db.Shops.Update(m);
            _db.SaveChanges();
>>>>>>> origin/feature/hoang/pos-tier-email
            TempData["toast"] = "Shop updated.";
            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
<<<<<<< HEAD
        // GET: /Owner/Shops/Delete/5
        public IActionResult Delete(int id)
        {
            var m = _db.Shops.Include(s => s.Proprietor).FirstOrDefault(s => s.Id == id);
=======
        public IActionResult Delete(int id)
        {
            var m = _db.Shops.Find(id);
>>>>>>> feature/akshata/data-shops
=======
        // GET: /Owner/Shops/Delete/5
        public IActionResult Delete(int id)
        {
            var m = _db.Shops.Include(s => s.Proprietor).FirstOrDefault(s => s.Id == id);
>>>>>>> origin/feature/hoang/pos-tier-email
            if (m == null) return NotFound();
            return View(m);
        }

<<<<<<< HEAD
<<<<<<< HEAD
        // POST: /Owner/Shops/Delete/5
=======
>>>>>>> feature/akshata/data-shops
=======
        // POST: /Owner/Shops/Delete/5
>>>>>>> origin/feature/hoang/pos-tier-email
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
<<<<<<< HEAD
<<<<<<< HEAD

            _db.Shops.Remove(m);
            _db.SaveChanges();
=======
            _db.Shops.Remove(m); _db.SaveChanges();
>>>>>>> feature/akshata/data-shops
=======

            _db.Shops.Remove(m);
            _db.SaveChanges();
>>>>>>> origin/feature/hoang/pos-tier-email
            TempData["toast"] = "Shop deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
<<<<<<< HEAD
<<<<<<< HEAD
}
=======
}

>>>>>>> feature/akshata/data-shops
=======
}
>>>>>>> origin/feature/hoang/pos-tier-email
