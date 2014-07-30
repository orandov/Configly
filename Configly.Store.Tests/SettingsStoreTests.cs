using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configly.Entities;
using Configly.Store.Entities;
using Configly.Store.Interfaces;

namespace Configly.Store.Tests
{
    [TestClass]
    public class SettingsStoreTests
    {
        private static string _defaultScopeName = "Default";
        private static string _dummyJsonValue = "{Hello:IamJson}";
        private static string _dummyApplication = "testapplication";

        [TestMethod]
        public void Get_ServerConnected_RetrievesSettingsFromServer()
        {
            var expectedSetting = new TestSettings { FirstProperty = "hello", SecondProperty = 100, Timeout = 30 };

            var rawSetting = GetDummySettingFromServer();

            var serverProxy = A.Fake<ISettingsServerProxy>();
            A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(true);
            A.CallTo(() => serverProxy.GetAndUpdate(A<Setting>._)).Returns(rawSetting);

            var settingsFile = A.Fake<ISettingsManager<TestSettings>>();

            var store = GetStore(serverProxy, settingsFile);

            var settingFromServer = store.Get();

            settingFromServer.ShouldBeEquivalentTo<TestSettings>(expectedSetting);
        }

        private SettingsStore<TestSettings> GetStore(ISettingsServerProxy proxy, ISettingsManager<TestSettings> fileManager)
        {
            var config = new SettingsStoreConfiguration
            {
                Scope = _defaultScopeName,
                ServerUrl = "http://dummyurl"
            };
            var logger = A.Fake<ILogger>();
            return new SettingsStore<TestSettings>(config, proxy, fileManager, logger);
        }

        private static Setting GetDummySettingFromServer()
        {
            var rawSetting = new Setting
            {
                Name = "TestSettings",
                Properties = new List<Property>{
                       new Property{
                            Key="FirstProperty",
                            Value = "hello"
                       },
                       new Property{
                            Key="SecondProperty",
                            Value = "100"
                       },new Property{
                            Key="Timeout",
                            Value = "30"
                       }
                   }
            };
            return rawSetting;
        }

        private static Setting GetDefaultSettingFromServer()
        {
            var rawSetting = new Setting
            {
                Name = "TestSettings",
                Properties = new List<Property>{
                       new Property{
                            Key="FirstProperty",
                            Value = "Default Value of First Property"
                       },
                       new Property{
                            Key="SecondProperty",
                            Value = "0"
                       },new Property{
                            Key="Timeout",
                            Value = "30"
                       }
                   }
            };
            return rawSetting;
        }

        [TestMethod]
        public void Get_ServerConnected_SavesSettingsToBackupFile()
        {
            object expectedSetting = new TestSettings { FirstProperty = "hello", SecondProperty = 100, Timeout = 30 };


            var rawSetting = GetDummySettingFromServer();

            var serverProxy = A.Fake<ISettingsServerProxy>();
            A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(true);
            A.CallTo(() => serverProxy.GetAndUpdate(A<Setting>._)).Returns(rawSetting);

            var settingsFile = A.Fake<ISettingsManager<TestSettings>>();

            var store = GetStore(serverProxy, settingsFile);

            var settingTask = store.Get();

            A.CallTo(() => settingsFile.Set(A<object>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        //[TestMethod]
        //public void Get_ServerConnectedNewSettings_SavesSettingsToBackupFile()
        //{
        //    var config = new SettingsStoreConfiguration
        //    {
        //        Scope = _defaultScopeName,
        //        ServerUrl = "http://dummyurl"
        //    };

        //    Setting rawSetting = null;
        //    string expectedJson = "{\"FirstProperty\":\"Default Value of First Property\",\"SecondProperty\":0,\"Timeout\":30}";

        //    var serverProxy = A.Fake<ISettingsServerProxy>();
        //    A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(true);
        //    A.CallTo(() => serverProxy.GetAndUpdate()).Returns(Task.FromResult(rawSetting));

        //    var settingsFile = A.Fake<ISettingsManager>();

        //    var store = new SettingsStore<TestSettings>(config, serverProxy, settingsFile);

        //    var settingTask = store.GetAndUpdate();

        //    A.CallTo(() => settingsFile.Set(expectedJson)).MustHaveHappened(Repeated.Exactly.Once);
        //}

        [TestMethod]
        public void Get_ServerConnectedNewSettings_ReturnsDefaultSettings()
        {
            var expectedSetting = new TestSettings { FirstProperty = "Default Value of First Property", SecondProperty = 0, Timeout = 30 };

            Setting rawSetting = GetDefaultSettingFromServer();

            var serverProxy = A.Fake<ISettingsServerProxy>();
            A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(true);
            A.CallTo(() => serverProxy.GetAndUpdate(A<Setting>._)).Returns(rawSetting);

            var settingsFile = A.Fake<ISettingsManager<TestSettings>>();

            var store = GetStore(serverProxy, settingsFile);

            var settingFromServer = store.Get();

            settingFromServer.ShouldBeEquivalentTo<TestSettings>(expectedSetting);
        }

        [TestMethod]
        public void Get_ServerDisconnected_RetrievesSettingsFromBackupFile()
        {
            var expectedSetting = new TestSettings { FirstProperty = "hello", SecondProperty = 100, Timeout = 30 };

            var serverProxy = A.Fake<ISettingsServerProxy>();
            A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(false);

            var settingsFile = A.Fake<ISettingsManager<TestSettings>>();
            A.CallTo(() => settingsFile.Get()).Returns(expectedSetting);

            var store = GetStore(serverProxy, settingsFile);

            var settingTask = store.Get();

            settingTask.ShouldBeEquivalentTo<TestSettings>(expectedSetting);
        }

        [TestMethod]
        public void Get_ServerDisconnectedNoBackupFile_ThrowsException()
        {
            var serverProxy = A.Fake<ISettingsServerProxy>();
            A.CallTo(() => serverProxy.IsConnectedToServer()).Returns<bool>(false);

            var settingsFile = A.Fake<ISettingsManager<TestSettings>>();
            A.CallTo(() => settingsFile.Get()).Throws<FileNotFoundException>();

            var store = GetStore(serverProxy, settingsFile);

            Action get = () => { store.Get(); };

            get.ShouldThrow<FileNotFoundException>();
        }
    }
}