using System.Collections.ObjectModel;
using Configly.DAL.Interfaces;
using Configly.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configly.Services.Tests
{
    /// <summary>
    ///     Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class SettingsServiceTests
    {
        private static readonly string _defaultScopeName = string.Empty;
        private static string _dummyJsonValue = "{Hello:IamJson}";
        private static string _dummyTName = "testTName";

        private static Setting GetSettings()
        {
            return new Setting
            {
                Name = _dummyTName,
                Properties = new Collection<Property>()
            };
        }

        [TestMethod]
        public void Get_WithValidArguements_ReturnsSettings()
        {
            var settingsRepository = A.Fake<ISettingsRepository>();
            var propRepository = A.Fake<IPropertiesRepository>();
            Setting expectedSetting = GetSettings();
            var expectedResponse = new GetSettingsResponse
            {
                Setting = expectedSetting,
                WereSettingsAdded = false
            };

            A.CallTo(() => settingsRepository.Get(_dummyTName, _defaultScopeName)).Returns(expectedSetting);

            var service = new SettingsService(propRepository, settingsRepository);
            var request = new GetSettingsRequest {Setting = expectedSetting, Scope = _defaultScopeName};

            GetSettingsResponse actual = service.GetAndUpdate(request);

            actual.ShouldBeEquivalentTo(expectedResponse);
        }

        //[TestMethod]
        //public void Get_WithNewSettingFromClient_ReturnsSettingsPlusNewSetting()
        //{
        //    var PropertiesRepository = A.Fake<IPropertiesRepository>();
        //    var scopeRepository = A.Fake<IScopeRepository>();
        //    var expected = GetSettings();
        //    expected.Add(new Setting(_dummyTName, "NewSettingKey", "Default Value", null));
        //    var dbSettings = GetSettings();

        //    A.CallTo(() => PropertiesRepository.GetAndUpdate(_dummyTName, _defaultScopeName)).Returns<List<Setting>>(dbSettings);

        //    var service = new SettingsService(PropertiesRepository, scopeRepository);
        //    var request = new GetSettingsRequest(_dummyTName, _defaultScopeName);
        //    request.properties = expected.Select(s => new { Key = s.Key, Value = s.Value }).ToDictionary(s => s.Key, s => s.Value);


        //    var actual = service.GetAndUpdate(request);

        //    actual.ShouldBeEquivalentTo(expected);
        //}

        //[TestMethod]
        //public void Get_BadArguements_ThrowsFaultException()
        //{
        //    var settingsRepository = A.Fake<IPropertiesRepository>();
        //    var scopeRepository = A.Fake<IScopeRepository>();

        //    A.CallTo(() => settingsRepository.GetAndUpdate(A<string>._, A<string>._)).Throws<Exception>();

        //    var service = new SettingsService(settingsRepository, scopeRepository);

        //    Action get = () => service.GetAndUpdate(A<GetSettingsRequest>._);

        //    get.ShouldThrow<FaultException>();
        //}

        //[TestMethod]
        //public void Set_NewSetting_InsertIsCalled()
        //{
        //    var PropertiesRepository = A.Fake<IPropertiesRepository>();
        //    var scopeRepository = A.Fake<IScopeRepository>();
        //    var newSetting = new Setting(_dummyTName, _dummyJsonValue, 0);

        //    A.CallTo(() => PropertiesRepository.GetAndUpdate(A<string>._, A<string>._)).Returns(null);

        //    var service = new SettingsService(PropertiesRepository, scopeRepository);

        //    var request = new SetSettingsRequest { Name = _dummyTName, JsonValue = _dummyJsonValue, Scope = _defaultScopeName };

        //    service.Set(request);

        //    A.CallTo(() => PropertiesRepository.Insert(A<Setting>._)).MustHaveHappened();
        //}

        //[TestMethod]
        //public void Set_ExistingSetting_UpdateIsCalled()
        //{
        //    var PropertiesRepository = A.Fake<IPropertiesRepository>();
        //    var scopeRepository = A.Fake<IScopeRepository>();
        //    var existingSetting = new Setting(_dummyTName, _dummyJsonValue, 0);

        //    A.CallTo(() => PropertiesRepository.GetAndUpdate(A<string>._, A<string>._)).Returns(existingSetting);

        //    var service = new SettingsService(PropertiesRepository, scopeRepository);

        //    var request = new SetSettingsRequest { Name = _dummyTName, JsonValue = _dummyJsonValue, Scope = _defaultScopeName };

        //    service.Set(request);

        //    A.CallTo(() => PropertiesRepository.Update(_dummyTName,_defaultScopeName,_dummyJsonValue)).MustHaveHappened();
        //}
    }
}