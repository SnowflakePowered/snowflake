using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Moq;
using Snowflake.Services;
using Snowflake.Services.AssemblyLoader;
using Snowflake.Services.Tests;
using Snowflake.Tests;
using Snowflake.Tests.Composable;
using Snowflake.Tests.InvalidComposable;
using Xunit;

namespace Snowflake.Loader.Tests
{
    public class AssemblyComposerTests
    {
        [Fact]
        public void AssemblyComposer_ComposeTest()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var moduleDirectory = appDataDirectory.CreateSubdirectory("modules");

            Stream composableAssemblyZipStream =
                TestUtilities.GetResource("Loader.Snowflake.Framework.Tests.DummyComposable.zip");
            ZipArchive archive = new ZipArchive(composableAssemblyZipStream);

            archive.ExtractToDirectory(moduleDirectory.CreateSubdirectory("Snowflake.Framework.Tests").FullName);
            var container = new ServiceContainer(appDataDirectory.FullName);

            Assert.Contains(container.Get<IModuleEnumerator>()
                .Modules, m => m.Entry == "Snowflake.Framework.Tests.DummyComposable.dll");

            var assemblyComposer = new AssemblyComposer(container, container.Get<IModuleEnumerator>());
            assemblyComposer.Compose();

            Assert.Equal("Test", container.Get<IDummyComposable>().Test);
        }

        [Fact]
        public void AssemblyComposer_InvalidComposeTest()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var moduleDirectory = appDataDirectory.CreateSubdirectory("modules");

            Stream composableAssemblyZipStream =
                TestUtilities.GetResource("Loader.Snowflake.Framework.Tests.InvalidComposable.zip");
            ZipArchive archive = new ZipArchive(composableAssemblyZipStream);

            archive.ExtractToDirectory(moduleDirectory.CreateSubdirectory("Snowflake.Framework.Tests").FullName);
            var container = new ServiceContainer(appDataDirectory.FullName);

            Assert.Contains(container.Get<IModuleEnumerator>()
                .Modules, m => m.Entry == "Snowflake.Framework.Tests.InvalidComposable.dll");

            var assemblyComposer = new AssemblyComposer(container, container.Get<IModuleEnumerator>());
            assemblyComposer.Compose();
            Assert.Null(container.Get<IInvalidService>());
        }

        [Fact]
        public void AssemblyComposer_InvalidBruteforceComposeTest()
        {
            var appDataDirectory = new DirectoryInfo(Path.GetTempPath())
                .CreateSubdirectory(Guid.NewGuid().ToString());
            var moduleDirectory = appDataDirectory.CreateSubdirectory("modules");

            Stream composableAssemblyZipStream =
                TestUtilities.GetResource("Loader.Snowflake.Framework.Tests.InvalidComposable.zip");
            ZipArchive archive = new ZipArchive(composableAssemblyZipStream);

            archive.ExtractToDirectory(moduleDirectory.CreateSubdirectory("Snowflake.Framework.Tests").FullName);
            var container = new ServiceContainer(appDataDirectory.FullName);

            var module = container.Get<IModuleEnumerator>()
                .Modules.FirstOrDefault(m => m.Entry == "Snowflake.Framework.Tests.InvalidComposable.dll");
            Assert.NotNull(module);

            var assemblyComposer = new AssemblyComposer(container, container.Get<IModuleEnumerator>());
            assemblyComposer.Compose();
            Assert.Null(container.Get<IInvalidService>());

            var invalidComposable = new InvalidDummyServiceComposable();
            Assert.Throws<InvalidOperationException>(() =>
            {
                invalidComposable.Compose(module,
                    new ServiceProvider(container, new List<string>() {typeof(IServiceRegistrationProvider).FullName}));
            });
        }
    }
}
