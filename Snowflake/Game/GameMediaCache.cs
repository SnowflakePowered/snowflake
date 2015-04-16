using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net;
using System.IO;

namespace Snowflake.Game
{
    public class GameMediaCache : IGameMediaCache
    {
        const string fileBoxartBack = "BoxartBack.png";
        const string fileBoxartFront = "BoxartFront.png";
        const string fileBoxartFull = "BoxartFull.png";
        const string fileGameFanart = "GameFanart.png";
        const string fileGameVideo = "GameVideo";
        const string fileGameMusic = "GameMusic";

        public string RootPath { get; private set; }
        public string CacheKey { get; private set; }
        public string GameVideoFileName { get; private set; }
        public string GameMusicFileName { get; private set; }

        string fullPath;

        public GameMediaCache(string rootPath, string cacheKey)
        {
            this.RootPath = rootPath;
            this.CacheKey = cacheKey;
            this.fullPath = Path.Combine(this.RootPath, this.CacheKey);
            if (!Directory.Exists(this.fullPath)) Directory.CreateDirectory(this.fullPath);
        }
        public GameMediaCache(string cacheKey) : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Snowflake", "gamemediacache"), cacheKey) { }

        private void SetImage(Uri imageUri, string fileName)
        {
            using (var webClient = new WebClient())
                try
                {
                    byte[] imageData = webClient.DownloadData(imageUri);
                    using (Stream imageStream = new MemoryStream(imageData))
                    using (Image image = Image.FromStream(imageStream, true, true))
                    {
                        try
                        {
                            if (File.Exists(Path.Combine(this.fullPath, fileName))) File.Delete(Path.Combine(this.fullPath, fileName));
                            image.Save(Path.Combine(this.fullPath, fileName), ImageFormat.Png);
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine(String.Format("[WARN] Swallowed ArgumentException: Attemped to add invalid image to game cache {0}"), imageUri.AbsoluteUri);
                        }
                        catch
                        {
                            Console.WriteLine(String.Format("[WARN] Swallowed UnknownException: Unable to save image {0} to game cache"), imageUri.AbsoluteUri);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine(String.Format("[WARN] Swallowed UnknownException: Unable to download {0} to game cache"), imageUri.AbsoluteUri);
                }
        }

        private Image GetImage(string fileName)
        {
            return Bitmap.FromFile(Path.Combine(this.fullPath, fileName));
        }

        public void SetBoxartFront(Uri boxartFrontUri)
        {
            this.SetImage(boxartFrontUri, fileBoxartFront);
        }

        public void SetBoxartBack(Uri boxartBackUri)
        {
            this.SetImage(boxartBackUri, fileBoxartBack);
        }

        public void SetBoxartFull(Uri boxartFullUri)
        {
            this.SetImage(boxartFullUri, fileBoxartFull);
        }

        public void SetGameFanart(Uri fanartUri)
        {
            this.SetImage(fanartUri, fileGameFanart);
        }

        public void SetGameVideo(Uri videoUri)
        {
            throw new NotImplementedException();
        }

        public void SetGameMusic(Uri musicUri)
        {
            throw new NotImplementedException();
        }

        public Image GetBoxartFrontImage()
        {
            return this.GetImage(fileBoxartFront);
        }

        public Image GetBoxartBackImage()
        {
            return this.GetImage(fileBoxartBack);
        }

        public Image GetBoxartFullImage()
        {
            return this.GetImage(fileBoxartFull);
        }

        public Image GetGameFanartImage()
        {
            return this.GetImage(fileGameFanart);
        }



    }
}
