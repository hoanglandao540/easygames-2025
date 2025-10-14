using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    public class ShopsController : Controller
    {
        private readonly AppDbContext _db;
        public ShopsController(AppDbContext db) => _db = db;

        // student-style: simple list page to prove DB+seed
        public IActionResult Index()
        {
            var shops = _db.Shops.OrderBy(s => s.City).ToList();
            return View(shops);
        }
    }
}

