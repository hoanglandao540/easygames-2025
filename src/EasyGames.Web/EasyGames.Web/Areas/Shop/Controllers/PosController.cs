
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
        private readonly IInventoryService _inv; // kept for other flows if you use it elsewhere
        private readonly ITierService _tier;
        private readonly IEmailService _email;

        public PosController(
            AppDbContext db,
            IPosCartService pos,
            IInventoryService inv,
            ITierService tier,
            IEmailService email)
        {
            _db = db;
            _pos = pos;
            _inv = inv;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // qty controls
        public IActionResult Inc(int id) { _pos.Inc(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Dec(int id) { _pos.Dec(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Remove(int id) { _pos.Remove(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Clear() { _pos.Clear(); return RedirectToAction(nameof(Index)); }

        // POST: /Shop/Pos/Pay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int shopId, string customerName, string customerEmail, string customerPhone)
        {
            var cart = _pos.Get();
            if (cart is null || !cart.Rows.Any())
            {
                TempData["toast"] = "No items to pay.";
                return RedirectToAction(nameof(Index), new { shopId });
            }

            // Lookup or create customer by phone (guest allowed)
            Customer? cust = null;
            if (!string.IsNullOrWhiteSpace(customerPhone))
            {
                var phone = customerPhone.Trim();
                cust = await _db.Customers.FirstOrDefaultAsync(c => c.Phone == phone);
                if (cust == null && (!string.IsNullOrWhiteSpace(customerName) || !string.IsNullOrWhiteSpace(customerEmail)))
                {
                    cust = new Customer { Name = customerName ?? "", Email = customerEmail ?? "", Phone = phone };
                    _db.Customers.Add(cust);
                    await _db.SaveChangesAsync();
                }
            }

            // Subtotal in-memory (avoid SQLite decimal SUM quirks)
            var subtotal = cart.Rows.AsEnumerable().Sum(r => r.Price * r.Qty);

            // Tier discount (if known customer)
            decimal discount = 0m;
            if (cust != null && !string.IsNullOrWhiteSpace(cust.Phone))
            {
                var lifetime = _db.Orders.AsNoTracking()
                    .Where(o => o.CustomerPhone == cust.Phone)
                    .AsEnumerable()
                    .Select(o => o.Total)
                    .DefaultIfEmpty(0m)
                    .Sum();

                var tier = _tier.Evaluate(lifetime);
                discount = tier switch
                {
                    TierLevel.Silver => subtotal * 0.02m,
                    TierLevel.Gold => subtotal * 0.05m,
                    TierLevel.Platinum => subtotal * 0.08m,
                    _ => 0m
                };
            }

            var total = subtotal - discount;

            await using var tx = await _db.Database.BeginTransactionAsync();

            var order = new Order
            {
                ShopId = shopId,
                CustomerId = cust?.Id,
                CustomerPhone = cust?.Phone ?? (string.IsNullOrWhiteSpace(customerPhone) ? null : customerPhone.Trim()),
                Total = total,
                CreatedUtc = DateTime.UtcNow
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(); // get Order.Id

            // load all involved shop stocks in one query
            var productIds = cart.Rows.Select(r => r.ProductId).Distinct().ToList();
            var stocks = await _db.ShopStocks.Where(s => s.ShopId == shopId && productIds.Contains(s.ProductId)).ToListAsync();
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
                    srow = new ShopStock { ShopId = shopId, ProductId = r.ProductId, Qty = 0, ReorderLevel = 3 };
                    _db.ShopStocks.Add(srow);
                    byPid[r.ProductId] = srow;
                }

                var newQty = srow.Qty - r.Qty; // allow negative → warn only
                if (newQty <= srow.ReorderLevel)
                    TempData["toast"] = $"Warning: {r.Name} low/negative (will be {newQty}). Sale allowed.";

                srow.Qty = newQty;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();

            // optional receipt (kept simple)
            if (!string.IsNullOrWhiteSpace(customerEmail))
            {
                await _email.SendAsync(customerEmail, "POS Receipt",
                    $"Thanks {customerName}! Order #{order.Id} total {order.Total:0.00}.");
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


