using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Configly.WebDashboard.Models;

namespace Configly.WebDashboard
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("BreezeApi", "api/{controller}/{action}");

            config.MapHttpAttributeRoutes();

            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new TrackingExceptionLogger());

            //// There must be exactly one exception handler. (There is a default one that may be replaced.)
            //// To make this sample easier to run in a browser, replace the default exception handler with one that sends
            //// back text/plain content for all errors.
            //config.Services.Replace(typeof(IExceptionHandler), new GenericTextExceptionHandler());
        }
    }
}