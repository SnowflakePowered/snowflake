using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Execution.Process;
using Snowflake.Extensibility;
using Snowflake.Framework.Remoting.Electron;
using Snowflake.Loader;
using Newtonsoft.Json;
using Snowflake.Support.Remoting.Electron.ThemeProvider;

namespace Snowflake.Support.Remoting.Electron
{
    public class ElectronAsarLoader : IModuleLoader<IElectronPackage>
    {
        private ILogger Logger { get; }

        public ElectronAsarLoader(ILogger logger)
        {
            this.Logger = logger;
        }

        public IEnumerable<IElectronPackage> LoadModule(IModule module)
        {
            FileInfo electronPackage = new FileInfo(Path.Combine(module.ContentsDirectory.FullName, module.Entry));
            if (!electronPackage.Exists)
            {
                throw new FileNotFoundException($"Electron ASAR {electronPackage.FullName} does not exist!");
            }

            this.Logger.Info($"Loading Electron ASAR Package {module.Name} {module.Version}...");
            string packagePath = Path.Combine(module.ContentsDirectory.FullName, module.Entry);
            string themeDefPath = Path.Combine(module.ContentsDirectory.FullName, "theme.json");

            if (!File.Exists(themeDefPath))
            {
                throw new FileNotFoundException(
                    $"Electron ASAR {electronPackage.FullName} does not contain a theme definition...");
            }

            var themeDef = JsonConvert.DeserializeObject<ThemeDefinition>(File.ReadAllText(themeDefPath));
            yield return new ElectronPackage()
            {
                Author = module.Author,
                PackagePath = Path.Combine(module.ContentsDirectory.FullName, module.Entry),
                Entry = themeDef.Entry ?? "index.html",
                Description = themeDef.Description,
                Icon = themeDef.Icon ?? "icon.png",
                Version = themeDef.Version,
                Name = themeDef.Name,
            };
        }
    }
}
