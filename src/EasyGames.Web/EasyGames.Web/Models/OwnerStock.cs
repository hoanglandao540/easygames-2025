namespace EasyGames.Web.Models
{
    public class OwnerStock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Qty { get; set; }

        public string Source { get; set; } = "";
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
    }
}

