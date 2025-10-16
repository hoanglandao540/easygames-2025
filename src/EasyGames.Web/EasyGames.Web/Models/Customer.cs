using System.Collections.Generic;

namespace EasyGames.Web.Models
{
    public class Customer
    {
        public int Id { get; set; }                 // PK
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
<<<<<<< HEAD

        // Link to AppUser for registered customers
        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}
=======
        // optional nav
        public List<Order> Orders { get; set; } = new();
    }
}

>>>>>>> feature/akshata/data-shops
