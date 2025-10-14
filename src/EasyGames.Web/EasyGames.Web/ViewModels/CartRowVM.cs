namespace EasyGames.Web.ViewModels
{
    public class CartRowVM
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal LineTotal => Price * Qty;
    }
}
