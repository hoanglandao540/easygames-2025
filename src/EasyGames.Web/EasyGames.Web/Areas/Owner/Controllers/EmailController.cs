using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class EmailController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IEmailService _email;
        private readonly ITierService _tier;

        public EmailController(AppDbContext db, IEmailService email, ITierService tier)
        {
            _db = db;
            _email = email;
            _tier = tier;
        }

        // GET: /Owner/Email
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Owner/Email/SendToTier
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToTier(string tier, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                TempData["toast"] = "Subject and body are required.";
                return RedirectToAction(nameof(Index));
            }

            // Get all customer users with their phone numbers
            var customers = await _db.AppUsers
                .Where(u => u.Role == AppRole.Customer && u.Phone != "")
                .ToListAsync();

            int sentCount = 0;
            TierLevel targetTier = Enum.Parse<TierLevel>(tier);

            foreach (var customer in customers)
            {
                // Calculate lifetime spend from orders
                var lifetime = await _db.Orders
                    .Where(o => o.CustomerPhone == customer.Phone)
                    .SumAsync(o => (decimal?)o.Total) ?? 0m;

                var customerTier = _tier.Evaluate(lifetime);

                // Send if matches tier or if sending to all
                if (tier == "All" || customerTier == targetTier)
                {
                    await _email.SendAsync(customer.Email, subject, body);
                    sentCount++;
                }
            }

            TempData["toast"] = $"Email sent to {sentCount} customer(s) in {tier} tier.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /Owner/Email/SendToAll
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToAll(string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(body))
            {
                TempData["toast"] = "Subject and body are required.";
                return RedirectToAction(nameof(Index));
            }

            var customers = await _db.AppUsers
                .Where(u => u.Role == AppRole.Customer)
                .ToListAsync();

            foreach (var customer in customers)
            {
                await _email.SendAsync(customer.Email, subject, body);
            }

            TempData["toast"] = $"Email sent to all {customers.Count} customer(s).";
            return RedirectToAction(nameof(Index));
        }
    }
}