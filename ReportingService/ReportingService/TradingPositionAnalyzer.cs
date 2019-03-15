using System;
using System.Configuration;
using System.Linq;
using NLog;
using TradingPlatform;

namespace ReportingService
{
    public class TradingPositionAnalyzer
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ReportGenerator _reportGenerator;
        public TradingPositionAnalyzer()
        {
            var reportLocation = ConfigurationManager.AppSettings.Get("ReportsLocation");
            _reportGenerator = new ReportGenerator(reportLocation);
        }
        public void AnalyzePowerPositionByDate(DateTime date)
        {
            var tradingService = new TradingService();
            try
            {
                var trades = tradingService.GetTrades(date);
                var tradesArray = trades as Trade[] ?? trades.ToArray();
                var periodsList = tradesArray.Select(trade => trade.Periods).ToList();
                var powerPositionAggregator = new PowerPositionAggregator();
                var reportInfoList = powerPositionAggregator.AggregatePowerPositionByDate(periodsList);
                _logger.Info("Start creating report...");
                _reportGenerator.GenerateReportByDate(reportInfoList, date);
                _logger.Info("Report successfuly created.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Info("Could not create report.");
            }
        }
    }
}
