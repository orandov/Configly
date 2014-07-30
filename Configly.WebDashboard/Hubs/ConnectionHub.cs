using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Configly.WebDashboard.Hubs
{
    [HubName("connections")]
    public class ConnectionHub : Hub
    {
        public void ConnectionsUpdated()
        {
            Clients.All.connectionsUpdated();
        }
    }
}