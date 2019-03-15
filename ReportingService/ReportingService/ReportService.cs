using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace ReportingService
{
    public partial class ReportService : ServiceBase
    {
        private readonly Timer _timer;
        private readonly int _runningInterval;
        private readonly TimeZoneInfo _gmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        private readonly ReportGenerator _reportGenerator;

        public ReportService()
        {
            InitializeComponent();
            _runningInterval = int.Parse(ConfigurationManager.AppSettings.Get("RunningIntervalInMinutes"));
            var reportLocation = ConfigurationManager.AppSettings.Get("ReportsLocation");
            _reportGenerator = new ReportGenerator(reportLocation, _gmtTimeZoneInfo);
            _timer = new Timer(WorkProcedure);
        }

        private void WorkProcedure(object target)
        {
            _reportGenerator.GenerateReport();
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
