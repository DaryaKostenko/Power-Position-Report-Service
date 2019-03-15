using System;
using System.Collections.Generic;
using TradingPlatform;

namespace ReportingService.Interfaces
{
    public interface IPowerPositionAggregator
    {
        List<IPowerPositionInfo> AggregatePowerPositionByDate(IEnumerable<TradingPeriod[]> periodsList);
    }
}
