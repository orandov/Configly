using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Configly.Store.Tests
{
    [TestClass]
    public class DefaultSettingsHandlerTests
    {
        [TestMethod]
        public void CreateDefaultSettingProperties_WithDefaultValue_FillsInDefaultValue()
        {
            var handler = new DefaultSettingsHandler<TestSettings>();

            var props = handler.CreateDefaultSettingProperties();

            Assert.AreEqual(props.FirstOrDefault(p => p.Key == "Timeout").Value, "30");
        }

        [TestMethod]
        public void CreateDefaultSettingProperties_WithDisplayName_FillsInDescription()
        {
            var handler = new DefaultSettingsHandler<TestSettings>();

            var props = handler.CreateDefaultSettingProperties();

            Assert.AreEqual(props.FirstOrDefault(p => p.Key == "SecondProperty").Description, "Some int property");
        }

        [TestMethod]
        public void CreateDefaultSettingProperties_FillsInType()
        {
            var handler = new DefaultSettingsHandler<TestSettings>();

            var props = handler.CreateDefaultSettingProperties();

            Assert.AreEqual(props.FirstOrDefault(p => p.Key == "SecondProperty").Type, "System.Int32");
            Assert.AreEqual(props.FirstOrDefault(p => p.Key == "FirstProperty").Type, "System.String");
        }

    }
}
