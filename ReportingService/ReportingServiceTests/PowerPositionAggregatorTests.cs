using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingService.Interfaces;
using TradingPlatform;

namespace ReportingService.Tests
{
    [TestClass()]
    public class PowerPositionAggregatorTests
    {
        [TestMethod()]
        public void AggregatePowerPositionByDateTest_Success()
        {
            var tradingPeriodList = new List<TradingPeriod[]>();
            for (var i = 0; i < 2; i++)
            {
                var periods = new TradingPeriod[3]
                {
                    new TradingPeriod()
                    {
                        Period = 1,
                        Volume = 1 + i
                    },
                    new TradingPeriod()
                    {
                        Period = 2,
                        Volume = 2 + i
                    },
                    new TradingPeriod()
                    {
                        Period = 3,
                        Volume = 3 + i
                    },
                };
                tradingPeriodList.Add(periods);
            }

            var expected = new List<IPowerPositionInfo>
            {
                new PowerPositionInfo()
                {
                    LocalTime = "06:00:00",
                    Volume = 3
                },
                new PowerPositionInfo()
                {
                    LocalTime = "07:00:00",
                    Volume = 5
                },
                new PowerPositionInfo()
                {
                    LocalTime = "08:00:00",
                    Volume = 7
                }
            };

            var powerPositionAggregator = new PowerPositionAggregator();
            var actual = powerPositionAggregator.AggregatePowerPositionByDate(tradingPeriodList);

            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}