using System.Text.Json;
using EasyGames.Web.ViewModels;
using Microsoft.AspNetCore.Http;

namespace EasyGames.Web.Services
{
    // student-style: POS cart in Session (separate from storefront cart)
    public class PosCartService : IPosCartService
    {
        private readonly ISession _session;
        private const string Key = "EG_POS_CART";

        public PosCartService(IHttpContextAccessor accessor)
        {
            _session = accessor.HttpContext!.Session;
        }

        public PosCartVM Get()
        {
            var json = _session.GetString(Key);
            return string.IsNullOrEmpty(json) ? new PosCartVM()
                : (JsonSerializer.Deserialize<PosCartVM>(json) ?? new PosCartVM());
        }

        private void Save(PosCartVM vm)
            => _session.SetString(Key, JsonSerializer.Serialize(vm));

        public void Add(int productId, string name, decimal price, int qty)
        {
            var vm = Get();
            var row = vm.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row == null)
                vm.Rows.Add(new PosCartRowVM { ProductId = productId, Name = name, Price = price, Qty = qty });
            else
                row.Qty += qty;
            vm.Recalc(); Save(vm);
        }

        public void Inc(int productId)
        {
            var vm = Get();
            var row = vm.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row != null) row.Qty++;
            vm.Recalc(); Save(vm);
        }

        public void Dec(int productId)
        {
            var vm = Get();
            var row = vm.Rows.FirstOrDefault(r => r.ProductId == productId);
            if (row != null)
            {
                row.Qty--;
                if (row.Qty <= 0) vm.Rows.Remove(row);
            }
            vm.Recalc(); Save(vm);
        }

        public void Remove(int productId)
        {
            var vm = Get();
            vm.Rows.RemoveAll(r => r.ProductId == productId);
            vm.Recalc(); Save(vm);
        }

        public void Clear() => Save(new PosCartVM());
    }
}

