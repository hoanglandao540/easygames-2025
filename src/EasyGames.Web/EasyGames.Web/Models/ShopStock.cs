namespace EasyGames.Web.Models
{
    public class ShopStock
    {
        public int Id { get; set; }

        public int ShopId { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
        public ShopLocation? Shop { get; set; }  // ← CHANGED
=======
        public Shop? Shop { get; set; }
>>>>>>> feature/akshata/data-shops
=======
        public ShopLocation? Shop { get; set; }  // ← CHANGED
>>>>>>> origin/feature/hoang/pos-tier-email

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Qty { get; set; }
        public int ReorderLevel { get; set; } = 3;

        // Inherited from OwnerStock when transferred
        public string Source { get; set; } = "";
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
    }
<<<<<<< HEAD
<<<<<<< HEAD
}
=======
}
>>>>>>> feature/akshata/data-shops
=======
}
>>>>>>> origin/feature/hoang/pos-tier-email
