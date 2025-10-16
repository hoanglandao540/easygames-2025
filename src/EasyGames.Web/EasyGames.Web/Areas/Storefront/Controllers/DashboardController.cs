using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EasyGames.Web.Models;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    [Authorize(Roles = nameof(AppRole.Customer))]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View(); // simple greeting + shortcuts
        }
    }
}
