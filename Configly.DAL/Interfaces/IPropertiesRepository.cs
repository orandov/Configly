using System.Collections.Generic;
using Configly.Entities;

namespace Configly.DAL.Interfaces
{
    public interface IPropertiesRepository
    {
        List<Property> Get(int settingId, string scope);
        Property Get(int id);
        void Insert(Property property);
        void Insert(IEnumerable<Property> properties);
        Property Update(Property property);
        void Update(List<Property> properties);
        void Delete(string application, string scope);
    }
}
