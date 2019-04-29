using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Tooling.Taskrunner.Tasks.AssemblyModuleBuilderTasks
{
    internal class ModuleDefinition
    {
        public string Name { get; }
        public string Entry { get; }
        public string Loader { get; }
        public string FrameworkVersion { get; }
        public string Version { get; }
        public string Author { get; }

        public ModuleDefinition(string name,
            string entry,
            string loader,
            string frameworkVersion,
            string author,
            string version)
        {
            this.Name = name;
            this.Entry = entry;
            this.Loader = loader;
            this.Version = version;
            this.Author = author;
            this.FrameworkVersion = frameworkVersion;
        }
    }
}
