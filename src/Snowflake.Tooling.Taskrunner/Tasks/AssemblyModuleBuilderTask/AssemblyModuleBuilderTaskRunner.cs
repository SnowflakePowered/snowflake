﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Snowflake.Tooling.Taskrunner.Framework.Attributes;
using Snowflake.Tooling.Taskrunner.Framework.Tasks;
using Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks;

namespace Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTask
{
    [Task("build", "Builds the module in the current directory or specified folder.")]
    public class AssemblyModuleBuilderTaskRunner : TaskRunner<AssemblyModuleBuilderTaskArguments>
    {
        public override async Task<int> Execute(AssemblyModuleBuilderTaskArguments arguments, string[] args)
        {
            Console.WriteLine($"Snowball Assembly Builder version {Assembly.GetEntryAssembly().GetName().Version}");
            var cwd = arguments.SourceDirectory != null
                ? new DirectoryInfo(Path.GetFullPath(arguments.SourceDirectory))
                : DirectoryProvider.WorkingDirectory;
            Console.WriteLine($"Attempting to build module in {cwd.FullName}...");
            if (!DirectoryProvider.IsProjectDirectory(cwd))
            {
                throw new FileNotFoundException("Error! No valid project file found.");
            }

            if (!DirectoryProvider.IsModuleDirectory(cwd))
            {
                throw new InvalidDataException(
                    "Error! No valid module.json found. Check for JSON errors or missing file.");
            }

            (var projectFile, var moduleFile) = DirectoryProvider.GetProjectFiles(cwd);
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

            if (!module.Entry.EndsWith(".dll") || module.Loader != "assembly")
            {
                throw new InvalidDataException(
                    "Error! Module is not a proper assembly module, can not pack non-assembly modules!");
            }

            var projectXml = XDocument.Parse(File.ReadAllText(projectFile.FullName)).Root.Descendants();
            string assemblyName = (from groups in projectXml
                                      from element in groups.Descendants()
                                      where element.Name.LocalName == "AssemblyName"
                                      select element.Value).FirstOrDefault() ??
                                  Path.GetFileNameWithoutExtension(projectFile.Name);

            if (assemblyName != Path.GetFileNameWithoutExtension(module.Entry))
            {
                throw new InvalidOperationException(
                    $"Error! Entry point {module.Entry} is not consistent with output assembly name {assemblyName}!");
            }

            string targetFramework = (from groups in projectXml
                from element in groups.Descendants()
                where element.Name.LocalName == "TargetFramework"
                select element.Value).FirstOrDefault();

            if (targetFramework != "net5.0")
            {
                throw new InvalidOperationException($"Error! Assembly modules must target framework net5.0");
            }

            Console.WriteLine($"Found \"{module.Name}\" with entry {module.Entry}");
            var builder = new DotNetBuilder(module, projectFile, arguments.OutputDirectory, arguments.ReleaseBuild, arguments.MsbuildArgs);

            Console.WriteLine("Cleaning and building module...");

            try
            {
                await builder.Build();
                Console.WriteLine($"Finished building module {module.Entry}");
                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to build module due to {ex.Message}", ex);
            }
        }
    }
}
