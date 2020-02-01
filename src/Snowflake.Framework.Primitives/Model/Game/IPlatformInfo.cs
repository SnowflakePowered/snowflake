using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Model.Game
{
    /// <summary>
    /// Represents an emulated console or a platform in Snowflake.
    /// </summary>
    public interface IPlatformInfo
    {
        /// <summary>
        /// Gets the ID of the platform that this object is related to
        /// </summary>
        PlatformId PlatformID { get; }

        /// <summary>
        /// Gets the friendly name of this platform
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Gets or sets any metadata that is attached to this object, such as descriptions
        /// </summary>
        IDictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// Gets the file types ROMs of this platform are known to have.
        /// Included as a mapping of file extension to mime type application/x-romfile-*
        /// </summary>
        IDictionary<string, string> FileTypes { get; }

        /// <summary>
        /// Gets the list of bios files for this platform
        /// </summary>
        IEnumerable<ISystemFile> BiosFiles { get; }

        /// <summary>
        /// Gets the maximum amount of inputs that are physically possible for this platform to have.
        /// </summary>
        int MaximumInputs { get; }
    }
}
