using EasyGames.Web.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EasyGames.Web.Services
{
    public interface IInventoryService
    {
        Task IncreaseAsync(int shopId, int productId, int qty);
        Task DecreaseAsync(int shopId, int productId, int qty);
        Task<List<StockRowVM>> GetStockAsync(int shopId);
    }
}
