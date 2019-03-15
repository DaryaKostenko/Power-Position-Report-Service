using System;

namespace ReportingService.Helpers
{
    public class ReportNameBuilder
    {
        public string CreateReportName(DateTime date)
        {
            return $"PowerPosition_{date.Date.Year}{date.Date.Month:D2}{date.Day:D2}_{date.Hour:D2}{date.Minute:D2}.csv";
        }
    }
}
