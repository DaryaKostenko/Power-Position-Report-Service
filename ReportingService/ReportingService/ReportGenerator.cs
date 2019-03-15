using System;
using System.IO;
using System.Linq;
using CsvHelper;
using NLog;
using ReportingService.Helpers;
using ReportingService.Interfaces;
using TradingPlatform;

namespace ReportingService
{
    public class ReportGenerator : IReportGenerator
    {
        private readonly TimeZoneInfo _gmtTimeZoneInfo;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _reportLocation;

        public ReportGenerator(string reportLocation, TimeZoneInfo gmtTimeZoneInfo)
        {
            _reportLocation = reportLocation;
            _gmtTimeZoneInfo = gmtTimeZoneInfo;
        }

        public void GenerateReport()
        {
            _logger.Info("Start creating report...");
            var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, _gmtTimeZoneInfo);
            var tradingService = new TradingService();
            try
            {
                var trades = tradingService.GetTrades(date);
                var tradesArray = trades as Trade[] ?? trades.ToArray();
                var volumes = new double[24];
                volumes = tradesArray.Aggregate(volumes,
                    (aggregatedVolume, trade) => aggregatedVolume
                        .Zip(trade.Periods, (volume, period) => volume + period.Volume).ToArray());
                var reportInfoList = volumes.Select((volume, i) => new PowerPositionInfo
                {
                    LocalTime = TimeSpan.FromHours((i + 6) % 24).ToString(),
                    Volume = volume
                }).ToList();

                var createReportNameHelper = new CreateReportNameHelper(date);
                var reportName = createReportNameHelper.CreateReportName();
                   
                using (var writer = new StreamWriter(Path.Combine(_reportLocation, reportName)))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(reportInfoList);
                }
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
