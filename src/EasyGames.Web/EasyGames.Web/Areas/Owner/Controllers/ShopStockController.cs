using EasyGames.Web.Data;
using EasyGames.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EasyGames.Web.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = nameof(AppRole.Owner))]
    public class ShopStocksController : Controller
    {
        private readonly AppDbContext _db;
        public ShopStocksController(AppDbContext db) { _db = db; }

        // GET: /Owner/ShopStocks
        public IActionResult Index()
        {
            // no reliance on navigation properties; join explicitly
            var rows = (from ss in _db.ShopStocks
                        join p in _db.Products on ss.ProductId equals p.Id
                        join s in _db.Shops on ss.ShopId equals s.Id
                        orderby s.City, p.Name
                        select new ShopStockVM
                        {
                            Shop = s.City + " (" + s.ShopCode + ")",
                            Product = p.Name,
                            Qty = ss.Qty,
                            Reorder = ss.ReorderLevel
                        })
                        .ToList();

            return View(rows);
        }

        // View model scoped to this controller (the view references it)
        public class ShopStockVM
        {
            public string Shop { get; set; } = "";
            public string Product { get; set; } = "";
            public int Qty { get; set; }
            public int Reorder { get; set; }
        }
    }
}


