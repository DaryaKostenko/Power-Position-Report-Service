using System;
using System.Collections.Generic;
using System.Linq;
using ReportingService.Interfaces;
using TradingPlatform;

namespace ReportingService
{
    public class PowerPositionAggregator : IPowerPositionAggregator
    {
        public List<IPowerPositionInfo> AggregatePowerPositionByDate(DateTime date, IEnumerable<Trade> trades)
        {
            try
            {
                var volumes = new double[24];
                volumes = trades.Aggregate(volumes,
                    (aggregatedVolume, trade) => aggregatedVolume
                        .Zip(trade.Periods, (volume, period) => volume + period.Volume).ToArray());

                var periodPowerPositionFactory = new PeriodPowerPositionFactory();
                return volumes.Select((volume, i) => periodPowerPositionFactory.GetPowerPositionInfo(i, volume)).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
