using EasyGames.Web.ViewModels;

namespace EasyGames.Web.Services
{
    public interface ICartService
    {
        CartVM Get();
        void Add(int productId, string name, decimal price, int qty = 1);
        void Inc(int productId);
        void Dec(int productId);
        void Remove(int productId);
        void Clear();
    }
}
