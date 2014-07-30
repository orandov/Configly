//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Configly.DAL;
//using FluentAssertions;
//using Configly.Entities;

//namespace Configly.Services.Tests
//{
//    [TestClass]
//    public class SettingsServiceIntegrationTests
//    {
//        static string _defaultScopeName = "Default";
//        static string _dummyJsonValue = "{Hello:IamJson}";
        
//        [ClassInitialize]
//        public static void Setup(TestContext ctx)
//        {
//            // Insert settingFromDB for Grt test
//            var repository = new PropertiesRepository();
//            var getSetting = repository.GetAndUpdate("GetUpdateTest", _defaultScopeName);
//            if (getSetting == null)
//            {
//                Setting newSetting = new Setting
//                {
//                    TName = "GetUpdateTest",
//                    Value = _dummyJsonValue,
//                    ScopeId = GetDefaultScope().PropertyId
//                };

//                repository.Insert(newSetting);
//            }
//        }

//        [ClassCleanup]
//        public static void CleanUp()
//        {
//            var repository = new PropertiesRepository();
//            repository.Delete("GetUpdateTest", _defaultScopeName);
//            repository.Delete("SetTest", _defaultScopeName);

//        }

//        //[TestMethod]
//        //public void Get_InvokeMethod_ReturnsSetting()
//        //{
//        //    string application = "GetUpdateTest";
//        //    var service = new SettingsService();
//        //    var request = new GetSettingsRequest
//        //    {
//        //        Name = application,
//        //        Scope = _defaultScopeName
//        //    };
//        //    var expected = new Setting(application, _dummyJsonValue, GetDefaultScope().PropertyId);
//        //    var settingFromDB = service.GetAndUpdate(request);

//        //    settingFromDB.ShouldBeEquivalentTo<Setting>(expected);
//        //}

//        //[TestMethod]
//        //public void Get_NonExistantApplication_ReturnsNull()
//        //{
//        //    string application = "Application_That_Does_Not_Exist";
//        //    var service = new SettingsService();
//        //    var request = new GetSettingsRequest
//        //    {
//        //        Name = application,
//        //        Scope = _defaultScopeName
//        //    };
//        //    var settingFromDB = service.GetAndUpdate(request);

//        //    settingFromDB.ShouldBeEquivalentTo<Setting>(null);
//        //}

//        //[TestMethod]
//        //public void Set_NewSettings_ReturnsSettingInGet()
//        //{
//        //    string application = "SetTest";
//        //    var service = new SettingsService();
//        //    var request = new SetSettingsRequest
//        //    {
//        //        Name = application,
//        //        Scope = _defaultScopeName,
//        //        JsonValue = _dummyJsonValue
//        //    };

//        //    var expected = new Setting(application, _dummyJsonValue, GetDefaultScope().PropertyId);
//        //     service.Set(request);

//        //     var getRequest = new GetSettingsRequest
//        //     {
//        //         Name = application,
//        //         Scope = _defaultScopeName
//        //     };

//        //     var settingFromDB = service.GetAndUpdate(getRequest);

//        //    settingFromDB.ShouldBeEquivalentTo<Setting>(expected);
//        //}


//        //[TestMethod]
//        //public void Set_ExistingSettings_ReturnsUpdatedSettingInGet()
//        //{
//        //    string application = "GetUpdateTest";
//        //    string updatedValue = "{Goodbye:Jason}" + DateTime.Now.ToLongDateString();
//        //    var service = new SettingsService();
//        //    var request = new SetSettingsRequest
//        //    {
//        //        Name = application,
//        //        Scope = _defaultScopeName,
//        //        JsonValue = updatedValue
//        //    };

//        //    var expected = new Setting(application, updatedValue, GetDefaultScope().PropertyId);
//        //    service.Set(request);

//        //    var getRequest = new GetSettingsRequest
//        //    {
//        //        Name = application,
//        //        Scope = _defaultScopeName
//        //    };

//        //    var settingFromDB = service.GetAndUpdate(getRequest);

//        //    settingFromDB.ShouldBeEquivalentTo<Setting>(expected);
//        //}



//        private static Scope GetDefaultScope()
//        {
//            var scopeRepository = new ScopeRepository();
//            var ds = scopeRepository.GetAndUpdate(_defaultScopeName);
//            if (ds == null)
//            {
//                ds = new Scope { Name = _defaultScopeName };
//                scopeRepository.Insert(ds);
//            }
//            return ds;
//        }
//    }
//}
