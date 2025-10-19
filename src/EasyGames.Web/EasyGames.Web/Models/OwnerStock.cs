using System.ComponentModel.DataAnnotations;

namespace EasyGames.Web.Models
{
    public class OwnerStock
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater")]
        public int Qty { get; set; }

        [Required(ErrorMessage = "Source is required")]
        [StringLength(200, ErrorMessage = "Source cannot exceed 200 characters")]
        public string Source { get; set; } = "";

        [Required(ErrorMessage = "Buy price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Buy price must be greater than 0")]
        public decimal BuyPrice { get; set; }

        [Required(ErrorMessage = "Sell price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Sell price must be greater than 0")]
        public decimal SellPrice { get; set; }

        // Navigation
        public Product? Product { get; set; }
    }
}