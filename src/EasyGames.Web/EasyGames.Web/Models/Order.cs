using System.ComponentModel.DataAnnotations.Schema;

namespace EasyGames.Web.Models
{
    public class Order
    {
        public int Id { get; set; }

        // where the sale happened
        public int ShopId { get; set; }
        public ShopLocation? Shop { get; set; }  // ← CHANGED

        // who bought (nullable for guest sale)
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // captured at POS (works for guest too)
        public string? CustomerPhone { get; set; }

        // canonical financials & time
        public decimal Total { get; set; }
        public DateTime CreatedUtc { get; set; }

        public List<OrderLine> Lines { get; set; } = new();

        // These keep older code compiling (don't touch DB schema).
        [NotMapped] public string? CustomerName { get; set; }
        [NotMapped] public string? CustomerEmail { get; set; }

        [NotMapped]
        public DateTime CreatedAt
        {
            get => CreatedUtc;
            set => CreatedUtc = value;
        }

        [NotMapped]
        public decimal GrandTotal
        {
            get => Total;
            set => Total = value;
        }
    }
}