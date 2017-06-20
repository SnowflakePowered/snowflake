using Microsoft.Extensions.DependencyModel;
using Snowflake.Services;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Snowflake.Framework.Loader
{
    internal class ModuleLoadContext : AssemblyLoadContext
    {
        private string folderPath;
        private ModuleLoadContext(string folderPath)
        {
            this.folderPath = folderPath;
        }

        public ModuleLoadContext(Module module) : this(Path.Combine(module.ModuleDirectory.FullName, "contents"))
        {
        
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Console.WriteLine($"Attempting to load {assemblyName.Name}");
            var deps = DependencyContext.Default;
            var resources = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();

            if (assemblyName.Name == "Snowflake.Framework.Primitives")
            {
                Version supportedVersion = typeof(ICoreService).GetTypeInfo().Assembly.GetName().Version;
                Console.WriteLine($"Found Snowflake Framework Version {assemblyName.Version}");
                if(assemblyName.Version.Major != supportedVersion.Major)
                {
                    //todo: more robust version check
                    throw new InvalidOperationException("Framework Version Mismatch! Please upgrade your plugin to the newest Snowflake Framework API!");
                }
            }

            //todo: use .netstandard 2.0 AssemblyLoadContext.GetLoadedAssemblies()
            var compileLibs = deps.CompileLibraries.Select(lib => new { lib.Name, lib.Version }).ToList();
            if (compileLibs.Select(l => l.Name).Contains(assemblyName.Name.ToLower()))
            {
                Console.WriteLine($"Loading {assemblyName.Name} from Runtime Librairies");
                return Assembly.Load(assemblyName);
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
                    var dependencyLoadContext = new ModuleLoadContext(dependencyFileInfo.DirectoryName);
                    Console.WriteLine($"Loading {assemblyName.Name} from module dependencies");
                    return dependencyLoadContext.LoadFromAssemblyPath(dependencyFileInfo.FullName);
                } else
                {
                    Console.WriteLine($"Loading {assemblyName.Name} from GAC!");
                    return Assembly.Load(assemblyName);
                }
            }
        }
    }
}
