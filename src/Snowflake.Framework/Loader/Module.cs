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

        public Module(string name, string displayName, string entry, string loader, string author, DirectoryInfo moduleDirectory,
            Version version)
        {
            this.Name = name;
            this.DisplayName = displayName;
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
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Entry { get; set; }
        public string Loader { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public IModule ToModule(DirectoryInfo moduleDirectory) => new Module(this.Name,
            this.DisplayName ?? this.Name,
            this.Entry,
            this.Loader,
            this.Author,
            moduleDirectory,
            System.Version.Parse(this.Version));
    }
}
