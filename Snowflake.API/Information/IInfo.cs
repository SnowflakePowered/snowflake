using System.Collections.Generic;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information
{
    /// <summary>
    /// Represents an object that can have platform-relational information attached to it.
    /// <see cref="Snowflake.Game.IGameInfo"/>
    /// <seealso cref="Snowflake.Platform.IPlatformInfo"/>
    /// </summary>
    public interface IInfo
    {
        /// <summary>
        /// The id of the platform that this object is related to
        /// </summary>
        string PlatformId { get; }
        /// <summary>
        /// The name of this object
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Any metadata that is attached to this object, such as descriptions
        /// </summary>
        IDictionary<string, string> Metadata { get; set; }
        /// <summary>
        /// The mediastore that is attached to this object.
        /// </summary>
        IMediaStore MediaStore { get;  }

    }
}
