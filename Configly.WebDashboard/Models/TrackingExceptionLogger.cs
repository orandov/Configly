using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;

namespace Configly.WebDashboard.Models
{
    public class TrackingExceptionLogger : ExceptionLogger
    {
        private const string Category = "Configly Server";

        public TrackingExceptionLogger()
        {
            Trace.TraceInformation("Configly Logging started. [Server]");
           
        }

        public override void Log(ExceptionLoggerContext context)
        {
            Trace.TraceError(AddDataToMessage("An error occured in the Configly Server", new
            {
                context.Request.RequestUri,
                context.Exception,
                User = (context.RequestContext != null ? context.RequestContext.Principal != null ? context.RequestContext.Principal.Identity.Name : "Anonymous" : "Anonymous")
            }));
        }

        public void Log(Exception ex)
        {
            Trace.TraceError(AddDataToMessage("An error occured in the Configly Server", ex));
        }

        private string AddDataToMessage(string message, object data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Message: ");
            sb.AppendLine(message);
            sb.AppendLine();

            if (data != null)
            {
                sb.AppendLine("Data: ");
                try
                {
                    sb.AppendLine(JsonConvert.SerializeObject(data, Formatting.Indented));

                }
                catch (Exception ex)
                {
                    sb.AppendLine("Unable to serialize data to Json.");
                    sb.AppendLine(ex.ToString());
                }
            }

            return sb.ToString();
        }
    }
}