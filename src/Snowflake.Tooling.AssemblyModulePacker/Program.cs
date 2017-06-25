using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Snowflake.Tooling.AssemblyModulePacker
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => Program.MainAsync(args)).Wait();
        }

        public static async Task MainAsync(string[] args)
        {

            Console.WriteLine($"Snowball Assembly Packer version {Assembly.GetEntryAssembly().GetName().Version}");
            var stateMachine = new StateMachine();
            var cwd = DirectoryProvider.WorkingDirectory;
            if (!DirectoryProvider.IsProjectDirectory(cwd))
            {
                await stateMachine.ExitWithState("No valid project file found.", 1);
                return;
            }

            if (!DirectoryProvider.IsModuleDirectory(cwd))
            {
                await stateMachine.ExitWithState("No valid module.json found. Check for JSON errors or missing file.", 1);
                return;
            }
            //todo: migrate to use states!
            (var projectFile, var moduleFile) = DirectoryProvider.GetProjectFiles(cwd);
            ModuleDefinition module;

            try
            {
                module = JsonConvert.DeserializeObject<ModuleDefinition>(File.ReadAllText(moduleFile?.FullName));
            }
            catch
            {
                await stateMachine.ExitWithState("No valid module.json found. Check for JSON errors or missing file.", 1);
                return;
            }

            if (!module.Entry.EndsWith(".dll") || module.Loader != "assembly")
            {
                await stateMachine.ExitWithState("Module is not a proper assembly module, can not pack non-assembly modules!", 1);
                return;
            }

            string assemblyName = (from groups in XDocument.Parse(File.ReadAllText(projectFile.FullName)).Root.Descendants()
                                   from element in groups.Descendants()
                                   where element.Name.LocalName == "AssemblyName"
                                   select element.Value).FirstOrDefault() ?? Path.GetFileNameWithoutExtension(projectFile.Name);

         
            if(assemblyName != Path.GetFileNameWithoutExtension(module.Entry))
            {
                await stateMachine.ExitWithState($"Entry point {module.Entry} is not consistent with output assembly name {assemblyName}!", 1);
                return;
            }

            await stateMachine.Transition($"Found module {module.Entry}");
            var builder = new DotNetBuilder(module, projectFile, args);
            DirectoryInfo buildResult = await stateMachine.Transition<DirectoryInfo>(async () =>
               {
                   try
                   {
                       return buildResult = await builder.Build();
                   }
                   catch (Exception ex)
                   {
                       await stateMachine.ExitWithState(ex.Message, 1);
                       return null;
                   }
               }, "Cleaning and building module...");
            await stateMachine.ExitWithState($"Finished building module at {Environment.NewLine} {buildResult.FullName}");

        }
    }
}