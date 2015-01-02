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
        /// The controllers in a platform.
        /// </summary>
        IReadOnlyDictionary<string, IControllerDefinition> Controllers { get; }
        IPlatformDefaults Defaults { get; set; }
        /// <summary>
        /// The file extensions ROMs of this platform are known to have.
        /// </summary>
        IList<string> FileExtensions { get; }
        /// <summary>
        /// The maximum amount of inputs that are physically possible for this platform to have.
        /// </summary>
        int MaximumInputs { get; }
    }
}
