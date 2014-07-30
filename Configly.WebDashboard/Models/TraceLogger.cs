using System;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace Configly.WebDashboard.Models
{
    public class TraceLogger : ILogger
    {
        public void Error(string message, object data = null)
        {
            Trace.TraceError(AddDataToMessage(message, data));
        }

        public void Error(string format, params object[] args)
        {
            Trace.TraceError(format, args);
        }

        public void Information(string message, object data = null)
        {
            Trace.TraceInformation(AddDataToMessage(message, data));

        }

        public void Information(string format, params object[] args)
        {
            Trace.TraceInformation(format, args);

        }

        public void Warning(string message, object data = null)
        {
            Trace.TraceWarning(AddDataToMessage(message, data));

        }

        public void Warning(string format, params object[] args)
        {
            Trace.TraceWarning(format, args);
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
