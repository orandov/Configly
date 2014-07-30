using System.Collections.Generic;

namespace Configly.Entities
{
    public class Setting
    {
        public int SettingId { get; set; }
        public string Name { get; set; }

        public ICollection<Property> Properties { get; set; }
    
    }
}
