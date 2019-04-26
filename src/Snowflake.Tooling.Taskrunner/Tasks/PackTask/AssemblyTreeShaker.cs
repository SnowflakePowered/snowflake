using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyModel;
using Snowflake.Tooling.Taskrunner.Tasks.AssemlyModuleBuilderTask;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    public class AssemblyTreeShaker
    {
        internal IEnumerable<string> GetFrameworkDependencies(DirectoryInfo moduleDirectory,
            ModuleDefinition packageModule)
        {
            if (packageModule.Loader != "assembly") return Enumerable.Empty<string>();
            Console.WriteLine("Resolving dependencies from " + Path.GetFileNameWithoutExtension(packageModule.Entry) + ".deps.json");
            var deps = moduleDirectory.CreateSubdirectory("contents")
                .EnumerateFiles(Path.GetFileNameWithoutExtension(packageModule.Entry) + ".deps.json").FirstOrDefault();
            if (deps == null) return Enumerable.Empty<string>();

            using var reader = new DependencyContextJsonReader();
            var dependencyContext = reader.Read(deps.OpenRead());

            Console.WriteLine("Counted " + dependencyContext.RuntimeGraph.Select(p => p.Runtime).Count() + " runtime dependencies");
            IEnumerable<Dependency> frameworkDependencies =
                dependencyContext.RuntimeLibraries.Where(l => l.Name.StartsWith("Snowflake.Framework"))
                    .SelectMany(l => l.Dependencies);

            var dependencyTree = this.ResolveDependencyTree(dependencyContext, frameworkDependencies);
            var nativeDlls = frameworkDependencies.Concat(dependencyTree).Distinct()
                .Select(p => dependencyContext.RuntimeLibraries.FirstOrDefault(l => l.Name == p.Name))
                .SelectMany(p => p.NativeLibraryGroups.Concat(p.RuntimeAssemblyGroups))
                .SelectMany(p => p.AssetPaths)
                .Select(p => Path.GetFileName(p));
            var frameworkDlls = dependencyContext.RuntimeLibraries.Where(l => l.Name.StartsWith("Snowflake.Framework"))
                .Select(l => l.Name + ".dll");
            var dependencyDlls = dependencyTree.Select(d => d.Name + ".dll");
            return nativeDlls.Concat(dependencyDlls).Concat(frameworkDlls).Distinct().ToList();
        }

        internal IEnumerable<Dependency> ResolveDependencyTree(DependencyContext context, IEnumerable<Dependency> tree)
        {
            var frameworkDependencyDependencies =
                context.RuntimeLibraries.AsParallel()
                    .Where(l => tree.Select(t => t.Name).Contains(l.Name))
                    .Select(l => l.Dependencies)
                    .SelectMany(l => this.ResolveDependencyTree(context, l));

            return tree.Concat(frameworkDependencyDependencies);
        }
    }
}
