using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    [Authorize(Roles = nameof(AppRole.Customer))]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ITierService _tier;

        public OrdersController(AppDbContext db, ITierService tier)
        {
            _db = db;
            _tier = tier;
        }

        // GET: /Storefront/Orders
        public async Task<IActionResult> Index()
        {
            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            // FIX: Don't hide orders if user has no phone - show message instead
            if (string.IsNullOrWhiteSpace(phone))
            {
                ViewBag.NoPhone = true;
                ViewBag.LifetimeSpend = 0m;
                ViewBag.CurrentTier = TierLevel.Bronze;
                ViewBag.NextTier = TierLevel.Silver;
                ViewBag.NextThreshold = 200m;
                return View(new List<Order>());
            }

            var orders = await _db.Orders
                .Include(o => o.Shop)
                .Include(o => o.Lines)
                .Where(o => o.CustomerPhone == phone)
                .OrderByDescending(o => o.CreatedUtc)
                .ToListAsync();

            // Calculate tier info
            var lifetime = orders.Sum(o => o.Total);
            var currentTier = _tier.Evaluate(lifetime);
            var nextTier = currentTier == TierLevel.Platinum
                ? TierLevel.Platinum
                : (TierLevel)((int)currentTier + 1);
            var nextThreshold = _tier.NextThreshold(currentTier);

            ViewBag.LifetimeSpend = lifetime;
            ViewBag.CurrentTier = currentTier;
            ViewBag.NextTier = nextTier;
            ViewBag.NextThreshold = nextThreshold;

            return View(orders);
        }

        // GET: /Storefront/Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var phone = User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            var order = await _db.Orders
                .Include(o => o.Shop)
                .Include(o => o.Lines)
                .FirstOrDefaultAsync(o => o.Id == id && o.CustomerPhone == phone);

            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}
