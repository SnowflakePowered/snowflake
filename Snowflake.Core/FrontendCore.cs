using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Information;
using Snowflake.API.Interface;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Newtonsoft.Json;

namespace Snowflake.Core
{
    public class FrontendCore
    {
        public Dictionary<string, Platform> LoadedPlatforms { get; private set; }
        public string AppDataDirectory { get; private set; }

        public Dictionary<string, IIdentifier> LoadedIdentifiers { get; private set; }
        public Dictionary<string, IEmulator> LoadedEmulators { get; private set; }
        public Dictionary<string, IScraper> LoadedScrapers { get; private set; }

     //   [ImportMany (typeof(IIdentifier))]
        //private List<IPlugin> LoadedPlugins { get; private set; }

        [Import(typeof(IIdentifier))]
        public IIdentifier datIdentifier;

        public FrontendCore() : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake")) { }
        public FrontendCore(string appDataDirectory)
        {
            this.AppDataDirectory = appDataDirectory;
            ///this.LoadedPlatforms = this.LoadPlatforms(Path.Combine(this.AppDataDirectory, "platforms"));

        }

        private Dictionary<string, Platform> LoadPlatforms(string platformDirectory)
        {
            var loadedPlatforms = new Dictionary<string, Platform>();

            foreach (string fileName in Directory.GetFiles(platformDirectory))
            {
                if (Path.GetExtension(fileName) == ".platform")
                {
                    try
                    {
                        var platform = JsonConvert.DeserializeObject<Platform>(File.ReadAllText(fileName));
                        loadedPlatforms.Add(platform.PlatformId, platform);
                    }
                    catch (Exception)
                    {
                        //log
                        Console.WriteLine("Exception occured when importing platform " + fileName);
                        continue;
                    }
                }
            }
            return loadedPlatforms;

        }

       

    }
}
