using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Snowflake.AssemblyLoader.Utility;
using Snowflake.Loader;
using Snowflake.Services.Logging;

namespace Snowflake.Services.AssemblyLoader
{
    internal class AssemblyModuleLoader : IModuleLoader<IComposable>
    {
        public AssemblyModuleLoader()
        {
        }

        /// <inheritdoc/>
        public IEnumerable<IComposable> LoadModule(IModule module)
        {
            var logger = new LogProvider().GetLogger("AssemblyComposer"); // Unknown if logging service is available.

            logger.Info($"Loading module {module.Entry}");
            try
            {
                var deps = module.ModuleDirectory.EnumerateDirectories().First(d => d.Name == "contents");
            }
            catch (InvalidOperationException ex)
            {
                throw new DirectoryNotFoundException($"Unable to find module contents for {module.Entry}", ex);
            }

            var loadContext = new AssemblyModuleLoadContext(module);

            // todo: check for semver!!
            var entryPath = Path.Combine(module.ModuleDirectory.FullName, "contents", module.Entry);

            if (!File.Exists(entryPath))
            {
                throw new FileNotFoundException($"Unable to find specified entry point {module.Entry}");
            }

            var assembly = loadContext.LoadFromAssemblyPath(entryPath);
            IEnumerable<Type> types;
            try
            {
                types = assembly.ExportedTypes
                        .Where(t => t.GetInterfaces().Contains(typeof(IComposable)))
                        .Where(t => t.GetConstructor(Type.EmptyTypes) != null);
            }
            catch (TypeLoadException ex)
            {
                throw new TypeLoadException($"Unable to load {module.Entry}, are you sure it is compatible with this version of Snowflake?", ex);
            }

            foreach (var type in types)
            {
                var container = Instantiate.CreateInstance(type);
                var castedContainer = (IComposable)container;
                Console.WriteLine($"Found Container {container.GetType().Name}");
                yield return castedContainer;
            }
        }
    }
}
