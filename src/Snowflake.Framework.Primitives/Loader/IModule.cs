using System;
using System.IO;

namespace Snowflake.Loader
{
    /// <summary>
    /// Represents the module information for a module folder.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Gets the author of the module as specified in the module manifest.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the entrypoint of the module as specified in the module manifest.
        /// </summary>
        string Entry { get; }

        /// <summary>
        /// Gets the loader of the module as specified in the module manifest.
        /// </summary>
        string Loader { get; }

        /// <summary>
        /// Gets the directory whether the module contents folder is contained.
        /// </summary>
        DirectoryInfo ModuleDirectory { get; }

        /// <summary>
        /// Gets the directory where the module contents are located.
        /// </summary>
        DirectoryInfo ContentsDirectory { get; }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The version of this module.
        /// </summary>
        Version Version { get; }
    }
}
