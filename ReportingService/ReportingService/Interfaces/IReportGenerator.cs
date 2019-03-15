using System;
using System.Collections.Generic;

namespace ReportingService.Interfaces
{
    public interface IReportGenerator
    {
        void GenerateReportByDate(List<IPowerPositionInfo> reportInfoList, DateTime date);
    }
}
