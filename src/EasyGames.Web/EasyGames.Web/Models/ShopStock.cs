namespace EasyGames.Web.Models
{
    public class ShopStock
    {
        public int Id { get; set; }

        public int ShopId { get; set; }
        public ShopLocation? Shop { get; set; }  // ← CHANGED

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Qty { get; set; }
        public int ReorderLevel { get; set; } = 3;

        // Inherited from OwnerStock when transferred
        public string Source { get; set; } = "";
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
    }
}