using System.Collections.Generic;
using Configly.Entities;

namespace Configly.DAL.Interfaces
{
    public interface IConnectionRepository
    {
        List<string> GetDistinctScopes(string settingName);
        void Save(ConnectionDetails connectionDetails);
    }
}
