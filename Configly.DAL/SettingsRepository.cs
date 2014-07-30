using System;
using System.Collections.Generic;
using System.Linq;
using Configly.DAL.Interfaces;
using Configly.Entities;

namespace Configly.DAL
{
    public class SettingsRepository : ISettingsRepository
    {
        public Setting Get(string name)
        {
            Setting setting = null;

            using (var context = new SettingsContext())
            {
                setting = context.Settings.Include("Properties").FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            return setting;

        }

        public Setting Get(string name, string scope) {
            Setting setting = null;
            using (var context = new SettingsContext())
            {
                setting = context.Settings.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                if (setting != null)
                {
                    var propertyRepository = new PropertiesRepository();
                    setting.Properties = propertyRepository.Get(setting.SettingId, scope);
                }
            }
            return setting;
        }
        public Setting Get(int id, string scope)
        {
            Setting setting = null;

            using (var context = new SettingsContext())
            {
                setting = context.Settings.Find(id);

                if (setting != null)
                {
                    var propertyRepository = new PropertiesRepository();
                    setting.Properties = propertyRepository.Get(setting.SettingId, scope);
                }
            }

            return setting;
        }

        public void Insert(Setting setting)
        {
            using (var context = new SettingsContext())
            {
                context.Settings.Add(setting);
                context.SaveChanges();
            }
        }

        public void Insert(IEnumerable<Setting> setting)
        {
            using (var context = new SettingsContext())
            {
                context.Settings.AddRange(setting);
                context.SaveChanges();
            }
        }

        public Setting Update(Setting setting)
        {
            Setting settingToUpdate;
            using (var context = new SettingsContext())
            {
                settingToUpdate = context.Settings.Find(setting.SettingId);

                if (settingToUpdate == null)
                {
                    throw new InvalidOperationException(string.Format("Cannot update the setting {0}. Setting does not exist", setting));
                }

                settingToUpdate.Name = setting.Name;

                var propertyRepository = new PropertiesRepository();
                foreach (var property in setting.Properties)
                {
                    propertyRepository.Update(property);
                }

                context.SaveChanges();
            }
            return settingToUpdate;
        }

        public void Delete(string application, string scope)
        {
            using (var context = new SettingsContext())
            {
                var settingToDelete = (from s in context.Settings
                                       where s.Name.Equals(application, StringComparison.OrdinalIgnoreCase)
                                       select s).FirstOrDefault();

                if (settingToDelete != null)
                {

                    var propertyRepository = new PropertiesRepository();
                    propertyRepository.Delete(application, scope);

                    context.Settings.Remove(settingToDelete);

                }

                context.SaveChanges();
            }
        }
    }
}