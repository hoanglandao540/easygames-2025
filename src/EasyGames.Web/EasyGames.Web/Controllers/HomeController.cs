using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyGames.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: /
        // If not logged in => show Guest view
        // If logged in => redirect by role
        public IActionResult Index()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
            {
                // not logged in => show guest landing
                return View("Guest");
            }

            // logged in => route by role
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            return role switch
            {
                "Owner" => RedirectToAction("Index", "Products", new { area = "Owner" }),
                "Shop" => RedirectToAction("Index", "Pos", new { area = "Shop" }),
                "Customer" => RedirectToAction("Index", "Catalog", new { area = "Storefront" }),
                _ => RedirectToAction("Index", "Catalog", new { area = "Storefront" }) // default
            };
        }

        // Optional explicit route: /Home/Guest
        public IActionResult Guest() => View();
    }
}
