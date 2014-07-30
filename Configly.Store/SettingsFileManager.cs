using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Configly.Store.Entities;
using Configly.Store.Interfaces;

namespace Configly.Store
{
    public class SettingsFileManager<T> : ISettingsManager<T> where T : class, new()
    {
        private readonly ILogger _logger;
        private readonly string _name;
        private readonly string _scope;

        public SettingsFileManager(string name, string scope, ILogger logger)
        {
            _name = name;
            _scope = scope;
            _logger = logger;
        }

        public T Get()
        {
            string backupFile = GetBackupFilePath();
            string jsonValue = File.ReadAllText(backupFile);

            var typedSettings = JsonConvert.DeserializeObject<T>(jsonValue);

            string message =
                string.Format("Settings have been retreived on the client from the backup file - [{0},{1}].", _name,
                    _scope);
            _logger.Information(message, new {TypedSettings = typedSettings, BackupFileText = jsonValue});

            return typedSettings;
        }

        public void Set(object settings)
        {
            Task.Factory.StartNew(() =>
            {
                string backupFile = GetBackupFilePath();
                string jsonValue = JsonConvert.SerializeObject(settings);

                for (int numOfRetry = 3; numOfRetry > 0; numOfRetry--)
                {
                    if (!WriteToBackupFile(backupFile, jsonValue))
                    {
                        Thread.Sleep(500);
                    }
                    else
                    {
                        break;
                    }
                }
            });
        }

        private string GetBackupFilePath()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SettingsBackup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return Path.Combine(path, string.Format("{0}_{1}_settings.json", _name, _scope));
        }

        private bool WriteToBackupFile(string backupFile, string jsonValue)
        {
            bool isSucces = true;

            try
            {
                File.WriteAllText(backupFile, jsonValue);
            }
            catch (Exception ex)
            {
                isSucces = false;
                Trace.WriteLine(ex);
            }
            return isSucces;
        }
    }
}