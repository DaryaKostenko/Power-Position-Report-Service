using System.Configuration;
using System.ServiceProcess;

using System.Threading;

namespace ReportingService
{
    public partial class ReportService : ServiceBase
    {
        private readonly Timer _timer;
        private readonly int _runningInterval;
        private readonly string _reportLocation;

        public ReportService()
        {
            InitializeComponent();
            _timer = new Timer(WorkProcedure);
            _runningInterval = int.Parse(ConfigurationManager.AppSettings.Get("RunningIntervalInMinutes"));
            _reportLocation = ConfigurationManager.AppSettings.Get("ReportsLocation");
        }

        private void WorkProcedure(object target)
        {
            var reportGenerator = new ReportGenerator();
            reportGenerator.GenerateReport(_reportLocation);
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
