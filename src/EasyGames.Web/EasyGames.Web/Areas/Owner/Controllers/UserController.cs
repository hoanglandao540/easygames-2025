using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) => _db = db;

        public IActionResult Index() => View(_db.Customers.OrderBy(x => x.Id).ToList());
        public IActionResult Create() => View(new Customer());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Customer m)
        { if (!ModelState.IsValid) return View(m); _db.Customers.Add(m); _db.SaveChanges(); TempData["msg"] = "User created."; return RedirectToAction(nameof(Index)); }

        public IActionResult Edit(int id) { var m = _db.Customers.Find(id); return m == null ? NotFound() : View(m); }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Customer m)
        { if (!ModelState.IsValid) return View(m); _db.Customers.Update(m); _db.SaveChanges(); TempData["msg"] = "User updated."; return RedirectToAction(nameof(Index)); }

        public IActionResult Delete(int id) { var m = _db.Customers.Find(id); return m == null ? NotFound() : View(m); }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        { var m = _db.Customers.Find(id); if (m == null) return NotFound(); _db.Customers.Remove(m); _db.SaveChanges(); TempData["msg"] = "User deleted."; return RedirectToAction(nameof(Index)); }
    }
}
