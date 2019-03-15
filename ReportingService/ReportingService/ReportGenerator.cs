using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using ReportingService.Helpers;
using ReportingService.Interfaces;

namespace ReportingService
{
    public class ReportGenerator : IReportGenerator
    {
       
        private readonly string _reportLocation;

        public ReportGenerator(string reportLocation)
        {
            _reportLocation = reportLocation;
        }

        public void GenerateReportByDate(List<IPowerPositionInfo> reportInfoList, DateTime date)
        {
            
            try
            {
                var createReportNameHelper = new ReportNameBuilder();
                var reportName = createReportNameHelper.CreateReportName(date);
                   
                using (var writer = new StreamWriter(Path.Combine(_reportLocation, reportName)))
                using (var csv = new CsvWriter(writer))
                {
                    csv.WriteRecords(reportInfoList);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
