using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Configly.DAL.Interfaces;
using Configly.Entities;

namespace Configly.DAL
{
    public class ConnectionRepository:IConnectionRepository
    {

        public List<string> GetDistinctScopes(string settingName)
        {
            List<string> scopes = null;
            using (var context = new SettingsContext())
            {
                scopes = context.Connections.Where(c => c.T.Equals(settingName, StringComparison.OrdinalIgnoreCase) && (c.Scope != null) && (c.Scope.Trim() != string.Empty)).Select(c => c.Scope).Distinct().ToList();
            }
            return scopes;
        }


        public void Save(ConnectionDetails connectionDetails)
        {
            using (var context = new SettingsContext())
            {
                string machineName = connectionDetails.MachineName;
                string scope = connectionDetails.Scope;
                string tname = connectionDetails.TypeName;

                var previousConnection = context.Connections.FirstOrDefault(t => t.MachineName == machineName && t.T == tname && t.Scope == scope);

                if (previousConnection != null)
                {
                    previousConnection.TimeStamp = DateTime.Now;
                }
                else
                {
                    Connection connection = new Connection();
                    connection.Ip = connectionDetails.IP;
                    connection.MachineName = connectionDetails.MachineName;
                    connection.Scope = connectionDetails.Scope;
                    connection.T = connectionDetails.TypeName;
                    connection.TimeStamp = DateTime.Now;

                    context.Connections.Add(connection);
                }
                context.SaveChanges();
            }
        }
    }
}
