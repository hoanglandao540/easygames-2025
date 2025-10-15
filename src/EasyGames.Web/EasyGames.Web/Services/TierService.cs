namespace EasyGames.Web.Services
{
    // student-style: simple thresholds
    // Silver >= 200, Gold >= 500, Platinum >= 1000
    public class TierService : ITierService
    {
        public TierLevel Evaluate(decimal lifetimeSpend)
        {
            if (lifetimeSpend >= 1000m) return TierLevel.Platinum;
            if (lifetimeSpend >= 500m) return TierLevel.Gold;
            if (lifetimeSpend >= 200m) return TierLevel.Silver;
            return TierLevel.Bronze;
        }

        public decimal NextThreshold(TierLevel current) => current switch
        {
            TierLevel.Bronze => 200m,
            TierLevel.Silver => 500m,
            TierLevel.Gold => 1000m,
            _ => 1000m // platinum is top
        };
    }
}
