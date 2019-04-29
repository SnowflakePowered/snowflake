using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks;
using Snowflake.Tooling.Taskrunner.Tasks.InstallTask;

namespace Snowflake.Tooling.Taskrunner.Tasks.InstallProjectTask
{
    // [Task("install-project", "Installs the built module project in the working directory.")]
    public class InstallProjectTaskRunner : TaskRunner<InstallProjectTaskArguments>
    {
        public async override Task<int> Execute(InstallProjectTaskArguments arguments, string[] args)
        {
            // todo: finish this.
            return 1;
            DirectoryInfo moduleDirectory = arguments.ModuleDirectory != null
            ? new DirectoryInfo(Path.GetFullPath(arguments.ModuleDirectory))
            : PathUtility.GetDefaultModulePath();

            (FileInfo Project, FileInfo Module) = DirectoryProvider.GetProjectFiles(DirectoryProvider.WorkingDirectory);
            if (!Project.Exists || !Module.Exists) throw new FileNotFoundException("Unable to find project file or module.json.");

            var artifactsDirectory = DirectoryProvider.WorkingDirectory
                  .CreateSubdirectory("bin")
                  .CreateSubdirectory("module")
                  .CreateSubdirectory(Path.GetFileNameWithoutExtension(Project.Name));

            Console.WriteLine($"Installing {artifactsDirectory.Name}...");

            if (!DirectoryProvider.IsModuleDirectory(artifactsDirectory))
            {
                throw new InvalidDataException(
                    "Error! No valid module.json or contents found. Check for JSON errors or missing file. ");
            }

            var moduleFile = DirectoryProvider.GetModule(artifactsDirectory);
            ModuleDefinition module;

            try
            {
                module = JsonConvert.DeserializeObject<ModuleDefinition>(File.ReadAllText(moduleFile?.FullName));
            }
            catch
            {
                throw new InvalidDataException(
                    "Error! No valid module.json found. Check for JSON errors or missing file.");
            }

            var installer = new ProjectInstaller(module, artifactsDirectory);
            Console.WriteLine($"Installed to {await installer.CopyModuleFiles(moduleDirectory, arguments.NoTreeShaking)}");

            return 0;
        }
    }
}
