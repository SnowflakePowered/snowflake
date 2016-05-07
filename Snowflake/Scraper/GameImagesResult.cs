using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Snowflake.Constants;

namespace Snowflake.Scraper
{
    public class GameImagesResult : IGameImagesResult
    {
        public IList<string> Fanarts { get; set; }
        public IList<string> Screenshots { get; set; }
        public IDictionary<string, string> Boxarts { get; set; }
        public string ImagesID { get; }
        public GameImagesResult()
        {
            this.ImagesID = Guid.NewGuid().ToString();
            this.Fanarts = new List<string>();
            this.Screenshots = new List<string>();
            this.Boxarts = new Dictionary<string, string>();
        }
        
        public void AddFromUrl(GameImageType imageType, Uri imageUrl)
        {
            switch (imageType)
            {
               
                case GameImageType.IMAGE_FANART:
                    this.Fanarts.Add(imageUrl.AbsoluteUri);
                    break;
                case GameImageType.IMAGE_SCREENSHOT:
                    this.Screenshots.Add(imageUrl.AbsoluteUri);
                    break;
                case GameImageType.IMAGE_BOXART_BACK:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_back, imageUrl.AbsoluteUri);
                    break;
                case GameImageType.IMAGE_BOXART_FRONT:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_front, imageUrl.AbsoluteUri);
                    break;
                case GameImageType.IMAGE_BOXART_FULL:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_full, imageUrl.AbsoluteUri);
                    break;
            }
        }
     
        public static string GetFullImagePath(string imageFileName, string imagesId)
        {
            return GameImagesResult.GetFullImagePath(imageFileName, imageFileName, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "data", "imagescache"));
        }
        public static string GetFullImagePath(string imageFileName, string imagesId, string cachePath)
        {
            return Path.Combine(cachePath, imagesId, imageFileName);
        }
        
    }

}
