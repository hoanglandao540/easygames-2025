using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Controllers
{
    // student-style: simple landing page
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
