namespace EasyGames.Web.Models
{
    // Shop/store info with proprietor link
    public class ShopLocation
    {
        public int Id { get; set; }
        public string ShopCode { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone { get; set; } = "";

        // Link to the Shop proprietor user
        public int? ProprietorUserId { get; set; }
        public AppUser? Proprietor { get; set; }
    }
}