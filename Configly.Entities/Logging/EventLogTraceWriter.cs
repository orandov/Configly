using System.Diagnostics;
using System.Text;

namespace Configly.Entities.Logging
{
    public class EventLogTraceWriter : System.IO.TextWriter
    {

        public EventLog _eventLog;

        public EventLogTraceWriter(EventLog eventLog)
            : base(System.Globalization.CultureInfo.InvariantCulture)
        {

            _eventLog = eventLog;

        }

        public override void WriteLine(string value)
        {

            _eventLog.WriteEntry(value);

        }
        
        public override Encoding Encoding
        {

            get { return Encoding.UTF8; }

        }

    }
}
