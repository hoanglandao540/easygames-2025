using System.ComponentModel.DataAnnotations;

namespace EasyGames.Web.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        // NEW: Category field for filtering
        public string Category { get; set; } = "General"; // Books, Toys, Games, Accessories
    }
}