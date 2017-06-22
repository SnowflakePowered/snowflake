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
using Newtonsoft.Json;

namespace Snowflake.Loader.ExtensibilityLoader
{
    internal class AssemblyModuleLoader : IModuleLoader<IComposer>
    {
        public IEnumerable<IComposer> LoadModule(IModule module)
        {
            Console.WriteLine($"Loading module {module.Entry}");
            try
            {
                var deps = module.ModuleDirectory.EnumerateDirectories().First(d => d.Name == "contents");
            }
            catch (InvalidOperationException ex)
            {
                throw new DirectoryNotFoundException($"Unable to find module contents for {module.Entry}", ex);
            }
            var loadContext = new AssemblyModuleLoadContext(module);
            //todo: check for semver!!
            var entryPath = Path.Combine(module.ModuleDirectory.FullName, "contents", module.Entry);

            if (!File.Exists(entryPath))
            {
                throw new FileNotFoundException($"Unable to find specified entry point {module.Entry}");
            }

            var assembly = loadContext.LoadFromAssemblyPath(entryPath);

            var types = assembly.ExportedTypes
                    .Where(t => t.GetInterfaces().Contains(typeof(IComposer)))
                    .Where(t => t.GetConstructor(Type.EmptyTypes) != null);

            foreach (var type in types)
            {
                var container = Instantiate.CreateInstance(type) as IComposer;
                Console.WriteLine($"Found Container {container.GetType().Name}");
                yield return container;
            }
        }
    }
}
