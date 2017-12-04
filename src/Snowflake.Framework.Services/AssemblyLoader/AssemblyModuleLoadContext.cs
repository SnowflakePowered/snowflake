using Microsoft.Extensions.DependencyModel;
using Snowflake.Loader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Snowflake.Extensibility;
using Snowflake.Services.Logging;
using System.Collections.Immutable;

namespace Snowflake.Services.AssemblyLoader
{
    internal class AssemblyModuleLoadContext : AssemblyLoadContext
    {
        private string folderPath;
        private readonly ILogger logger;
        private static IImmutableDictionary<(string name, Version unsignedVersion), Assembly> unsignedAssemblies;
        static AssemblyModuleLoadContext()
        {
            AssemblyModuleLoadContext.unsignedAssemblies = ImmutableDictionary.Create<(string name, Version unsignedVersion),
                Assembly>();
        }
        private AssemblyModuleLoadContext(string folderPath)
        {
            this.logger = new LogProvider().GetLogger("AssemblyComposer"); //Unknown if logging service is available.
            this.folderPath = folderPath;
        }

        public AssemblyModuleLoadContext(IModule module) : this(module.ContentsDirectory.FullName)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            logger.Info($"Attempting to load {assemblyName.Name}");
            if (assemblyName.GetPublicKeyToken().Length == 0 && unsignedAssemblies.ContainsKey((assemblyName.Name, assemblyName.Version)))
            {
                logger.Warn($"Resolving {assemblyName.Name} version {assemblyName.Version} from unsigned assembly cache.");
                return AssemblyModuleLoadContext.unsignedAssemblies[(assemblyName.Name, assemblyName.Version)];
            }
            var deps = DependencyContext.Default;
            var resources = deps.RuntimeLibraries.Where(d => d.Name.ToLowerInvariant() == assemblyName.Name.ToLower()).ToList();

            if (assemblyName.Name == "Snowflake.Framework.Primitives")
            {
                Version supportedVersion = typeof(IServiceContainer).GetTypeInfo().Assembly.GetName().Version;
                Console.WriteLine($"Found Snowflake Framework Version {assemblyName.Version}");
                if(assemblyName.Version.Major != supportedVersion.Major)
                {
                    //todo: more robust version check
                    throw new InvalidOperationException("Framework Version Mismatch! Please upgrade your plugin to the newest Snowflake Framework API!");
                }
            }

            //todo: use .netstandard 2.0 AssemblyLoadContext.GetLoadedAssemblies()
            
            var runtimeLibs = deps.RuntimeLibraries.Select(lib => new { lib.Name, lib.Version }).ToList();
            if (runtimeLibs.Select(l => l.Name.ToLower()).Contains(assemblyName.Name.ToLower()))
            {
                logger.Info($"Attempting to resolve {assemblyName.Name} version {assemblyName.Version} from runtime...");
                try
                {
                    return Assembly.Load(assemblyName);
                }
                catch
                {
                    logger.Warn($"Unable to find the proper version of {assemblyName.Name} loaded in runtime.");
                    var prepAssembly = Assembly.Load(new AssemblyName(assemblyName.Name)); 
                    if (assemblyName.Version.Major == prepAssembly.GetName().Version.Major)
                    {
                        logger.Warn($"Resolving {assemblyName.Name} version {assemblyName.Version} with mismatched minor version {prepAssembly.GetName().Version} from runtime.");
                        logger.Info($"If this behaviour causes side effects, build against {prepAssembly.GetName().Version}.");
                        return prepAssembly;
                    }
                    
                }
            }

            if (resources.Count > 0)
            {
                return Assembly.Load(new AssemblyName(resources.First().Name));
            }
            else
            {
                // todo: load from default
                var dependencyFileInfo = new FileInfo($"{Path.Combine(this.folderPath, assemblyName.Name)}.dll");
                if (File.Exists(dependencyFileInfo.FullName))
                {
                    var dependencyLoadContext = new AssemblyModuleLoadContext(dependencyFileInfo.DirectoryName);
                    logger.Info($"Loading {assemblyName.Name} {assemblyName.Version} from module dependencies");
                    var loadedDependency = dependencyLoadContext.LoadFromAssemblyPath(dependencyFileInfo.FullName);
                    var loadedDependencyName = loadedDependency.GetName();
                    if (loadedDependencyName.GetPublicKeyToken().Length == 0)
                    {
                        logger.Info($"Caching unsigned assembly {assemblyName.Name} {assemblyName.Version}.");
                        AssemblyModuleLoadContext.unsignedAssemblies =
                            unsignedAssemblies.Add((loadedDependencyName.Name, loadedDependencyName.Version), loadedDependency);
                    }
                    return loadedDependency;
                }
                else
                {
                    logger.Info($"Loading {assemblyName.Name} from GAC!");
                    return Assembly.Load(assemblyName);
                }
            }
        }
    }
}
