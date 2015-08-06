using System;

namespace Snowflake.Information.MediaStore
{
    /// <summary>
    /// Represents a mediastore that is able to store pieces of media for an IInfo
    /// </summary>
    [Obsolete("MediaStore has been superseded by GameMediaCache")]
    public interface IMediaStore
    {
        /// <summary>
        /// The section to store images.
        /// </summary>
        IMediaStoreSection Images { get; }
        /// <summary>
        /// The section to store audio.
        /// </summary>
        IMediaStoreSection Audio { get; }
        /// <summary>
        /// The section to store video.
        /// </summary>
        IMediaStoreSection Video { get; }
        /// <summary>
        /// The section to store miscellenous resources.
        /// </summary>
        IMediaStoreSection Resources { get; }
        /// <summary>
        /// The key to access the mediastore
        /// </summary>
        string MediaStoreKey { get; }

    }
}
