using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Loader
{
    public class Module : IModule
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <inheritdoc/>
        public string Entry { get; }

        /// <inheritdoc/>
        public string Loader { get; }

        /// <inheritdoc/>
        public string Author { get; }

        /// <inheritdoc/>
        public DirectoryInfo ModuleDirectory { get; }

        /// <inheritdoc/>
        public DirectoryInfo ContentsDirectory { get; }

        /// <inheritdoc/>
        public Version Version { get; }

        public Module(string name, string entry, string loader, string author, DirectoryInfo moduleDirectory,
            Version version)
        {
            this.Name = name;
            this.Entry = entry;
            this.Loader = loader;
            this.Author = author;
            this.ModuleDirectory = moduleDirectory;
            this.ContentsDirectory = moduleDirectory.CreateSubdirectory("contents");
            this.Version = version;
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
            this.Version = version;
            this.Author = author;
            this.FrameworkVersion = frameworkVersion;
        }

        public IModule ToModule(DirectoryInfo moduleDirectory) => new Module(this.Name,
            this.Entry,
            this.Loader,
            this.Author,
            moduleDirectory,
            System.Version.Parse(this.Version));
    }
}
