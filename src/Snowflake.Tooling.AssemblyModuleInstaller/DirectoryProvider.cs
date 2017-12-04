using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Tooling.AssemblyModulePacker
{
    internal static class DirectoryProvider
    {
        public static DirectoryInfo WorkingDirectory => new DirectoryInfo(Directory.GetCurrentDirectory());

        public static DirectoryInfo GetDefaultModuleInstallDirectory => new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
    .CreateSubdirectory("snowflake").CreateSubdirectory("modules");
        public static DirectoryInfo GetBuildDirectory(string moduleName) => new DirectoryInfo(Directory.GetCurrentDirectory()).CreateSubdirectory("bin")
            .CreateSubdirectory("module").CreateSubdirectory(moduleName);

        public static bool IsProjectDirectory(DirectoryInfo directory) => directory.EnumerateFiles().Any(p => p.Extension.ToLower() == ".csproj");

        public static bool IsModuleDirectory(DirectoryInfo directory) => directory.EnumerateFiles().Any(p => p.Name == "module.json");

        private static FileInfo GetProject(DirectoryInfo directory) => directory.EnumerateFiles().FirstOrDefault(p => p.Extension.ToLower() == ".csproj");

        private static FileInfo GetModule(DirectoryInfo directory) => directory.EnumerateFiles().FirstOrDefault(p => p.Name == "module.json");

        public static (FileInfo Project, FileInfo Module) GetProjectFiles(DirectoryInfo directory) => (DirectoryProvider.GetProject(directory), DirectoryProvider.GetModule(directory));

    }
}
