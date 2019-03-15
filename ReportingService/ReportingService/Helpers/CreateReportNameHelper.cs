using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Helpers
{
    public class CreateReportNameHelper
    {
        public DateTime CreationReportDate { get; }

        public CreateReportNameHelper(DateTime date)
        {
            CreationReportDate = date;
        }

        public string CreateReportName()
        {
            return $"PowerPosition_{CreationReportDate.Date.Year}{CheckDisplayNumber(CreationReportDate.Date.Month)}{CheckDisplayNumber(CreationReportDate.Day)}_{CheckDisplayNumber(CreationReportDate.Hour)}{CheckDisplayNumber(CreationReportDate.Minute)}.csv";
        }

        private string CheckDisplayNumber(int number)
        {
            return number > 9 ? number.ToString() : $"0{number}";
        }
    }
}
