using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Snowflake.Loader
{
    internal class ModuleEnumerator : IModuleEnumerator
    {
        private DirectoryInfo ModuleDirectory { get; }

        /// <inheritdoc/>
        public IEnumerable<IModule> Modules { get; }

        public ModuleEnumerator(string appDataDirectory)
        {
            this.ModuleDirectory = new DirectoryInfo(Path.Combine(appDataDirectory, "modules"));
            this.ModuleDirectory.Create();
            this.Modules = this.EnumerateModules().ToList();
        }

        private static readonly JsonSerializerOptions serializationOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private IEnumerable<IModule> EnumerateModules()
        {
            return from directory in this.ModuleDirectory.EnumerateDirectories()
                where File.Exists(Path.Combine(directory.FullName, "module.json"))
                select JsonSerializer.Deserialize<ModuleDefinition>(
                        File.ReadAllText(Path.Combine(directory.FullName, "module.json")), 
                        serializationOptions)
                    .ToModule(directory);
        }
    }
}
