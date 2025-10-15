namespace EasyGames.Web.Services
{
    public interface ITierService
    {
        TierLevel Evaluate(decimal lifetimeSpend);
        decimal NextThreshold(TierLevel current);
    }
}
