using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks;
using Snowflake.Tooling.Taskrunner.Tasks.PackTask;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallProjectTask
{
    public class ProjectInstaller
    {
        private ModuleDefinition ModuleDefinition { get; }
        private DirectoryInfo ModuleDirectory { get; }

        internal ProjectInstaller(ModuleDefinition moduleDefinition, DirectoryInfo artifactsDirectory)
        {
            this.ModuleDefinition = moduleDefinition;
            this.ModuleDirectory = artifactsDirectory;
        }

        public string GetPackageName() => $"{this.ModuleDefinition.Loader}.{this.ModuleDirectory.Name}";

        public async Task<string> CopyModuleFiles(DirectoryInfo installDirectory, bool noTreeShaking)
        {
            var targetDirectory = installDirectory.CreateSubdirectory(this.GetPackageName());

            var treeShaker = noTreeShaking
             ? Enumerable.Empty<string>()
             : new AssemblyTreeShaker()
                 .GetFrameworkDependencies(this.ModuleDirectory, this.ModuleDefinition);

            return targetDirectory.FullName;
        }
    }
}
