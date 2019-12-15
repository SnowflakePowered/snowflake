using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Orchestration.SystemFiles;
using Snowflake.Filesystem;
using Snowflake.Loader;
using Zio;

namespace Snowflake.Services
{
    public class StoneProviderComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IFileSystem))]
        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
            var stone = new StoneProvider();

            var physicalFs = serviceContainer.Get<IFileSystem>();
            var contentDirectory = serviceContainer.Get<IContentDirectoryProvider>();
            var appDataPath = physicalFs.ConvertPathFromInternal(contentDirectory.ApplicationData.FullName);
            var biosFs = physicalFs.GetOrCreateSubFileSystem(appDataPath / "bios");
            var systemFiles = new SystemFileProvider(new Directory(biosFs), stone);

            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IStoneProvider>(stone);
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<ISystemFileProvider>(systemFiles);

            // Generate all the directories
            foreach (var platform in stone.Platforms)
            {
                systemFiles.GetSystemFileDirectory(platform.Key);
            }
        }
    }
}
