using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configly.WebDashboard.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Configly.DAL;
using Configly.DAL.Interfaces;
using Configly.Entities;
using Configly.Services;

namespace Configly.WebDashboard.Hubs
{
    [HubName("settings")]
    public class SettingsHub : Hub
    {
        private static IConnectionRepository _connectionRepository;
        private static ISettingsRepository _settingsRepository;
        private static ILogger _logger;
        static SettingsHub()
        {
            _connectionRepository = new ConnectionRepository();
            _settingsRepository = new SettingsRepository();
            _logger = new TraceLogger();
        }

        public SettingsHub()
        {
            _connectionRepository = new ConnectionRepository();
            _settingsRepository = new SettingsRepository();
            _logger = new TraceLogger();
        }

        public SettingsHub(IConnectionRepository connectionRepository, ISettingsRepository settingsRepository, ILogger logger)
        {
            _connectionRepository = connectionRepository;
            _settingsRepository = settingsRepository;
            _logger = logger;
        }

        public Setting GetAndUpdate(Setting setting)
        {
            if (setting == null) return null;

            GetSettingsResponse response;

            _logger.Information(string.Format("Getting setting for [{0}]", setting.Name), new { SettingFromClient = setting });


            try
            {
                var service = new SettingsService();

                var request = new GetSettingsRequest
                {
                    Setting = setting,
                    Scope = ParseParmeters().Scope
                };
                response = service.GetAndUpdate(request);

                if (response.WereSettingsAdded)
                {
                    Task.Factory.StartNew(() => NotifyClientsOfChangedSettings(response.Setting));
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while retrieving settings", new
                {
                    SettingFromClient = setting,
                    Exception = ex
                });
                throw;
            }

            _logger.Information(string.Format("Successfully retrieved setting [{0}].", setting.Name), response);

            return response.Setting;
        }

        public static void NotifyClientsOfChangedSettings(Setting setting, string scope = null)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SettingsHub>();

            context.Clients.Group("WebDashboard").settingsRefreshed(setting);

            // If a scope override prop was changed only notify that scope
            if (!string.IsNullOrEmpty(scope))
            {
                context.Clients.Group(string.Format("{0}_{1}", setting.Name, scope)).settingsRefreshed(setting);
            }
            else
            {
                context.Clients.Group(setting.Name).settingsRefreshed(setting);

                // Get all scopes for connections under settings
                // foreach scope get settingFromDB w/props for that scope
                // call settingsRefreshed on that scope group
                List<string> scopes = _connectionRepository.GetDistinctScopes(setting.Name);
                foreach (var currentScope in scopes)
                {
                    var settingToNotify = _settingsRepository.Get(setting.SettingId, currentScope);
                    context.Clients.Group(string.Format("{0}_{1}", settingToNotify.Name, currentScope)).settingsRefreshed(settingToNotify);
                }
            }
        }

        public override Task OnConnected()
        {
            AddToConnections();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            AddToConnections();
            return base.OnReconnected();
        }

        private ConnectionDetails ParseParmeters()
        {
            ConnectionDetails result = new ConnectionDetails();
            result.Scope = Context.QueryString["scope"];
            result.TypeName = Context.QueryString["tname"] == "" ? string.Empty : Context.QueryString["tname"];
            result.IP = Context.QueryString["ip"];
            result.MachineName = Context.QueryString["machineName"];
            return result;
        }

        private void AddToConnections()
        {
            ConnectionDetails connectionDetails = null;
            try
            {
                connectionDetails = ParseParmeters();
                if (string.IsNullOrEmpty(connectionDetails.TypeName))
                    return;

                AddConnectionToGroup(connectionDetails);
                _connectionRepository.Save(connectionDetails);

                var context = GlobalHost.ConnectionManager.GetHubContext<ConnectionHub>();

                context.Clients.All.connectionsUpdated();


            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while adding a connection.", new
                {
                    connectionDetails,
                    Exception = ex
                });

               throw;
            }
        }

        private void AddConnectionToGroup(ConnectionDetails connectionDetails)
        {
            if (string.IsNullOrWhiteSpace(connectionDetails.Scope))
            {
                this.Groups.Add(Context.ConnectionId, connectionDetails.TypeName);
            }
            else
            {
                this.Groups.Add(Context.ConnectionId, string.Format("{0}_{1}", connectionDetails.TypeName, connectionDetails.Scope));
            }
        }
    }


}