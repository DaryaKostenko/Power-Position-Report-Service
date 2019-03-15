using System;
using System.IO;
using System.Linq;
using CsvHelper;
using NLog;
using TradingPlatform;

namespace ReportingService
{
    public class ReportGenerator
    {
        private readonly TimeZoneInfo _gmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public void GenerateReport(string reportPath)
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

                var reportName =
                    $"PowerPosition_{date.Date.Year}{CheckDisplayNumber(date.Date.Month)}{CheckDisplayNumber(date.Day)}_{CheckDisplayNumber(date.Hour)}{CheckDisplayNumber(date.Minute)}.csv";
                using (var writer = new StreamWriter(Path.Combine(reportPath, reportName)))
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

        private string CheckDisplayNumber(int number)
        {
            return number > 9 ? number.ToString() : $"0{number}";
        }
    }
}
