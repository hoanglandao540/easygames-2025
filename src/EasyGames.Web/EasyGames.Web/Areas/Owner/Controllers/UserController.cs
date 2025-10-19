using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class UsersController : Controller
    {
        private readonly AppDbContext _db;
        public UsersController(AppDbContext db) { _db = db; }

        // GET: /Owner/Users with Search and Filter
        public async Task<IActionResult> Index(string? search, string? role, string? sort)
        {
            var query = _db.AppUsers.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchTerm = search.Trim().ToLower();
                query = query.Where(u =>
                    u.Name.ToLower().Contains(searchTerm) ||
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.Phone.ToLower().Contains(searchTerm));
            }

            // Role filter
            if (!string.IsNullOrWhiteSpace(role) && role != "All")
            {
                var roleEnum = Enum.Parse<AppRole>(role);
                query = query.Where(u => u.Role == roleEnum);
            }

            // Sorting
            query = sort switch
            {
                "name_asc" => query.OrderBy(u => u.Name),
                "name_desc" => query.OrderByDescending(u => u.Name),
                "email_asc" => query.OrderBy(u => u.Email),
                "role_asc" => query.OrderBy(u => u.Role).ThenBy(u => u.Name),
                _ => query.OrderBy(u => u.Role).ThenBy(u => u.Name)
            };

            var list = await query.ToListAsync();

            // Pass filter values
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentRole = role;
            ViewBag.CurrentSort = sort;

            return View(list);
        }

        // GET: /Owner/Users/Create
        public IActionResult Create() => View();

        // POST: /Owner/Users/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(string name, string email, string password, string? phone, AppRole role)
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
                Phone = (phone ?? "").Trim(),
                PasswordHash = Password.Hash(password),
                Role = role
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            TempData["toast"] = "User created.";
            return RedirectToAction(nameof(Index));
        }

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