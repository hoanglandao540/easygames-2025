using System.Collections.Generic;

namespace EasyGames.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }                 // PK
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        // optional nav
        public List<Order> Orders { get; set; } = new();
    }
}

