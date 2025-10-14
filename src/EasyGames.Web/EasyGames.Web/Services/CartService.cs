using System.Text.Json;
using EasyGames.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EasyGames.Web.Services
{
    // student-style: simple session-based cart
    public class CartService : ICartService
    {
        private readonly ISession _session;
        private const string Key = "EG_CART";

        public CartService(IHttpContextAccessor accessor)
        {
            _session = accessor.HttpContext!.Session;
        }

        public CartVM Get()
        {
            var json = _session.GetString(Key);
            return string.IsNullOrEmpty(json)
                ? new CartVM()
                : JsonSerializer.Deserialize<CartVM>(json) ?? new CartVM();
        }

        private void Save(CartVM vm)
            => _session.SetString(Key, JsonSerializer.Serialize(vm));

        public void Add(int productId, string name, decimal price, int qty = 1)
        {
            var cart = Get();
            var row = cart.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row == null)
                cart.Rows.Add(new CartRowVM { ProductId = productId, Name = name, Price = price, Qty = qty });
            else
                row.Qty += qty;
            cart.Recalc();
            Save(cart);
        }

        public void Inc(int productId)
        {
            var cart = Get();
            var row = cart.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row != null) row.Qty++;
            cart.Recalc(); Save(cart);
        }

        public void Dec(int productId)
        {
            var cart = Get();
            var row = cart.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row != null)
            {
                row.Qty--;
                if (row.Qty <= 0) cart.Rows.Remove(row);
            }
            cart.Recalc(); Save(cart);
        }

        public void Remove(int productId)
        {
            var cart = Get();
            cart.Rows.RemoveAll(r => r.ProductId == productId);
            cart.Recalc(); Save(cart);
        }

        public void Clear()
        {
            Save(new CartVM());
        }
    }
}
