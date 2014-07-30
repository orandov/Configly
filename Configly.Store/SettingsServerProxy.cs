using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Configly.Entities;
using System.Net;
using System.Net.Sockets;
using Configly.Entities.Logging;
using Configly.Store.Entities;
using Configly.Store.Interfaces;

namespace Configly.Store
{
    public class SettingsServerProxy : ISettingsServerProxy
    {
        private IHubProxy _settingsHub;
        private HubConnection _hubConnection;
        private SettingsStoreConfiguration _config;
        private ILogger _logger;
        private string _settingsName;

        public event EventHandler<Setting> OnRefresh;
        public event EventHandler OnReconnection;

        public SettingsServerProxy(string settingsName, SettingsStoreConfiguration config, ILogger logger)
        {
            _config = config;
            _settingsName = settingsName;
            _logger = logger;
            InitializeConnection();
        }

        public Setting GetAndUpdate(Setting setting)
        {
            return _settingsHub.Invoke<Setting>("GetAndUpdate", setting).Result;
        }

        public bool IsConnectedToServer()
        {
            return _hubConnection.State == ConnectionState.Connected;
        }

        private void InitializeConnection()
        {
            SetupHubConnection();
            CreateHubProxy();
            StartHubConnection();
        }

        private void StartHubConnection()
        {
            try
            {
                _hubConnection.Start().Wait();

                _logger.Information(string.Format("Settings client has connected to the server - [{0},{1}]", _settingsName, _config.Scope), new { StoreConfig = _config, QueryString = _hubConnection.QueryString });
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("An error occured in the client while connecting to the server - [{0},{1}]", _settingsName, _config.Scope), new { Exception = ex, StoreConfig = _config, QueryString = _hubConnection.QueryString });
            }
        }

        private void CreateHubProxy()
        {
            _settingsHub = _hubConnection.CreateHubProxy("settings");

            _settingsHub.On<Setting>("SettingsRefreshed", (setting) =>
            {
                if (OnRefresh != null)
                {
                    OnRefresh(this, setting);
                }
            });
        }

        private void SetupHubConnection()
        {
            Dictionary<string, string> queryString = PrepareQueryString();

            _hubConnection = new HubConnection(_config.ServerUrl, queryString);
            _hubConnection.Closed += hubConnection_Closed;
            _hubConnection.Error += hubConnection_Error;
            _hubConnection.Reconnected += hubConnection_Reconnected;
            _hubConnection.Reconnecting += hubConnection_Reconnecting;
            _hubConnection.StateChanged += hubConnection_StateChanged;

            _hubConnection.TraceLevel = TraceLevels.Messages | TraceLevels.Events;
            var eventLog = new EventLog { Source = "ConfiglyClientLog" };
            _hubConnection.TraceWriter = new EventLogTraceWriter(eventLog);
        }

        private Dictionary<string, string> PrepareQueryString()
        {
            string machineName = Dns.GetHostName();
            string ipAdress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(t => t.AddressFamily == AddressFamily.InterNetwork).ToString();

            var queryString = new Dictionary<string, string>();
            queryString.Add("tname", _settingsName);
            queryString.Add("scope", _config.Scope);
            queryString.Add("machineName", machineName);
            queryString.Add("ip", ipAdress);
            return queryString;
        }

        #region SignalR Event Handlers

        void hubConnection_StateChanged(StateChange obj)
        {
            _logger.Information(string.Format("Settings client state has changed from [{0}] to [{1}] - [{2},{3}]", obj.OldState, obj.NewState, _settingsName, _config.Scope), new { StoreConfig = _config });
        }

        void hubConnection_Reconnecting()
        {
            _logger.Information(string.Format("Settings client is trying to reconnect to the server - [{0},{1}]", _settingsName, _config.Scope), new { StoreConfig = _config });
        }


        void hubConnection_Error(Exception ex)
        {
            _logger.Error(string.Format("An error occured on the client's hub connection - [{0},{1}]", _settingsName, _config.Scope), new { Exception = ex, StoreConfig = _config });

        }

        void hubConnection_Closed()
        {
            _logger.Warning(string.Format("Client's hub connection was closed - [{0},{1}]", _settingsName, _config.Scope), new { StoreConfig = _config });

            if (IsConnectedToServer()) return;

            _logger.Information(string.Format("Attempting to restart connection to the server - [{0},{1}]", _settingsName, _config.Scope), new { StoreConfig = _config });

            InitializeConnection();
        }

        void hubConnection_Reconnected()
        {
            _logger.Information(string.Format("Settings client has reconnected to the server - [{0},{1}]", _settingsName, _config.Scope), new { StoreConfig = _config });

            if (OnReconnection != null)
            {
                OnReconnection(this, new EventArgs());
            }
        }
        #endregion
    }
}
