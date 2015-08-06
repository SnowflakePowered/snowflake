using System;
using System.Drawing;

namespace Snowflake.Game
{
    /// <summary>
    /// The IGameMediaCache allows Snowflake to store metadata-media about a game
    /// Each IGameInfo object can access their IGameMediaCache by keying against the UUID. 
    /// IGameMediaCache is implemented by GameMediaCache as a folder per-game with a single file for each object
    /// Images are saved and retrieved in PNG format regardless of original format. Music and video are stored in their original format
    /// It is recommended that music and video are stored in .mp3 or vorbis .ogg format, and h.264 in a .mp4 container or VP8 .webm , respectively.
    /// </summary>
    public interface IGameMediaCache
    {
        /// <summary>
        /// The root path of the game media cache, where all game media cache folders are stored
        /// </summary>
        string RootPath { get; }
        /// <summary>
        /// The cache key of the game media cache, usually the UUID of the game this corresponds to
        /// </summary>
        string CacheKey { get; }
        /// <summary>
        /// Sets the front box art and saves it as a PNG file. 
        /// The same file data can be retrieved by GetBoxartFrontImage
        /// </summary>
        /// <param name="boxartFrontUri">The URI to download. This can be a file URI</param>
        void SetBoxartFront(Uri boxartFrontUri);
        /// <summary>
        /// Sets the back box art and saves it as a PNG file. 
        /// The same file data can be retrieved by GetBoxartBackImage
        /// </summary>
        /// <param name="boxartBackUri">The URI to download. This can be a file URI</param>
        void SetBoxartBack(Uri boxartBackUri);
        /// <summary>
        /// Sets the full box art and saves it as a PNG file. 
        /// The same file data can be retrieved by GetBoxartFullImage
        /// </summary>
        /// <param name="boxartFullUri">The URI to download. This can be a file URI</param>
        void SetBoxartFull(Uri boxartFrontUri);
        /// <summary>
        /// Sets the fanart and saves it as a PNG file. 
        /// The same file data can be retrieved by GetGameFanartImage
        /// </summary>
        /// <param name="fanartUri">The URI to download. This can be a file URI</param>
        void SetGameFanart(Uri fanartUri);
        /// <summary>
        /// Download and sets the game video. It is recommended the video format be in h.264 in a .mp4 container or VP8 in a .webm container
        /// </summary>
        /// <param name="videoUri">The URI to download</param>
        void SetGameVideo(Uri videoUri);
        /// <summary>
        /// Download and sets the game music. It is recommended the audio format be in .mp3 MPEG-layer 3 format or .ogg Vorbis format
        /// </summary>
        /// <param name="musicUri">The URI to download</param>
        void SetGameMusic(Uri musicUri);
        /// <summary>
        /// Get the front boxart image data. Returns null if not exists.
        /// </summary>
        /// <returns>The front boxart image as a System.Drawing.Image object</returns>
        Image GetBoxartFrontImage();
        /// <summary>
        /// Get the back boxart image data. 
        /// </summary>
        /// <returns>The front boxart image as a System.Drawing.Image object. Returns null if not exists.</returns>
        Image GetBoxartBackImage();
        /// <summary>
        /// Get the full boxart image data. 
        /// </summary>
        /// <returns>The full boxart image as a System.Drawing.Image object. Returns null if not exists</returns>
        Image GetBoxartFullImage();
        /// <summary>
        /// Get the fanart image data. 
        /// </summary>
        /// <returns>The fanart image as a System.Drawing.Image object. Returns null if not exists</returns>
        Image GetGameFanartImage();
        /// <summary>
        /// Gets the video file name. Empty if not exists
        /// </summary>
        string GameVideoFileName { get; }
        /// <summary>
        /// Gets the music file name. Empty if not exists
        /// </summary>
        string GameMusicFileName { get; }
    }
}
