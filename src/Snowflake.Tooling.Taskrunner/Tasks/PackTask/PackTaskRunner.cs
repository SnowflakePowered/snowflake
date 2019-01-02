using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemlyModuleBuilderTask;

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
                throw new InvalidOperationException("Must specify a module to pack.");
            }

            Console.WriteLine($"Packing {arguments.ModuleDirectory}...");
            var moduleDirectory = new DirectoryInfo(Path.GetFullPath(arguments.ModuleDirectory));
            if (!DirectoryProvider.IsModuleDirectory(moduleDirectory))
            {
                throw new InvalidDataException(
                    "Error! No valid module.json  or contents found. Check for JSON errors or missing file.");
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
