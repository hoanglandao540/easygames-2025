using System;

namespace EasyGames.Web.Models
{
    // order header
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public decimal GrandTotal { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
