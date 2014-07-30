using System;

namespace Configly.Entities
{
    public class Connection
    {
        public int ConnectionId { get; set; } 
        public string MachineName { get; set; }
        public string Ip { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Scope { get; set; }
        public string T { get; set; }
    }
}
