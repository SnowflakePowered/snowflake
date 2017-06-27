using Snowflake.Loader;
using Snowflake.Services.Tests;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;
namespace Snowflake.Loader.Tests
{
    public class ModuleEnumeratorTests
    {
        [Fact]
        public void ModuleEnumerator_Init()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var moduleEnum = new ModuleEnumerator(appDataDirectory.FullName);
            Assert.True(appDataDirectory.EnumerateDirectories().Any(d => d.Name == "modules"));
        }

        [Fact]
        public void ModuleEnumerator_LoadModules()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var moduleDirectory = appDataDirectory.CreateSubdirectory("modules").CreateSubdirectory("testModule");
            string testModule = TestUtilities.GetStringResource("Loader.testModule.json");
            File.WriteAllText(Path.Combine(moduleDirectory.FullName, "module.json"), testModule, Encoding.UTF8);
            var moduleEnum = new ModuleEnumerator(appDataDirectory.FullName);
            Assert.True(moduleEnum.Modules.Any(m => m.Entry == "testModule"));
            Assert.Equal(moduleDirectory.FullName, moduleEnum.Modules.First().ModuleDirectory.FullName);
            Assert.Equal(moduleDirectory.CreateSubdirectory("contents").FullName, moduleEnum.Modules.First().ContentsDirectory.FullName);

        }
    }
}
