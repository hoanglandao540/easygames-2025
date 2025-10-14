using EasyGames.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    public class CatalogController : Controller
    {
        private readonly AppDbContext _db;
        public CatalogController(AppDbContext db) => _db = db;

        public IActionResult Index()
            => View(_db.Products.OrderBy(p => p.Id).ToList());
    }
}
