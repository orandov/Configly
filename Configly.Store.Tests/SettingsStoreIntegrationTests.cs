using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Configly.Store;
using Configly.Store.Entities;

namespace Configly.Store.Tests
{
    [TestClass]
    public class SettingsStoreIntegrationTests
    {
        [TestMethod]
        public void Get_NewTestSettings_RetrievesDefaultValues()
        {
            var config = new SettingsStoreConfiguration
            {
                ServerUrl = "http://localhost/Configly"
            };

            var store = new SettingsStore<TestSettings>(config);
            store.OnRefresh += store_OnRefresh;
            TestSettings settings = store.Get();

            Assert.IsNotNull(settings);
        }

        void store_OnRefresh(object sender, SettingsRefreshArgs e)
        {
            
        }
    }
}
