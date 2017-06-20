using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;
using Snowflake.Extensibility;
using Snowflake.Utility;

namespace Snowflake.Framework.Loader
{
    public class AssemblyModuleLoader
    {
        private DirectoryInfo ModuleDirectory { get; }
        public IEnumerable<Module> Modules { get; set; }
        public AssemblyModuleLoader(string appDataDirectory)
        {
            this.ModuleDirectory = new DirectoryInfo(Path.Combine(appDataDirectory, "modules"));
            this.Modules = this.EnumerateModules().ToList();
        }

        private IEnumerable<Module> EnumerateModules()
        {
            return (from directory in this.ModuleDirectory.EnumerateDirectories()
                    where File.Exists(Path.Combine(directory.FullName, "entry"))
                    select new Module {
                        Name = directory.Name,
                        Entry = File.ReadAllText(Path.Combine(directory.FullName, "entry")),
                        ModuleDirectory = directory
                    });
        }

        public IEnumerable<IPluginContainer> InitializePluginModule(Module module)
        {
            Console.WriteLine($"Loading module {module.Entry}");
            try
            {
                var deps = module.ModuleDirectory.EnumerateDirectories().First(d => d.Name == "contents");
            }catch(InvalidOperationException ex)
            {
                throw new DirectoryNotFoundException($"Unable to find module contents for {module.Entry}", ex);
            } 
            var loadContext = new ModuleLoadContext(module);
            //todo: check for semver!!
            var entryPath = Path.Combine(module.ModuleDirectory.FullName, "contents", module.Entry);

            if (!File.Exists(entryPath))
            {
                throw new FileNotFoundException($"Unable to find specified entry point {module.Entry}");
            }

            var assembly = loadContext.LoadFromAssemblyPath(entryPath);

            var types = assembly.ExportedTypes
                    .Where(t => t.GetInterfaces().Contains(typeof(IPluginContainer)))
                    .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var type in types)
            {
                var container = Instantiate.CreateInstance(type) as IPluginContainer;
                Console.WriteLine($"Found Plugin Container {container.GetType().Name}");
                yield return container;
            }
        }
    }
}
