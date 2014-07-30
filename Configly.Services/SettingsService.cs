using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Configly.DAL;
using Configly.DAL.Interfaces;
using Configly.Entities;
using Configly.Entities.Interfaces;

namespace Configly.Services
{
    public class SettingsService : ISettingsService
    {
        IPropertiesRepository _propertiesRepository;
        ISettingsRepository _settingRepository;

        public SettingsService()
        {
            // TODO: Replace with DI
            _propertiesRepository = new PropertiesRepository();
            _settingRepository = new SettingsRepository();
        }

        public SettingsService(IPropertiesRepository propertiesRepository, ISettingsRepository settingRepository)
        {
            _propertiesRepository = propertiesRepository;
            _settingRepository = settingRepository;
        }

        public GetSettingsResponse GetAndUpdate(GetSettingsRequest request)
        {
            var response = new GetSettingsResponse();


            Setting settingFromDB = _settingRepository.Get(request.Setting.Name, request.Scope);

            // New Setting
            if (settingFromDB == null)
            {
                // Always add properties to the null scope. They can override in the UI
                foreach (var prop in request.Setting.Properties)
                {
                    prop.Scope = null;
                }

                _settingRepository.Insert(request.Setting);
                response.Setting = request.Setting;
                response.WereSettingsAdded = true;
            }
            else
            {
                // If new properties were added to the setting From client need to add them to the DB
                if (request.Setting.Properties.Count > settingFromDB.Properties.Count)
                {
                    response.Setting = AddNewKeysToDbAndReturnFullList(request, settingFromDB);
                    response.WereSettingsAdded = true;
                }
                else
                {
                    response.Setting = settingFromDB;
                }
            }
            return response;
        }

        private Setting AddNewKeysToDbAndReturnFullList(GetSettingsRequest request, Setting settingFromDb)
        {
            var existingKeys = settingFromDb.Properties.Select(pDB => pDB.Key);

            IEnumerable<Property> newKeys = request.Setting.Properties.Where(p => !existingKeys.Contains(p.Key));

            foreach (var property in newKeys)
            {
                property.SettingId = settingFromDb.SettingId;
                // Always add properties to the null scope. They can override in the UI
                property.Scope = null;
            }

            _propertiesRepository.Insert(newKeys);

            return _settingRepository.Get(settingFromDb.SettingId, request.Scope);
        }

        //public Setting Set(SetSettingsRequest request)
        //{
        //    if (request.SettingToSave == null)
        //        return null;

        //    Setting savedSettings = null;
        //    try
        //    {
        //        Setting settingToSave = _settingRepository.Get(request.SettingToSave.Name);

        //        if (settingToSave == null)
        //        {
        //            throw new Exception("Can't find the setting to update.");
        //        }
        //        else
        //        {
        //            _propertiesRepository.Update((List<Property>)request.SettingToSave.Properties);
        //        }

        //        var updatedSetting = _settingRepository.Get(request.SettingToSave.Name);
        //        return updatedSetting;

        //    }
        //    catch (FaultException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.Message);
        //    }

        //}

    }
}
