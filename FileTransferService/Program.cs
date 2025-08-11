using System.ServiceProcess;

namespace ExamTwo.FileTransferService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new FileTransferService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
