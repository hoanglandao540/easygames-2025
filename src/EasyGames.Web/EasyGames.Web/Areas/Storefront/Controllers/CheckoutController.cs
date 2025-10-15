using EasyGames.Web.Data;
using EasyGames.Web.Models;
using EasyGames.Web.Services;
using EasyGames.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    [Authorize(Roles = nameof(AppRole.Customer))]

    public class CheckoutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ICartService _cart;
        public CheckoutController(AppDbContext db, ICartService cart) { _db = db; _cart = cart; }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Cart = _cart.Get();
            return View(new CheckoutVM());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutVM vm)
        {
            if (!ModelState.IsValid) { ViewBag.Cart = _cart.Get(); return View(vm); }

            var cart = _cart.Get();
            if (!cart.Rows.Any()) { TempData["msg"] = "Cart is empty."; return RedirectToAction("Index", "Catalog"); }

            var order = new Order
            {
                CustomerName = vm.CustomerName,
                CustomerEmail = vm.CustomerEmail,
                CreatedAt = DateTime.UtcNow,
                GrandTotal = cart.GrandTotal
            };
            _db.Orders.Add(order);
            _db.SaveChanges();

            foreach (var r in cart.Rows)
            {
                _db.OrderLines.Add(new OrderLine
                {
                    OrderId = order.Id,
                    ProductId = r.ProductId,
                    Name = r.Name,
                    Price = r.Price,
                    Qty = r.Qty
                });
            }
            _db.SaveChanges();

            _cart.Clear();
            return RedirectToAction(nameof(Success), new { id = order.Id });
        }

        public IActionResult Success(int id) => View(id);
    }
}
