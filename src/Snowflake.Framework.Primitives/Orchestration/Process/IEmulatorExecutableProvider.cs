using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Orchestration.Process
{
    /// <summary>
    /// Provides access to loaded <see cref="IEmulatorExecutable"/> instances.
    /// </summary>
    public interface IEmulatorExecutableProvider
    {
        /// <summary>
        /// Gets any emulator with a matching name.
        /// </summary>
        /// <param name="name">The name of the emulator.</param>
        /// <returns>An <see cref="IEmulatorExecutable"/> that can start the given emulator.</returns>
        IEmulatorExecutable GetEmulator(string name);

        /// <summary>
        /// Gets an emulator with the matching name and closest major version.
        /// </summary>
        /// <param name="name">The name of the emulator.</param>
        /// <param name="semver">The requested version of the emulator.</param>
        /// <returns>An <see cref="IEmulatorExecutable"/> that is guaranteed to have
        /// at least the same major version as the requested version, or null if no
        /// <see cref="IEmulatorExecutable"/> are loaded with the requested name and version.</returns>
        IEmulatorExecutable GetEmulator(string name, Version semver);
    }
}
