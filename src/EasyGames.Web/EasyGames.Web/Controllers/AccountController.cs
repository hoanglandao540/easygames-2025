using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
<<<<<<< HEAD
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
=======
>>>>>>> feature/akshata/data-shops
=======
using Microsoft.EntityFrameworkCore;
>>>>>>> origin/feature/hoang/pos-tier-email

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
<<<<<<< HEAD
<<<<<<< HEAD
        public IActionResult Register(string name, string email, string password, string? phone)
=======
        public IActionResult Register(string name, string email, string password)
>>>>>>> feature/akshata/data-shops
=======
        public IActionResult Register(string name, string email, string password, string? phone)
>>>>>>> origin/feature/hoang/pos-tier-email
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
<<<<<<< HEAD
                Phone = (phone ?? "").Trim(),
                PasswordHash = Password.Hash(password),
                Role = AppRole.Customer
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();

            TempData["toast"] = "Account created! Please login.";
=======
=======
                Phone = (phone ?? "").Trim(),
>>>>>>> origin/feature/hoang/pos-tier-email
                PasswordHash = Password.Hash(password),
                Role = AppRole.Customer
            };
            _db.AppUsers.Add(user);
            _db.SaveChanges();
<<<<<<< HEAD
>>>>>>> feature/akshata/data-shops
=======

            TempData["toast"] = "Account created! Please login.";
>>>>>>> origin/feature/hoang/pos-tier-email
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
<<<<<<< HEAD
<<<<<<< HEAD
            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);
=======
            var user = _db.AppUsers.FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);
>>>>>>> feature/akshata/data-shops
=======
            var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hash);
>>>>>>> origin/feature/hoang/pos-tier-email

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
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> origin/feature/hoang/pos-tier-email

            // Add phone claim for customers (needed for order history)
            if (!string.IsNullOrWhiteSpace(user.Phone))
            {
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Phone));
            }

<<<<<<< HEAD
=======
>>>>>>> feature/akshata/data-shops
=======
>>>>>>> origin/feature/hoang/pos-tier-email
            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return Redirect(defaultLanding);
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        // ===== Logout =====
=======
        // ===== Logout (go straight to Guest page) =====
>>>>>>> feature/akshata/data-shops
=======
        // ===== Logout =====
>>>>>>> origin/feature/hoang/pos-tier-email
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
<<<<<<< HEAD
<<<<<<< HEAD
=======
        // ===== FIXED LOGOUT - Clears session and redirects to Guest =====
        [HttpPost, ValidateAntiForgeryToken, Authorize]
        public async Task<IActionResult> Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();

            // Sign out from cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            TempData["toast"] = "You have been logged out successfully.";

            // Redirect to Guest landing page
>>>>>>> origin/feature/hoang/pos-tier-email
            return RedirectToAction("Guest", "Home");
=======
            return RedirectToAction("Guest", "Home"); // <- always go to Guest page
>>>>>>> feature/akshata/data-shops
=======
            return RedirectToAction("Guest", "Home");
>>>>>>> origin/feature/hoang/pos-tier-email
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Denied() => View();
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> origin/feature/hoang/pos-tier-email

        // ===== Profile =====
        [HttpGet, Authorize]
        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = _db.AppUsers.Find(userId);
            if (user == null) return NotFound();
            return View(user);
        }
<<<<<<< HEAD
    }
}
=======
    }
}


>>>>>>> feature/akshata/data-shops
=======
    }
}
>>>>>>> origin/feature/hoang/pos-tier-email
