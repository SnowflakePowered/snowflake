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

        /// <inheritdoc />
        public string DisplayName { get; }

        /// <inheritdoc />
        public dynamic? LoaderOptions { get; }

        public Module(string name, string displayName, string entry, string loader, string author, DirectoryInfo moduleDirectory,
            Version version, dynamic? loaderOptions)
        {
            this.Name = name;
            this.DisplayName = displayName;
            this.Entry = entry;
            this.Loader = loader;
            this.Author = author;
            this.ModuleDirectory = moduleDirectory;
            this.ContentsDirectory = moduleDirectory.CreateSubdirectory("contents");
            this.Version = version;
            this.LoaderOptions = loaderOptions;
        }
    }

    internal class ModuleDefinition
    {
        private string Name { get; }
        private string DisplayName { get; }
        private string Entry { get; }
        private string Loader { get; }
        private string FrameworkVersion { get; }
        private string Version { get; }
        private string Author { get; }
        private dynamic LoaderOptions { get; }
        public ModuleDefinition(string name,
            string displayName,
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
            this.DisplayName = displayName;
            this.FrameworkVersion = frameworkVersion;
            this.LoaderOptions = loaderOptions;
        }

        public IModule ToModule(DirectoryInfo moduleDirectory) => new Module(this.Name,
            this.DisplayName ?? this.Name,
            this.Entry,
            this.Loader,
            this.Author,
            moduleDirectory,
            System.Version.Parse(this.Version),
            this.LoaderOptions);
    }
}
