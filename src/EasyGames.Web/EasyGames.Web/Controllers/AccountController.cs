using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyGames.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) { _db = db; }

        // GET: /Account/Register  (public)
        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View();

        // POST: /Account/Register  (public)  --> ALWAYS creates Customer
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public IActionResult Register(string name, string email, string password)
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
                Role = AppRole.Customer //  force to Customer
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            // go to Login after registration
            return RedirectToAction(nameof(Login));
        }

        // GET: /Account/Login (public)
        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login (public)
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            var hash = Password.Hash(password ?? "");
            var user = _db.AppUsers.FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);

            if (user == null)
            {
                TempData["toast"] = "Invalid email or password.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(id)
            );

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return user.Role switch
            {
                AppRole.Owner => RedirectToAction("Index", "Products", new { area = "Owner" }),
                AppRole.Shop => RedirectToAction("Index", "Pos", new { area = "Shop" }),
                AppRole.Customer => RedirectToAction("Index", "Catalog", new { area = "Storefront" }),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // POST: /Account/Logout (auth only)
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Denied
        [HttpGet, AllowAnonymous]
        public IActionResult Denied() => View();
    }
}



