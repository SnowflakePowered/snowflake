using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Framework.Loader
{
    public class Module
    {
        public string Name { get; }
        public string Entry { get; }
        public string Loader { get; }
        public dynamic LoaderOptions { get; }
        public DirectoryInfo ModuleDirectory { get; }
        public Module(string name, string entry, string loader, dynamic loaderOptions, DirectoryInfo moduleDirectory) {
            this.Name = name;
            this.Entry = entry;
            this.Loader = loader;
            this.LoaderOptions = loaderOptions;
            this.ModuleDirectory = moduleDirectory;
        }
    }

    internal class ModuleDefinition
    {
        private string Name { get; }
        private string Entry { get; }
        private string Loader { get; }
        private string FrameworkVersion { get; }
        private string Version { get; }
        private string Author { get; }
        private dynamic LoaderOptions { get; }
        public ModuleDefinition(string name,
            string entry, 
            string loader, 
            string frameworkVersion, 
            string author, 
            string version,
            dynamic loaderOptions)
        {
            this.Name = name;
            this.Entry = entry;
            this.Loader = loader;
            this.LoaderOptions = loaderOptions;
        }

        public Module ToModule(DirectoryInfo moduleDirectory) => new Module(this.Name, this.Entry, this.Loader, this.LoaderOptions, moduleDirectory);
    }
}
