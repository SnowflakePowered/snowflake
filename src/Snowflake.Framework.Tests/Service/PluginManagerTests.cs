using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Xunit;
using Snowflake.Services;

namespace Snowflake.Service.Tests
{

    public interface ITestPlugin : IPlugin
    {
        bool Test();
    }

    public class PluginManagerTests
    {
        [Fact]
        public void PluginManager_RegisterTest()
        {
            var coreInstance = new Moq.Mock<ICoreService>();
            var testPlugin = new Moq.Mock<ITestPlugin>();
            testPlugin.SetupGet(plugin => plugin.PluginName).Returns("Test");
            IPluginManager pluginManager = new PluginManager("", coreInstance.Object);
            pluginManager.Register(testPlugin.Object);
        }

        [Fact]
        public void PluginManager_GetTest()
        {
            var coreInstance = new Moq.Mock<ICoreService>();
            var testPlugin = new Moq.Mock<ITestPlugin>();
            testPlugin.SetupGet(plugin => plugin.PluginName).Returns("Test");
            testPlugin.Setup(plugin => plugin.Test()).Returns(true);
            IPluginManager pluginManager = new PluginManager("", coreInstance.Object);
            pluginManager.Register(testPlugin.Object);
            Assert.True(pluginManager.Get<ITestPlugin>("Test").Test());
        }

        [Fact]
        public void PluginManager_GetMultipleTest()
        {
            var coreInstance = new Moq.Mock<ICoreService>();
            var testPlugin = new Moq.Mock<ITestPlugin>();
            testPlugin.SetupGet(plugin => plugin.PluginName).Returns("Test");
            testPlugin.Setup(plugin => plugin.Test()).Returns(true);
            IPluginManager pluginManager = new PluginManager("", coreInstance.Object);
            pluginManager.Register(testPlugin.Object);
            Assert.True(pluginManager.Get<ITestPlugin>().First().Value.Test());
        }

    }
}
