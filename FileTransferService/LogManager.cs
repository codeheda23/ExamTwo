using System;
using Serilog;
using Serilog.Core;

namespace ExamTwo.FileTransferService
{
    public static class LogManager
    {
        private static readonly Logger _logger;

        static LogManager()
        {
            string serviceName = "FileTransferService";

            _logger = new LoggerConfiguration()
                .MinimumLevel.Information()

                
                //Rolling File
                .WriteTo.File(path: $@"C:\Logs\{serviceName}\log-.txt",

                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")

                // Windows Event Viewer
                .WriteTo.EventLog(
                    source: serviceName,
                    manageEventSource: true)

                .CreateLogger();
        }

        public static void Info(string message) => _logger.Information(message);
        public static void Error(string message, Exception ex = null) => _logger.Error(ex, message);
        // we can add methods for Debug, Warning & Fatal if necessary but I will be using only Info and Error for this service.
        public static void Shutdown() => (_logger as IDisposable)?.Dispose();
    }
}
