using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using EasyGames.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    [Authorize(Roles = nameof(AppRole.Customer))]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ICartService _cart;
        private readonly ITierService _tier;

        public CheckoutController(AppDbContext db, ICartService cart, ITierService tier)
        {
            _db = db;
            _cart = cart;
            _tier = tier;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = _cart.Get();
            if (!cart.Rows.Any())
            {
                TempData["toast"] = "Cart is empty.";
                return RedirectToAction("Index", "Catalog");
            }

            // Pre-fill with user info
            var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var vm = new CheckoutVM
            {
                CustomerName = userName,
                CustomerEmail = userEmail
            };

            ViewBag.Cart = cart;
            ViewBag.UserName = userName;
            ViewBag.UserEmail = userEmail;

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutVM vm, string address)
        {
            var cart = _cart.Get();
            if (!cart.Rows.Any())
            {
                TempData["msg"] = "Cart is empty.";
                return RedirectToAction("Index", "Catalog");
            }

            // Get user info from claims (cannot be changed)
            var userName = User.FindFirst(ClaimTypes.Name)?.Value ?? "";
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var userPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";

            if (string.IsNullOrWhiteSpace(address))
            {
                ModelState.AddModelError("address", "Delivery address is required.");
                ViewBag.Cart = cart;
                ViewBag.UserName = userName;
                ViewBag.UserEmail = userEmail;
                return View(vm);
            }

            // Find or create Customer record
            Customer? customer = null;
            if (!string.IsNullOrWhiteSpace(userPhone))
            {
                customer = await _db.Customers
                    .FirstOrDefaultAsync(c => c.Phone == userPhone);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = userName,
                        Email = userEmail,
                        Phone = userPhone
                    };
                    _db.Customers.Add(customer);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Update info if changed
                    customer.Name = userName;
                    customer.Email = userEmail;
                    _db.Customers.Update(customer);
                    await _db.SaveChangesAsync();
                }
            }

            // Calculate subtotal - FIX: Use AsEnumerable() for client-side calculation
            var subtotal = cart.Rows.AsEnumerable().Sum(r => r.Price * r.Qty);

            // Apply tier discount - async-safe approach: fetch orders async, then sum in memory
            decimal discount = 0m;
            if (customer != null && !string.IsNullOrWhiteSpace(customer.Phone))
            {
                // Fetch matching orders from DB asynchronously
                var orders = await _db.Orders
                    .Where(o => o.CustomerPhone == customer.Phone)
                    .ToListAsync();

                // Sum on client side (works even if list is empty)
                var lifetime = orders.Sum(o => o.Total);

                var tierLevel = _tier.Evaluate(lifetime);
                discount = tierLevel switch
                {
                    TierLevel.Silver => subtotal * 0.02m,
                    TierLevel.Gold => subtotal * 0.05m,
                    TierLevel.Platinum => subtotal * 0.08m,
                    _ => 0m
                };
            }

            var total = subtotal - discount;

            // Create order
            var order = new Order
            {
                ShopId = 1, // Default online shop
                CustomerId = customer?.Id,
                CustomerPhone = customer?.Phone ?? userPhone,
                Total = total,
                CreatedUtc = DateTime.UtcNow
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Add order lines
            foreach (var r in cart.Rows)
            {
                _db.OrderLines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = r.ProductId,
                    Name = r.Name,
                    Price = r.Price,
                    Qty = r.Qty
                });
            }
            await _db.SaveChangesAsync();

            _cart.Clear();
            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        public IActionResult Success(int id) => View(id);
    }
}
