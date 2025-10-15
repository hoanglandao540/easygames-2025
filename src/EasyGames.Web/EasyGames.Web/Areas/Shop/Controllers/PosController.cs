using System;
using System.Linq;
using System.Threading.Tasks;
using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using EasyGames.Web.ViewModels;
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
        private readonly IInventoryService _inv;
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

        // quantity controls
        public IActionResult Inc(int id) { _pos.Inc(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Dec(int id) { _pos.Dec(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Remove(int id) { _pos.Remove(id); return RedirectToAction(nameof(Index)); }
        public IActionResult Clear() { _pos.Clear(); return RedirectToAction(nameof(Index)); }

        // POST: /Shop/Pos/Pay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int shopId, string customerName, string customerEmail)
        {
            var vm = _pos.Get();
            if (!vm.Rows.Any())
            {
                TempData["toast"] = "No items to pay.";
                return RedirectToAction(nameof(Index));
            }

            await using var tx = await _db.Database.BeginTransactionAsync();
            Order? order = null;

            try
            {
                // 1) create order header
                order = new Order
                {
                    CustomerName = customerName ?? "",
                    CustomerEmail = customerEmail ?? "",
                    CreatedAt = DateTime.UtcNow,
                    GrandTotal = vm.GrandTotal
                };
                _db.Orders.Add(order);
                await _db.SaveChangesAsync(); // get Id

                // 2) add lines + decrease stock
                foreach (var r in vm.Rows)
                {
                    _db.OrderLines.Add(new OrderLine
                    {
                        OrderId = order.Id,
                        ProductId = r.ProductId,
                        Name = r.Name,
                        Price = r.Price,
                        Qty = r.Qty
                    });

                    await _inv.DecreaseAsync(shopId, r.ProductId, r.Qty);
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch (Exception ex)
            {
                if (_db.Database.CurrentTransaction != null)
                {
                    try { await tx.RollbackAsync(); } catch { /* ignore */ }
                }

                TempData["toast"] = $"Payment failed: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            // 3) post-commit: compute tier (SQLite-safe client-side sum), send receipt, clear cart
            var email = order!.CustomerEmail ?? string.Empty;

            // Force client-side aggregation (SQLite can't SUM decimal server-side)
            var lifetime = _db.Orders
                .AsNoTracking()
                .Where(o => o.CustomerEmail == email)
                .AsEnumerable()                // switch to LINQ-to-Objects
                .Select(o => o.GrandTotal)     // decimal property
                .DefaultIfEmpty(0m)
                .Sum();

            var tier = _tier.Evaluate(lifetime);

            await _email.SendAsync(order.CustomerEmail, "POS Receipt",
                $"Thanks {order.CustomerName}! Order #{order.Id} total {order.GrandTotal:0.00}. Tier: {tier}.");

            _pos.Clear();
            return RedirectToAction(nameof(Success), new { id = order.Id, tier = tier });
        }

        // GET: /Shop/Pos/Success
        public IActionResult Success(int id, TierLevel tier)
        {
            ViewBag.Tier = tier;
            return View(id);
        }
    }
}

