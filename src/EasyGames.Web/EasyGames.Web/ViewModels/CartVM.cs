namespace EasyGames.Web.ViewModels
{
    public class CartVM
    {
        public List<CartRowVM> Rows { get; set; } = new();
        public decimal GrandTotal { get; set; }

        public void Recalc() => GrandTotal = Rows.Sum(r => r.LineTotal);
    }
}
