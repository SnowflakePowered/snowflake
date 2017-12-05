using Newtonsoft.Json;
using Snowflake.Tooling.AssemblyModulePacker;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Snowflake.Tooling.AssemblyModuleInstaller
{
    static class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() => Program.MainAsync(args)).Wait();
        }
        public static void ExitWithState(string stateMessage, int exitCode = 0)
        {
            Console.WriteLine(stateMessage);
            Environment.Exit(exitCode);
        }
        public static async Task MainAsync(string[] args)
        {
            // todo: this code is real shit.
            Console.WriteLine($"Snowball Assembly Installer version {Assembly.GetEntryAssembly().GetName().Version}");
            var cwd = DirectoryProvider.WorkingDirectory;
            if (!DirectoryProvider.IsProjectDirectory(cwd))
            {
                ExitWithState("Error! No valid project file found.", 1);
                return;
            }

            if (!DirectoryProvider.IsModuleDirectory(cwd))
            {
                ExitWithState("Error! No valid module.json found. Check for JSON errors or missing file.", 1);
                return;
            }
            (var projectFile, var moduleFile) = DirectoryProvider.GetProjectFiles(cwd);
            ModuleDefinition module;

            try
            {
                module = JsonConvert.DeserializeObject<ModuleDefinition>(File.ReadAllText(moduleFile?.FullName));
            }
            catch
            {
                ExitWithState("Error! No valid module.json found. Check for JSON errors or missing file.", 1);
                return;
            }

            if (!module.Entry.EndsWith(".dll") || module.Loader != "assembly")
            {
                ExitWithState("Error! Module is not a proper assembly module, can not pack non-assembly modules!", 1);
                return;
            }
            Console.WriteLine($"Found module {module.Entry}");

            var buildDir = DirectoryProvider.GetBuildDirectory(Path.GetFileNameWithoutExtension(module.Entry));
            if (!buildDir.Exists)
            {
                ExitWithState("Error! Module was not built! Build the module before installing!", 1);
                return;
            }

            var installedDir = DirectoryProvider.GetDefaultModuleInstallDirectory.CreateSubdirectory(Path.GetFileNameWithoutExtension(module.Entry));
            Console.WriteLine("Removing old installation from modules...");
            try
            {
                installedDir.Delete(true);
            }
            catch (IOException ex)
            {
                ExitWithState("Unable to clean output directory, is it in use?");
            }
            Console.WriteLine("Installing new installation...");
            try
            {
                CopyDir.CopyAll(buildDir, installedDir);

            }catch(IOException ex)
            {
                ExitWithState("Unable to install, is it in use?");
            }
        }
    }
}
