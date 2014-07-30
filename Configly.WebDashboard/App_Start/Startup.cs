using Configly.WebDashboard;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Configly.WebDashboard
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration {EnableDetailedErrors = true};

            // app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR(hubConfiguration);
        }
    }
}
