using System.Collections.Generic;

namespace Snowflake.Platform
{
    /// <summary>
    /// Represents an emulated console or a platform in Snowflake.
    /// </summary>
    public interface IPlatformInfo
    { 
        /// <summary>
        /// The id of the platform that this object is related to
        /// </summary>
        string PlatformID { get; }
        /// <summary>
        /// The friendly name of this platform
        /// </summary>
        string FriendlyName { get; }
        /// <summary>
        /// Any metadata that is attached to this object, such as descriptions
        /// </summary>
        IDictionary<string, string> Metadata { get; set; }
        /// <summary>
        /// The file extensions ROMs of this platform are known to have.
        /// </summary>
        IEnumerable<string> FileExtensions { get; }
        /// <summary>
        /// The maximum amount of inputs that are physically possible for this platform to have.
        /// </summary>
        int MaximumInputs { get; }
    }
}
