using System;
using System.Linq;
using System.Threading.Tasks;
using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace EasyGames.Web.Areas.Shop.Controllers
{
    [Area("Shop")]
    [Authorize(Roles = nameof(AppRole.Shop))]
    public class PosController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IPosCartService _pos;
        private readonly ITierService _tier;
        private readonly IEmailService _email;

        public PosController(
            AppDbContext db,
            IPosCartService pos,
            ITierService tier,
            IEmailService email)
        {
            _db = db;
            _pos = pos;
            _tier = tier;
            _email = email;
        }

        // GET: /Shop/Pos
        public IActionResult Index()
        {
            ViewBag.Products = _db.Products.OrderBy(p => p.Name).ToList();
            ViewBag.Shops = _db.Shops.OrderBy(s => s.City).ToList();
            return View(_pos.Get());
        }

        // POST: /Shop/Pos/AddLine
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddLine(int productId, int qty = 1)
        {
            if (qty < 1) qty = 1;
            var p = _db.Products.Find(productId);
            if (p != null)
            {
                _pos.Add(p.Id, p.Name, p.Price, qty);
                TempData["toast"] = $"Added {qty} × {p.Name}";
            }
            else
            {
                TempData["toast"] = "Product not found.";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Inc(int id) { _pos.Inc(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Dec(int id) { _pos.Dec(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Remove(int id) { _pos.Remove(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Clear() { _pos.Clear(); return RedirectToAction(nameof(Index)); }

        // POST: /Shop/Pos/Pay
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int shopId, string? customerName, string? customerEmail, string? customerPhone)
        {
            var cart = _pos.Get();
            if (cart is null || !cart.Rows.Any())
            {
                TempData["toast"] = "No items to pay.";
                return RedirectToAction(nameof(Index));
            }

            // Validate shop exists
            var shop = await _db.Shops.FindAsync(shopId);
            if (shop == null)
            {
                TempData["toast"] = "Invalid shop selected.";
                return RedirectToAction(nameof(Index));
            }

            // FIX: Find or create Customer record with proper null handling
            Customer? cust = null;
            AppUser? appUser = null;

            if (!string.IsNullOrWhiteSpace(customerPhone))
            {
                var phone = customerPhone.Trim();

                // Check if registered user exists
                appUser = await _db.AppUsers
                    .FirstOrDefaultAsync(u => u.Phone == phone && u.Role == AppRole.Customer);

                // Find or create Customer record
                cust = await _db.Customers
                    .Include(c => c.AppUser)
                    .FirstOrDefaultAsync(c => c.Phone == phone);

                if (cust == null)
                {
                    cust = new Customer
                    {
                        Name = appUser?.Name ?? customerName ?? "Guest",
                        Email = appUser?.Email ?? customerEmail ?? "",
                        Phone = phone,
                        AppUserId = appUser?.Id
                    };
                    _db.Customers.Add(cust);
                    await _db.SaveChangesAsync();
                }
                else if (appUser != null && cust.AppUserId == null)
                {
                    // Link existing Customer to AppUser
                    cust.AppUserId = appUser.Id;
                    await _db.SaveChangesAsync();
                }
            }

            // Calculate subtotal
            var subtotal = cart.Rows.AsEnumerable().Sum(r => r.Price * r.Qty);

            // Apply tier discount
            decimal discount = 0m;
            if (cust != null && !string.IsNullOrWhiteSpace(cust.Phone))
            {
                var lifetime = await _db.Orders
                    .Where(o => o.CustomerPhone == cust.Phone)
                    .SumAsync(o => (decimal?)o.Total) ?? 0m;

                var tierLevel = _tier.Evaluate(lifetime);
                discount = tierLevel switch
                {
                    TierLevel.Silver => subtotal * 0.02m,
                    TierLevel.Gold => subtotal * 0.05m,
                    TierLevel.Platinum => subtotal * 0.08m,
                    _ => 0m
                };

                if (discount > 0)
                {
                    TempData["toast"] = $"{tierLevel} tier discount applied: {discount:0.00}";
                }
            }

            var total = subtotal - discount;

            await using var tx = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                ShopId = shopId,
                CustomerId = cust?.Id,
                CustomerPhone = cust?.Phone ?? customerPhone?.Trim(),
                Total = total,
                CreatedUtc = DateTime.UtcNow
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            // Load shop stocks for all products
            var productIds = cart.Rows.Select(r => r.ProductId).Distinct().ToList();
            var stocks = await _db.ShopStocks
                .Where(s => s.ShopId == shopId && productIds.Contains(s.ProductId))
                .ToListAsync();
            var byPid = stocks.ToDictionary(s => s.ProductId, s => s);

            foreach (var r in cart.Rows)
            {
                _db.OrderLines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = r.ProductId,
                    Name = r.Name,
                    Qty = r.Qty,
                    Price = r.Price
                });

                if (!byPid.TryGetValue(r.ProductId, out var srow))
                {
                    srow = new ShopStock
                    {
                        ShopId = shopId,
                        ProductId = r.ProductId,
                        Qty = 0,
                        ReorderLevel = 3
                    };
                    _db.ShopStocks.Add(srow);
                    byPid[r.ProductId] = srow;
                }

                var newQty = srow.Qty - r.Qty;
                if (newQty <= srow.ReorderLevel)
                {
                    TempData["lowStock"] = $"Warning: {r.Name} is now low/negative ({newQty} remaining).";
                }

                srow.Qty = newQty;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            // Send receipt
            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                await _email.SendAsync(customerEmail, "POS Receipt",
                    $"Thanks {customerName ?? "valued customer"}! Order #{order.Id} total: ${order.Total:0.00}.");
            }

            _pos.Clear();
            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        // GET: /Shop/Pos/Success
        public IActionResult Success(int id)
        {
            return View(id);
        }
    }
}

