using System;
using System.Collections.Generic;
using System.Linq;
using Configly.DAL.Interfaces;
using Configly.Entities;

namespace Configly.DAL
{
    public class PropertiesRepository : IPropertiesRepository
    {
        public List<Property> Get(int settingId, string scope)
        {
            List<Property> defaultProperties = null;

            using (var context = new SettingsContext())
            {

                defaultProperties = context.Propertys.Where(p => p.SettingId == settingId && (p.Scope == null || p.Scope.Trim() == string.Empty)).ToList();

                if (!string.IsNullOrWhiteSpace(scope))
                {
                    var scopeSettings = context.Propertys.Where(p => p.SettingId == settingId && p.Scope.Equals(scope, StringComparison.OrdinalIgnoreCase)).ToList();

                    // Set scope values
                    foreach (var ds in defaultProperties)
                    {
                        var ss = scopeSettings.FirstOrDefault(s => s.Key == ds.Key);

                        if (ss != null)
                        {
                            ds.Value = ss.Value;
                        }
                    }
                }
            }

            return defaultProperties;
        }

        public Property Get(int id)
        {
            Property property = null;

            using (var context = new SettingsContext())
            {
                property = context.Propertys.Find(id);
            }

            return property;
        }

        public void Insert(Property property)
        {
            using (var context = new SettingsContext())
            {
                context.Propertys.Add(property);
                context.SaveChanges();
            }
        }

        public void Insert(IEnumerable<Property> properties)
        {
            using (var context = new SettingsContext())
            {
                context.Propertys.AddRange(properties);
                context.SaveChanges();
            }
        }

        public Property Update(Property property)
        {
            Property propertyToUpdate;
            using (var context = new SettingsContext())
            {
                propertyToUpdate = context.Propertys.Find(property.PropertyId);

                if (propertyToUpdate == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot update the property {0}. PropertyToUpdate does not exist", property));
                }

                propertyToUpdate.Value = property.Value;
                propertyToUpdate.Scope = property.Scope;

                context.SaveChanges();
            }
            return propertyToUpdate;
        }

        public void Update(List<Property> properties)
        {
            foreach (var property in properties)
            {
                Update(property);
            }
        }

        public void Delete(string application, string scope)
        {
            using (var context = new SettingsContext())
            {
                var propertiesToDelete = (from s in context.Propertys.Include("Scope")
                                       where s.Setting.Name.Equals(application, StringComparison.OrdinalIgnoreCase) &&
                                       ((scope == null || scope.Trim() == string.Empty) || s.Scope == scope)
                                       select s);

                if (propertiesToDelete != null)
                {
                    foreach (var property in propertiesToDelete)
                    {
                        context.Propertys.Remove(property);
                    }
                }

                context.SaveChanges();
            }
        }
    }
}
