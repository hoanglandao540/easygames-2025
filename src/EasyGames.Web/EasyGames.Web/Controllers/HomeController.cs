using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyGames.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!(User?.Identity?.IsAuthenticated ?? false))
                return View("Guest");

            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            return role switch
            {
                "Owner" => Redirect("/Owner/Dashboard"),
                "Shop" => Redirect("/Shop/Dashboard"),
                "Customer" => Redirect("/Storefront/Dashboard"),
                _ => Redirect("/Storefront/Catalog")
            };
        }

        public IActionResult Guest() => View();
    }
}


