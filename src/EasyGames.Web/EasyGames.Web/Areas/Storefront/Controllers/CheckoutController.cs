using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using EasyGames.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            ViewBag.Cart = _cart.Get();
            return View(new CheckoutVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutVM vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Cart = _cart.Get();
                return View(vm);
            }

            var cart = _cart.Get();
            if (!cart.Rows.Any())
            {
                TempData["msg"] = "Cart is empty.";
                return RedirectToAction("Index", "Catalog");
            }

            // Get current user's phone from claims
            var userPhone = User.FindFirst(ClaimTypes.MobilePhone)?.Value ?? "";

            // FIX: Find or create Customer record
            Customer? customer = null;
            if (!string.IsNullOrWhiteSpace(userPhone))
            {
                customer = await _db.Customers
                    .FirstOrDefaultAsync(c => c.Phone == userPhone);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        Name = vm.CustomerName ?? "",
                        Email = vm.CustomerEmail ?? "",
                        Phone = userPhone
                    };
                    _db.Customers.Add(customer);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // Update if newer info provided
                    if (!string.IsNullOrWhiteSpace(vm.CustomerName))
                        customer.Name = vm.CustomerName;
                    if (!string.IsNullOrWhiteSpace(vm.CustomerEmail))
                        customer.Email = vm.CustomerEmail;
                    _db.Customers.Update(customer);
                    await _db.SaveChangesAsync();
                }
            }

            // Calculate subtotal
            var subtotal = cart.Rows.Sum(r => r.Price * r.Qty);

            // Apply tier discount
            decimal discount = 0m;
            if (customer != null && !string.IsNullOrWhiteSpace(customer.Phone))
            {
                var lifetime = await _db.Orders
                    .Where(o => o.CustomerPhone == customer.Phone)
                    .SumAsync(o => (decimal?)o.Total) ?? 0m;

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
                ShopId = 1, // Default shop or get from context
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
