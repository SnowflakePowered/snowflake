using System;
using System.Collections.Generic;
using Snowflake.Emulator;

namespace Snowflake.Service.Manager
{
    /// <summary>
    /// The Emulator Assemblies manager manages emulator assemblies
    /// </summary>
    public interface IEmulatorAssembliesManager
    {
        /// <summary>
        /// The location of the emulator assemblies
        /// </summary>
        string AssembliesLocation { get; }
        /// <summary>
        /// The emulator assemblies
        /// </summary>
        IReadOnlyDictionary<string, IEmulatorAssembly> EmulatorAssemblies { get; }
        /// <summary>
        /// Gets the directory of an emulator assembly
        /// </summary>
        /// <param name="assembly">The emulator assembly</param>
        /// <returns>The directory of the emulator assembly</returns>
        string GetAssemblyDirectory(IEmulatorAssembly assembly);
        /// <summary>
        /// Loads all emulator assemblies
        /// </summary>
        void LoadEmulatorAssemblies();
    }
}
