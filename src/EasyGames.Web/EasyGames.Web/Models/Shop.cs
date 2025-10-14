namespace EasyGames.Web.Models
{
    //  a shop/store info
    public class Shop
    {
        public int Id { get; set; }
        public string ShopCode { get; set; } = "";
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Phone { get; set; } = "";
    }
}
