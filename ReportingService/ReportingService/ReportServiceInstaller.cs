using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService
{
    [RunInstaller(true)]
    public class ReportServiceInstaller : Installer
    {
        public ReportServiceInstaller()
        {
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = "ReportService";
            serviceInstaller.DisplayName = "Report Service";
            serviceInstaller.DelayedAutoStart = true;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.Add(serviceInstaller);

            var serviceProcessInstaller = new ServiceProcessInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;

            this.Installers.Add(serviceProcessInstaller);
        }
        
    }
}
