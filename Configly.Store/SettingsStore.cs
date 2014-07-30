using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Configly.Entities;
using System.Reflection;
using System.Diagnostics;
using Configly.Store.Entities;
using Configly.Store.Interfaces;

namespace Configly.Store
{
    public class SettingsStore<T> : ISettingsStore<T> where T : class, new()
    {
        private SettingsStoreConfiguration _config;
        private ISettingsServerProxy _settingsProxy;
        private ISettingsManager<T> _settingsFileManager;
        private ILogger _logger;
        private T _typedSettings;
        private string _settingsName;

        public event EventHandler<SettingsRefreshArgs> OnRefresh;

        public SettingsStore(SettingsStoreConfiguration config)
        {
            _logger = new TraceLogger();
            Init(config);
        }

        public SettingsStore(SettingsStoreConfiguration config, ILogger logger)
        {
            _logger = logger;
            Init(config);
        }


        //private void InitWithCache(SettingsStoreConfiguration config)
        //{
        //    if (File.Exists(""))
        //    {
        //        //Load cache
        //        InitializeProxy();
        //        return;
        //    }
        //    else
        //    {
        //        InitializeProxy().Wait();
        //        return;
        //    }
        //}

        private void Init(SettingsStoreConfiguration config)
        {
            _typedSettings = new T();
            _config = config;
            _settingsName = typeof(T).FullName;
            _settingsProxy = new SettingsServerProxy(_settingsName, config, _logger);
            _settingsProxy.OnReconnection += settingsProxy_OnReconnection;
            _settingsProxy.OnRefresh += settingsProxy_OnRefresh;
            _settingsFileManager = new SettingsFileManager<T>(_settingsName, _config.Scope, _logger);

            string message = string.Format("Settings Store has been initialized - [{0},{1}].", _settingsName, _config.Scope);
            _logger.Information(message, new { TypedSettings = _typedSettings, StoreConfig = _config });

        }

        public SettingsStore(SettingsStoreConfiguration config, ISettingsServerProxy settingsProxy, ISettingsManager<T> settingsFileManager, ILogger logger)
        {
            _typedSettings = new T();
            _config = config;
            _settingsName = typeof(T).FullName;
            _settingsProxy = settingsProxy;
            _settingsFileManager = settingsFileManager;
            _logger = logger;
        }

        public T Get()
        {
            string message = string.Format("Attempting to retrieve Settings from the server - [{0},{1}].", _settingsName, _config.Scope);
            _logger.Information(message, new { TypedSettings = _typedSettings, StoreConfig = _config });

            _typedSettings = GetSettings();

            _settingsFileManager.Set(_typedSettings);

             message = string.Format("Settings have been retreived on the client - [{0},{1}].", _settingsName, _config.Scope);
            _logger.Information(message, new { TypedSettings = _typedSettings, StoreConfig = _config });

            return _typedSettings;
        }

        private T GetSettings()
        {
            Setting defaultSetting = CreateDefaultSettingWithProperties();

            if (!_settingsProxy.IsConnectedToServer())
            {
                return _settingsFileManager.Get();
            }
            else
            {
                Setting settingFromServer = _settingsProxy.GetAndUpdate(defaultSetting);

                FillInTypedSettings(_typedSettings, settingFromServer.Properties);

                return _typedSettings;
            }
        }

        private Setting CreateDefaultSettingWithProperties()
        {
            var newHandler = new DefaultSettingsHandler<T>();
            var properties = newHandler.CreateDefaultSettingProperties();

            var setting = new Setting()
            {
                Name = _settingsName,
                Properties = properties
            };
            return setting;
        }

        private void FillInTypedSettings(T typedSettings, IEnumerable<Property> propertiesFromServer)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var setting = propertiesFromServer.FirstOrDefault(s => s.Key == prop.Name);
                if (setting != null)
                {
                    try
                    {
                        prop.SetValue(typedSettings, Convert.ChangeType(setting.Value, prop.PropertyType));
                    }
                    catch (Exception ex)
                    {
                        string message = string.Format("An error while setting the value [{0}] in the property [{1},{2}] of the [{3}] class.", setting.Value, prop.Name, prop.PropertyType, _settingsName);
                        _logger.Error(message, ex);
                    }
                }
            }
        }

        private void settingsProxy_OnRefresh(object sender, Setting settings)
        {
            FillInTypedSettings(_typedSettings, settings.Properties);
            _settingsFileManager.Set(_typedSettings);

            if (OnRefresh != null)
            {
                OnRefresh(this, new SettingsRefreshArgs(_typedSettings));
            }

            string message = string.Format("Settings have been refreshed on the client - [{0},{1}].", _settingsName, _config.Scope);
            _logger.Information(message, new { SettingsFromServer = settings, TypedSettings = _typedSettings, StoreConfig = _config });
        }

        private void settingsProxy_OnReconnection(object sender, EventArgs e)
        {
            // TODO: Get latest settings when reconnect with server. GetSettings didn't work. It killed the connection
            //GetSettings();
        }
    }
}