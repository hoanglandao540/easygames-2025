namespace EasyGames.Web.ViewModels
{
    // simple stock row for pages
    public class StockRowVM
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public int Qty { get; set; }
        public int ReorderLevel { get; set; }
    }
}
