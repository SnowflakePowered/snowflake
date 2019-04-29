using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    [Task("pack", "Packs a module into a snowball distributable.")]
    public class PackTaskRunner : TaskRunner<PackTaskArguments>
    {
        public override async Task<int> Execute(PackTaskArguments arguments, string[] args)
        {
            Console.WriteLine($"Snowball Module Packer version {Assembly.GetEntryAssembly().GetName().Version}");

            if (arguments.ModuleDirectory == null)
            {
                (FileInfo Project, FileInfo Module) = DirectoryProvider.GetProjectFiles(DirectoryProvider.WorkingDirectory);
                if (!Project.Exists || !Module.Exists) throw new InvalidOperationException("Must specify a module to pack.");

                var currentModuleDir = DirectoryProvider.WorkingDirectory
                    .CreateSubdirectory("bin")
                    .CreateSubdirectory("module")
                    .CreateSubdirectory(Path.GetFileNameWithoutExtension(Project.Name));
                arguments.ModuleDirectory = currentModuleDir.FullName;
            }

            Console.WriteLine($"Packing {arguments.ModuleDirectory}...");
            var moduleDirectory = new DirectoryInfo(Path.GetFullPath(arguments.ModuleDirectory));
            if (!DirectoryProvider.IsModuleDirectory(moduleDirectory))
            {
                throw new InvalidDataException(
                    "Error! No valid module.json or contents found. Check for JSON errors or missing file. " +
                    "If the supplied argument is a project folder, run pack without arguments to pack the current project.");
            }

            var moduleFile = DirectoryProvider.GetModule(moduleDirectory);
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

            var packer = new ModulePacker(moduleDirectory, module);

            DirectoryInfo directory = arguments.OutputFile != null
                ? new DirectoryInfo(Path.GetFullPath(arguments.OutputFile))
                : DirectoryProvider.WorkingDirectory;
            Console.WriteLine($"Packed {await packer.PackArchive(directory)}");
            return 0;
        }
    }
}
