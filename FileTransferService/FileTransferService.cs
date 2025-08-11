using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace ExamTwo.FileTransferService
{
    public partial class FileTransferService : ServiceBase
    {
        //If in a production environment, it's best to store and configure the hardcoded values below in an environment variable or app.Config

        private const string SourceDir = @"C:\Folder1";
        private const string DestinationDir = @"C:\Folder2";
        private FileSystemWatcher _fsWatcher;

        public FileTransferService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Directory.CreateDirectory(SourceDir);
                Directory.CreateDirectory(DestinationDir);

                _fsWatcher = new FileSystemWatcher(SourceDir)
                {
                    NotifyFilter = NotifyFilters.FileName,
                    Filter = "*.*",
                    EnableRaisingEvents = true
                };

                _fsWatcher.Created += OnFileCreated;

                LogManager.Info($"Service Started. Monitoring: {SourceDir}");
            }
            catch (Exception ex)
            {
                LogManager.Error("Fatal error during service startup.", ex);
                this.Stop();
            }
        }

        protected override void OnStop()
        {
            if (_fsWatcher != null)
            {
                _fsWatcher.EnableRaisingEvents = false;
                _fsWatcher.Dispose();
            }
            LogManager.Info("Service Stopped.");
            LogManager.Shutdown();
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(500);

                string destinationPath = Path.Combine(DestinationDir, e.Name);
                File.Move(e.FullPath, destinationPath);

                LogManager.Info($"Transferred file: '{e.Name}' to '{DestinationDir}'.");
            }
            catch (Exception ex)
            {
                LogManager.Error($"Error transferring file '{e.Name}'.", ex);
            }
        }


    }
}
