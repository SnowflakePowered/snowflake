using System;
using System.Collections.Generic;
using Snowflake.Information.MediaStore;
namespace Snowflake.Scraper
{
    public interface IGameImagesResult
    {
        void AddFromUrl(GameImageType imageType, Uri imageUrl);
        IDictionary<string, string> Boxarts { get; set; }
        IList<string> Fanarts { get; set; }
        string ImagesID { get; }
        IList<string> Screenshots { get; set; }
        IMediaStore ToMediaStore(string mediaStoreKey);
    }
}
