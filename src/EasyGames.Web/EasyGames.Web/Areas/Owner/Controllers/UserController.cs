using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        // GET: /Owner/Users
        public IActionResult Index()
        {
            var list = _db.AppUsers
                          .OrderBy(u => u.Role)
                          .ThenBy(u => u.Name)
                          .ToList();
            return View(list);
        }

        // GET: /Owner/Users/Create
        public IActionResult Create() => View();

        // POST: /Owner/Users/Create
        [HttpPost, ValidateAntiForgeryToken]
<<<<<<< HEAD
        public IActionResult Create(string name, string email, string password, string? phone, AppRole role)
=======
        public IActionResult Create(string name, string email, string password, AppRole role)
>>>>>>> feature/akshata/data-shops
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["toast"] = "Email and password are required.";
                return View();
            }
            if (_db.AppUsers.Any(u => u.Email == email))
            {
                TempData["toast"] = "Email already exists.";
                return View();
            }

            var user = new AppUser
            {
                Name = (name ?? "").Trim(),
                Email = email.Trim(),
<<<<<<< HEAD
                Phone = (phone ?? "").Trim(),
=======
>>>>>>> feature/akshata/data-shops
                PasswordHash = Password.Hash(password),
                Role = role
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            TempData["toast"] = "User created.";
            return RedirectToAction(nameof(Index));
        }
<<<<<<< HEAD

        // GET: /Owner/Users/Edit/5
        public IActionResult Edit(int id)
        {
            var m = _db.AppUsers.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // POST: /Owner/Users/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(AppUser m, string? newPassword)
        {
            if (!ModelState.IsValid) return View(m);

            var existing = _db.AppUsers.Find(m.Id);
            if (existing == null) return NotFound();

            existing.Name = m.Name;
            existing.Email = m.Email;
            existing.Phone = m.Phone;
            existing.Role = m.Role;

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                existing.PasswordHash = Password.Hash(newPassword);
            }

            _db.SaveChanges();
            TempData["toast"] = "User updated.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Owner/Users/Delete/5
        public IActionResult Delete(int id)
        {
            var m = _db.AppUsers.Find(id);
            if (m == null) return NotFound();
            return View(m);
        }

        // POST: /Owner/Users/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var m = _db.AppUsers.Find(id);
            if (m == null) return NotFound();

            _db.AppUsers.Remove(m);
            _db.SaveChanges();
            TempData["toast"] = "User deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
=======
    }
}
>>>>>>> feature/akshata/data-shops
