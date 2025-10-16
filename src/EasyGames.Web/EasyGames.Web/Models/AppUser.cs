namespace EasyGames.Web.Models
{
    // simple roles mapped to Areas
    public enum AppRole
    {
        Owner = 0,
        Shop = 1,
        Customer = 2
    }

<<<<<<< HEAD
=======

>>>>>>> feature/akshata/data-shops
    public class AppUser
    {
        public int Id { get; set; }              // PK
        public string Name { get; set; } = "";   // display name
        public string Email { get; set; } = "";  // login username
<<<<<<< HEAD
        public string Phone { get; set; } = "";  // for customer order tracking
        public string PasswordHash { get; set; } = "";
        public AppRole Role { get; set; }        // role per Area
    }
}
=======
        public string PasswordHash { get; set; } = ""; 
        public AppRole Role { get; set; }        // role per Area
    }
}





>>>>>>> feature/akshata/data-shops
