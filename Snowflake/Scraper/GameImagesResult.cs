using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Snowflake.Constants;
using Snowflake.Information.MediaStore;

namespace Snowflake.Scraper
{
    public class GameImagesResult : IGameImagesResult
    {
        public IList<string> Fanarts { get; set; }
        public IList<string> Screenshots { get; set; }
        public IDictionary<string, string> Boxarts { get; set; }
        public string ImagesID { get; private set; }
        public GameImagesResult()
        {
            this.ImagesID = ShortGuid.NewShortGuid();
            this.Fanarts = new List<string>();
            this.Screenshots = new List<string>();
            this.Boxarts = new Dictionary<string, string>();
        }
        
        public void AddFromUrl(GameImageType imageType, Uri imageUrl)
        {
            switch (imageType)
            {
               
                case GameImageType.Fanart:
                    this.Fanarts.Add(imageUrl.AbsoluteUri);
                    break;
                case GameImageType.Screenshot:
                    this.Screenshots.Add(imageUrl.AbsoluteUri);
                    break;
                case GameImageType.Boxart_back:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_back, imageUrl.AbsoluteUri);
                    break;
                case GameImageType.Boxart_front:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_front, imageUrl.AbsoluteUri);
                    break;
                case GameImageType.Boxart_full:
                    this.Boxarts.Add(ImagesInfoFields.img_boxart_full, imageUrl.AbsoluteUri);
                    break;
            }
        }

        public IMediaStore ToMediaStore(string mediaStoreKey)
        {
            var mediaStore = new FileMediaStore(mediaStoreKey);
            using (var webClient = new WebClient())
            {
                for (int i = 0; i < this.Fanarts.Count; i++)
                {
                    var fanart = this.Fanarts[i];
                    string filename = Path.GetFileName(new Uri(fanart).AbsolutePath);
                    try
                    {
                        string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "fanart_" + filename);
                        webClient.DownloadFile(fanart, downloadPath);
                        mediaStore.Images.Add("fanart_" + i, downloadPath);
                    }
                    catch
                    {
                        continue;
                    }
                }
                for (int i = 0; i < this.Screenshots.Count; i++)
                {
                    var screenshot = this.Screenshots[i];
                    string filename = Path.GetFileName(new Uri(screenshot).AbsolutePath);
                    try
                    {
                        string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "screenshot_" + filename);
                        webClient.DownloadFile(screenshot, downloadPath);
                        mediaStore.Images.Add("screenshot_" + i, downloadPath);
                    }
                    catch
                    {
                        continue;
                    }
                }
                foreach (KeyValuePair<string, string> boxart in this.Boxarts)
                {
                    
                    string filename = Path.GetFileName(new Uri(boxart.Value).AbsolutePath);
                    try
                    {
                        string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), "boxart_" + filename);
                        webClient.DownloadFile(boxart.Value, downloadPath);
                        mediaStore.Images.Add(boxart.Key.Substring(4), downloadPath);//remove 'img_' prefix
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            return mediaStore;
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
