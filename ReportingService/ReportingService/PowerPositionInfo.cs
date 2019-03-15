using ReportingService.Interfaces;

namespace ReportingService
{
    public class PowerPositionInfo : IPowerPositionInfo
    {
        public string LocalTime { get; set; }
        public double Volume { get; set; }
    }
}
