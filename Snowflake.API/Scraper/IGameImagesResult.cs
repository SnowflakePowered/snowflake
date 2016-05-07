using System;
using System.Collections.Generic;

namespace Snowflake.Scraper
{
    /// <summary>
    /// Represents a set of images that relate to a game
    /// </summary>
    public interface IGameImagesResult
    {
        void AddFromUrl(GameImageType imageType, Uri imageUrl);
        IDictionary<string, string> Boxarts { get; set; }
        IList<string> Fanarts { get; set; }
        string ImagesID { get; }
        IList<string> Screenshots { get; set; }
    }
}
