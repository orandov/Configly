using System.Collections.Generic;
using Configly.Entities;

namespace Configly.DAL.Interfaces
{
    public interface ISettingsRepository
    {
        Setting Get(string name);
        Setting Get(int id, string scope);
        Setting Get(string name, string scope);
        void Insert(Setting setting);
        void Insert(IEnumerable<Setting> setting);
        Setting Update(Setting setting);
        void Delete(string application,string scope);
    }
}
