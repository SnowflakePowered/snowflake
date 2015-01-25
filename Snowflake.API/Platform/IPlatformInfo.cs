using System;
using System.Collections.Generic;
using Snowflake.Controller;
using Snowflake.Information;

namespace Snowflake.Platform
{
    /// <summary>
    /// Represents an emulated console or a platform in Snowflake.
    /// </summary>
    public interface IPlatformInfo : IInfo
    {
        /// <summary>
        /// The defaults used to populate IPlatformPreferenceDatabase entry
        /// </summary>
        IPlatformDefaults Defaults { get; set; }
        /// <summary>
        /// The file extensions ROMs of this platform are known to have.
        /// </summary>
        IList<string> FileExtensions { get; }
        /// <summary>
        /// The maximum amount of inputs that are physically possible for this platform to have.
        /// </summary>
        int MaximumInputs { get; }
        /// <summary>
        /// A list of valid controllers for this platform
        /// </summary>
        IList<string> Controllers { get; }
        /// <summary>
        /// The controller ports
        /// </summary>
        IPlatformControllerPorts ControllerPorts { get; }
    }
}
