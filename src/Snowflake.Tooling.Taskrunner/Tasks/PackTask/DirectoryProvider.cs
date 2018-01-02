using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Tasks.PackTask
{
    internal static class DirectoryProvider
    {
        public static DirectoryInfo WorkingDirectory => new DirectoryInfo(Directory.GetCurrentDirectory());

        public static bool IsModuleDirectory(DirectoryInfo directory) 
            => directory.EnumerateFiles().Any(p => p.Name == "module.json")
            && directory.EnumerateDirectories().Any(p => p.Name == "contents");

        public static FileInfo GetModule(DirectoryInfo directory) => directory.EnumerateFiles().FirstOrDefault(p => p.Name == "module.json");
    }
}
