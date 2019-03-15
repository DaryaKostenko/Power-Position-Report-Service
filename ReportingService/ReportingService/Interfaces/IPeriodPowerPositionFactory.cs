using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Interfaces
{
    public interface IPeriodPowerPositionFactory
    {
        IPowerPositionInfo GetPowerPositionInfo(int period, double volume);
    }
}
