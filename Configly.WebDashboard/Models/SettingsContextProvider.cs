using System;
using System.Collections.Generic;
using System.Linq;
using Breeze.ContextProvider.EF6;
using Configly.WebDashboard.Hubs;
using Configly.DAL;
using Configly.Entities;

namespace Configly.WebDashboard.Models
{
    public class SettingsContextProvider : EFContextProvider<SettingsContext>
    {
        protected override void AfterSaveEntities(Dictionary<Type, List<Breeze.ContextProvider.EntityInfo>> saveMap, List<Breeze.ContextProvider.KeyMapping> keyMappings)
        {
            foreach (var item in saveMap)
            {
                foreach (var entityItem in item.Value)
                {
                    var entity = entityItem.Entity as Property;

                    if (entity != null)
                    {
                        var setting = Context.Settings.Find(entity.SettingId);

                        SettingsHub.NotifyClientsOfChangedSettings(setting, entity.Scope);
                    }
                }
            }
            base.AfterSaveEntities(saveMap, keyMappings);
        }

        protected override Dictionary<Type, List<Breeze.ContextProvider.EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<Breeze.ContextProvider.EntityInfo>> saveMap)
        {
            foreach (var item in saveMap)
            {
                foreach (var entityItem in item.Value)
                {
                    var entity = entityItem.Entity as Property;

                    if (entity != null)
                    {
                        using (var context = new SettingsContext())
                        {
                            var previousValue = context.Propertys.First(t => t.PropertyId == entity.PropertyId).Value;
                            var history = new PropertyHistory
                            {
                                PropertyId = entity.PropertyId,
                                OldValue = previousValue,
                                NewValue = entity.Value,
                                TimeStamp = DateTime.Now
                            };

                            context.PropertyHistories.Add(history);
                            context.SaveChanges();
                        }
                    }
                }
            }
            return base.BeforeSaveEntities(saveMap);
        }
    }
}