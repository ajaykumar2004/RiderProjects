using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

[RunInstaller(true)]
public class ProjectInstaller : Installer
{
    private ServiceProcessInstaller processInstaller;
    private ServiceInstaller serviceInstaller;

    public ProjectInstaller()
    {
        processInstaller = new ServiceProcessInstaller
        {
            Account = ServiceAccount.LocalSystem
        };

        serviceInstaller = new ServiceInstaller
        {
            ServiceName = "MSMQListenerService",
            DisplayName = "MSMQ Listener Windows Service",
            Description = "Listens to MSMQ and logs messages to Event Viewer",
            StartType = ServiceStartMode.Automatic
        };

        Installers.Add(processInstaller);
        Installers.Add(serviceInstaller);
    }
}