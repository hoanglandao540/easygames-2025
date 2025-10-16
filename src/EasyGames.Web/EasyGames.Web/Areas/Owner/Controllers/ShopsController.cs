using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using EasyGames.Web.Models;

=======
using EasyGames.Web.Models;


>>>>>>> feature/akshata/data-shops
namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ShopsController : Controller
    {
        private readonly AppDbContext _db;
        public ShopsController(AppDbContext db) => _db = db;

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
>>>>>>> feature/akshata/data-shops
            TempData["toast"] = "Shop created.";
            return RedirectToAction(nameof(Index));
        }

<<<<<<< HEAD
        // GET: /Owner/Shops/Edit/5
=======
>>>>>>> feature/akshata/data-shops
        public IActionResult Edit(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
<<<<<<< HEAD

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
=======
            return View(m);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Shop m)
        {
            if (!ModelState.IsValid) return View(m);
            _db.Shops.Update(m); _db.SaveChanges();
>>>>>>> feature/akshata/data-shops
            TempData["toast"] = "Shop updated.";
            return RedirectToAction(nameof(Index));
        }

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
            if (m == null) return NotFound();
            return View(m);
        }

<<<<<<< HEAD
        // POST: /Owner/Shops/Delete/5
=======
>>>>>>> feature/akshata/data-shops
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.Shops.Find(id);
            if (m == null) return NotFound();
<<<<<<< HEAD

            _db.Shops.Remove(m);
            _db.SaveChanges();
=======
            _db.Shops.Remove(m); _db.SaveChanges();
>>>>>>> feature/akshata/data-shops
            TempData["toast"] = "Shop deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
<<<<<<< HEAD
}
=======
}

>>>>>>> feature/akshata/data-shops
