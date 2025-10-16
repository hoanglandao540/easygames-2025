namespace EasyGames.Web.Models
{
    //  how many of a product at a shop
    public class ShopStock
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int ReorderLevel { get; set; } = 3;
        public string Source { get; set; } = "";
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }  

    }
}
