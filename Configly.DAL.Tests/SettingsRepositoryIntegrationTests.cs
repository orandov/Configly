using System;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;
using System.Collections.Generic;

namespace Configly.DAL.Tests
{
    [TestClass]
    public class SettingsRepositoryIntegrationTests
    {
        static string _defaultScopeName = string.Empty;
        static string _slukScopeName = "SL-UK";
        static string _dummyKey = "DummyPropertyName";
        static string _dummyDescription = "DummyDescription";
        static string _dummyValue = "Value of a setting property.";

        //[ClassInitialize]
        //public static void Setup(TestContext ctx)
        //{
        //    // Insert setting for Update test
        //    var repository = new PropertiesRepository();
        //    var updateSetting = repository.Get(1, _defaultScopeName);
        //    if (updateSetting == null || updateSetting.Count == 0)
        //    {
        //        var newSetting = new Property
        //        {
        //            Setting = new Setting{Name = "UpdateTest",SettingId = 123},
        //            Value = _dummyValue,
        //            Key = _dummyKey,
        //            Description = _dummyDescription
        //        };

        //        repository.Insert(newSetting);
        //    }

        //    SetupGetTest(repository);


        //}

        //private static void SetupGetTest(PropertiesRepository repository)
        //{
        //    var getSetting = repository.Get(2, _slukScopeName);
        //    if (getSetting == null || getSetting.Count == 0)
        //    {


        //        Property newSetting = new Property
        //        {
        //            Key = "1",
        //            Value = "1"
        //        };

        //        Property newSetting1 = new Property
        //        {
        //            Key = "1",
        //            Value = "2",
        //        };

        //        Property newSetting2 = new Property
        //        {
                  
        //            Key = "3",
        //            Value = "3",
        //        };

        //        repository.Insert(new List<Property> { newSetting, newSetting1, newSetting2 });
        //    }
        //}

        //[ClassCleanup]
        //public static void CleanUp()
        //{
        //    var repository = new PropertiesRepository();
        //    repository.Delete("InsertTest", _defaultScopeName);
        //    repository.Delete("UpdateTest", _defaultScopeName);
        //    repository.Delete("GetTest", _defaultScopeName);
        //    repository.Delete("GetTest", _slukScopeName);

        //}

      



        [TestMethod]
        public void Get_ScopeHasOverrides_ReturnsNullScopeAndOverrides()
        {
            //var repository = new PropertiesRepository();

            //var list1 = repository.GetAndUpdate("GetTest", _slukScopeName);

            //list1.Count.ShouldBeEquivalentTo(2);
            //list1[0].Value.ShouldBeEquivalentTo("2");
            //list1[1].Value.ShouldBeEquivalentTo("3");
        }

        [TestMethod]
        public void Insert_InvalidScopeId_ThrowsException()
        {
            //var repository = new PropertiesRepository();
            //Setting newSetting = new Setting
            //{
            //    TName = "InsertTest",
            //    Key = _dummyKey,
            //    Description = _dummyDescription,
            //    Value = _dummyValue,
            //    ScopeId = 1234567890
            //};

            //Action insert = () => { repository.Insert(newSetting); };

            //insert.ShouldThrow<Exception>();

        }

        [TestMethod]
        public void InsertGet_InvokeMethod_NewSettingsExist()
        {
            //var repository = new PropertiesRepository();

            //Property newSetting = new Property
            //{
            //    TName = "InsertTest",
            //    Value = _dummyValue,
            //    Key = _dummyKey,
            //    Description = _dummyDescription,
            //};

            //repository.Insert(newSetting);

            //List<Setting> q = repository.GetAndUpdate(newSetting.TName, _defaultScopeName);

            //q.Count.ShouldBeEquivalentTo(1);
     //       q[0].ShouldBeEquivalentTo<Setting>(newSetting);
        }


        [TestMethod]
        public void Update_ChangeValue_SettingIsUpdated()
        {
            //var repository = new PropertiesRepository();
            //string application = "UpdateTest";
            //string value = DateTime.Now.ToString();
            //var expectedSetting = new Setting
            //{
            //    TName = application,
            //    Key = _dummyKey,
            //    Description = _dummyDescription,
            //    Value = value
            //};

            //List<Setting> q1 = repository.GetAndUpdate(application, _defaultScopeName);
            //expectedSetting.PropertyId = q1[0].PropertyId;

            //repository.Update(expectedSetting);

            //List<Setting> q = repository.GetAndUpdate(application, _defaultScopeName);

            //q.Count.ShouldBeEquivalentTo(1);
            //q[0].ShouldBeEquivalentTo<Setting>(expectedSetting);
        }

        [TestMethod]
        public void Update_SettingDoesNotExist_ThrowsException()
        {
            //var repository = new PropertiesRepository();
            //string application = "application_that_does_not_exist";

            //string value = DateTime.Now.ToString();

            //Action update = delegate
            //{
            //    repository.Update(new Setting
            //    {
                    
            //        Key = _dummyKey,
            //        Description = _dummyDescription,
            //        Value = value
            //    });
            //};

            //update.ShouldThrow<InvalidOperationException>();
        }

       
    }
}
