using System;
using System.Collections.Generic;
using System.Linq;
using ReportingService.Interfaces;
using TradingPlatform;

namespace ReportingService
{
    public class PowerPositionAggregator : IPowerPositionAggregator
    {
        public List<IPowerPositionInfo> AggregatePowerPositionByDate(IEnumerable<TradingPeriod[]> periodsList)
        {
            try
            {
                var volumes = new double[24];

                volumes = periodsList.Aggregate(volumes,
                (aggregatedVolume, periods) => aggregatedVolume
                    .Zip(periods, (volume, period) => volume + period.Volume).ToArray());

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
