using System.ServiceProcess;

namespace MsmqListenerService
{
    static class Program
    {
        static void Main()
        {
            ServiceBase.Run(new ListenerService());
        }
    }
}
