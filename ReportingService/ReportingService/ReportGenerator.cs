using System;
using System.IO;
using System.Linq;
using CsvHelper;
using TradingPlatform;

namespace ReportingService
{
    public static class ReportGenerator
    {
        private static readonly TimeZoneInfo GmtTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
        private static string _logName;
        public static void GenerateReport(string reportPath)
        {
            _logName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServiceProcessService.log");
            File.AppendAllText(_logName, DateTime.Now.ToLongTimeString() + " Start creating report...\n");
            var date = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, GmtTimeZoneInfo);
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
                File.AppendAllText(_logName, DateTime.Now.ToLongTimeString() + " Report successfuly created.\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText(_logName, DateTime.Now.ToLongTimeString() + " " + ex.Message);
                File.AppendAllText(_logName, DateTime.Now.ToLongTimeString() + " Could not create report.\n");
            }
        }

        private static string CheckDisplayNumber(int number)
        {
            return number > 9 ? number.ToString() : $"0{number}";
        }
    }
}
