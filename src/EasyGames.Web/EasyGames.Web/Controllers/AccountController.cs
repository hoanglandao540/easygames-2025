using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace EasyGames.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db;
        public AccountController(AppDbContext db) { _db = db; }

        // ===== Register (Customer-only) =====
        [HttpGet, AllowAnonymous]
        public IActionResult Register() => View();

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public IActionResult Register(string name, string email, string password, string? phone)
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
                Role = AppRole.Customer
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            TempData["toast"] = "Account created! Please login.";
            return RedirectToAction(nameof(LoginCustomer));
        }

        // ===== Role-specific logins =====
        [HttpGet, AllowAnonymous]
        public IActionResult LoginOwner(string? returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public Task<IActionResult> LoginOwner(string email, string password, string? returnUrl)
            => DoLogin(email, password, AppRole.Owner, "/Owner/Dashboard", returnUrl, "This page is for Owner login.");

        [HttpGet, AllowAnonymous]
        public IActionResult LoginShop(string? returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public Task<IActionResult> LoginShop(string email, string password, string? returnUrl)
            => DoLogin(email, password, AppRole.Shop, "/Shop/Dashboard", returnUrl, "This page is for Shop Proprietor login.");

        [HttpGet, AllowAnonymous]
        public IActionResult LoginCustomer(string? returnUrl) { ViewBag.ReturnUrl = returnUrl; return View(); }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public Task<IActionResult> LoginCustomer(string email, string password, string? returnUrl)
            => DoLogin(email, password, AppRole.Customer, "/Storefront/Dashboard", returnUrl, "This page is for Customer login.");

        // ===== Shared login core =====
        private async Task<IActionResult> DoLogin(string email, string password, AppRole requiredRole, string defaultLanding, string? returnUrl, string wrongRoleMsg)
        {
            var hash = Password.Hash(password ?? "");
            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);

            if (user == null)
            {
                TempData["toast"] = "Invalid email or password.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            if (user.Role != requiredRole)
            {
                TempData["toast"] = wrongRoleMsg;
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

            // Add phone claim for customers (needed for order history)
            if (!string.IsNullOrWhiteSpace(user.Phone))
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));
            }

            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return Redirect(defaultLanding);
        }

        // ===== Logout =====
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Guest", "Home");
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Denied() => View();

        // ===== Profile =====
        [HttpGet, Authorize]
        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = _db.AppUsers.Find(userId);
            if (user == null) return NotFound();
            return View(user);
        }
    }
}