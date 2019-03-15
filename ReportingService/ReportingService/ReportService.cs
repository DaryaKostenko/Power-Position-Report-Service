using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using TradingPlatform;

namespace ReportingService
{
    public partial class ReportService : ServiceBase
    {
        private readonly Timer _timer;
        private readonly int _runningInterval;
        private readonly TimeZoneInfo _gmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        public ReportService()
        {
            InitializeComponent();
            _runningInterval = int.Parse(ConfigurationManager.AppSettings.Get("RunningIntervalInMinutes"));
            _timer = new Timer(WorkProcedure);
        }

        private void WorkProcedure(object target)
        {
            var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, _gmtTimeZoneInfo);
            var tradingPositionAnalyzer = new TradingPositionAnalyzer();
            tradingPositionAnalyzer.AnalyzePowerPositionByDate(date);
        }

        protected override void OnStart(string[] args)
        {
            _timer.Change(0, _runningInterval * 60000);
        }

        protected override void OnStop()
        {
            _timer.Change(Timeout.Infinite, 0);
        }
    }
}
