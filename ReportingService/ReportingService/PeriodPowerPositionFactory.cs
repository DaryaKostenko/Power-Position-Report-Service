using System;
using ReportingService.Interfaces;

namespace ReportingService
{
    public class PeriodPowerPositionFactory : IPeriodPowerPositionFactory
    {
        public IPowerPositionInfo GetPowerPositionInfo(int period, double volume)
        {
            return new PowerPositionInfo
            {
                LocalTime = TimeSpan.FromHours((period + 6) % 24).ToString(),
                Volume = volume
            };
        }
    }
}
