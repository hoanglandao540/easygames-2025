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
        public IActionResult Create(string name, string email, string password, AppRole role)
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
                PasswordHash = Password.Hash(password),
                Role = role
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            TempData["toast"] = "User created.";
            return RedirectToAction(nameof(Index));
        }
    }
}
