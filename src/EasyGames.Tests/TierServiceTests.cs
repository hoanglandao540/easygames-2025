using EasyGames.Web.Services;
using FluentAssertions;
using Xunit;

namespace EasyGames.Tests
{
    public class TierServiceTests
    {
        [Theory]
        [InlineData(0, TierLevel.Bronze)]
        [InlineData(199.99, TierLevel.Bronze)]
        [InlineData(200, TierLevel.Silver)]
        [InlineData(499.99, TierLevel.Silver)]
        [InlineData(500, TierLevel.Gold)]
        [InlineData(999.99, TierLevel.Gold)]
        [InlineData(1000, TierLevel.Platinum)]
        [InlineData(5000, TierLevel.Platinum)]
        public void Evaluate_ReturnsExpectedTier(decimal spend, TierLevel expected)
        {
            var svc = new TierService();
            svc.Evaluate(spend).Should().Be(expected);
        }

        [Theory]
        [InlineData(TierLevel.Bronze, 200)]
        [InlineData(TierLevel.Silver, 500)]
        [InlineData(TierLevel.Gold, 1000)]
        [InlineData(TierLevel.Platinum, 1000)]
        public void NextThreshold_ReturnsExpected(TierLevel level, decimal expected)
        {
            var svc = new TierService();
            svc.NextThreshold(level).Should().Be(expected);
        }
    }
}
