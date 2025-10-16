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
<<<<<<< HEAD
=======
>>>>>>> origin/feature/hoang/pos-tier-email

        // Link to AppUser for registered customers
        public int? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

<<<<<<< HEAD
        public List<Order> Orders { get; set; } = new();
    }
}
=======
        // optional nav
        public List<Order> Orders { get; set; } = new();
    }
}

>>>>>>> feature/akshata/data-shops
=======
        public List<Order> Orders { get; set; } = new();
    }
}
>>>>>>> origin/feature/hoang/pos-tier-email
