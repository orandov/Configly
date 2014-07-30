using System;
using Newtonsoft.Json;

namespace Configly.Entities
{
    public class PropertyHistory
    {
        public int PropertyHistoryId { get; set; }
        public string User { get; set; }
        public DateTime TimeStamp { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        [JsonIgnore]
        public Property Property { get; set; }
        public int PropertyId { get; set; } 
    }
}
