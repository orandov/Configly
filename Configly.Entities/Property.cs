using Newtonsoft.Json;

namespace Configly.Entities
{
    public class Property
    {
        public int PropertyId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string Scope { get; set; }
        public string Type { get; set; }

        [JsonIgnore] 
        public Setting Setting { get; set; }
        public int SettingId { get; set; }

        //public ICollection<PropertyHistory> PropertyHistories { get; set; }

    }
}
