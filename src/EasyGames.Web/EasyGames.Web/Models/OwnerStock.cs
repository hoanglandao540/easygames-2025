namespace EasyGames.Web.Models
{
    public class OwnerStock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Qty { get; set; }

        // tracking required by brief
        public string Source { get; set; } = "";     // e.g. Distributor name, supplier batch
        public decimal BuyPrice { get; set; }        // owner’s cost
        public decimal SellPrice { get; set; }       // owner’s intended sell price (web)
    }
}
