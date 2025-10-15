namespace EasyGames.Web.ViewModels
{
    public class PosCartVM
    {
        public List<PosCartRowVM> Rows { get; set; } = new();
        public decimal GrandTotal { get; set; }
        public void Recalc() => GrandTotal = Rows.Sum(r => r.LineTotal);
    }
}
