using System;
using System.Collections.Generic;
using System.Drawing;

namespace Snowflake.Game
{
    public interface IGameScreenshotCache
    {
        /// <summary>
        /// The root path of the game screenshot cache, where all game media cache folders are stored
        /// </summary>
        string RootPath { get; }
        /// <summary>
        /// The cache key of the game screenshot cache, usually the UUID of the game this corresponds to
        /// </summary>
        string CacheKey { get; }
        /// <summary>
        /// Adds a screenshot to the game given a Uri
        /// </summary>
        /// <param name="screenshotUri">The Uri to download</param>
        void AddScreenshot(Uri screenshotUri);
        /// <summary>
        /// Adds a screenshot to the game given an Image object
        /// </summary>
        /// <param name="screenshotData">The image object</param>
        void AddScreenshot(Image screenshotData);
        /// <summary>
        /// Removes a screenshot from the cache
        /// </summary>
        /// <param name="screenshotIndex">The index of the screenshot to remove</param>
        void RemoveScreenshot(int screenshotIndex);
        /// <summary>
        /// The backing screenshot collection
        /// </summary>
        IReadOnlyList<string> ScreenshotCollection { get; }


    }
}
