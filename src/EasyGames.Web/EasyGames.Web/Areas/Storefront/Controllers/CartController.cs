using EasyGames.Web.Data;
using EasyGames.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyGames.Web.Areas.Storefront.Controllers
{
    [Area("Storefront")]
    public class CartController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ICartService _cart;

        public CartController(AppDbContext db, ICartService cart)
        {
            _db = db; _cart = cart;
        }

        public IActionResult Index() => View(_cart.Get());

        // Add 1 of product; stay on caller if returnUrl provided (Catalog)
        public IActionResult Add(int id, string? returnUrl)
        {
            var p = _db.Products.Find(id);
            if (p != null)
            {
                _cart.Add(p.Id, p.Name, p.Price, 1);
                TempData["toast"] = $"{p.Name} added to cart.";
            }
            return !string.IsNullOrWhiteSpace(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction(nameof(Index));
        }

        // Increment by 1
        public IActionResult Inc(int id, string? returnUrl)
        {
            _cart.Inc(id);
            return !string.IsNullOrWhiteSpace(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction(nameof(Index));
        }

        // Decrement by 1 (removes row at 0)
        public IActionResult Dec(int id, string? returnUrl)
        {
            _cart.Dec(id);
            return !string.IsNullOrWhiteSpace(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction(nameof(Index));
        }

        // Remove product row
        public IActionResult Remove(int id, string? returnUrl)
        {
            _cart.Remove(id);
            return !string.IsNullOrWhiteSpace(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction(nameof(Index));
        }

        // Clear entire cart
        public IActionResult Clear(string? returnUrl)
        {
            _cart.Clear();
            return !string.IsNullOrWhiteSpace(returnUrl)
                ? Redirect(returnUrl!)
                : RedirectToAction(nameof(Index));
        }
    }
}
