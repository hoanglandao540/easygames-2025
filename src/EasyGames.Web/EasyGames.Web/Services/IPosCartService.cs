using EasyGames.Web.ViewModels;

namespace EasyGames.Web.Services
{
    public interface IPosCartService
    {
        PosCartVM Get();
        void Add(int productId, string name, decimal price, int qty);
        void Inc(int productId);
        void Dec(int productId);
        void Remove(int productId);
        void Clear();
    }
}
