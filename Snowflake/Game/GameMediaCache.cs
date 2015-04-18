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
        public string GameVideoFileName { get { return Directory.EnumerateFiles(this.fullPath).Where(file => file.StartsWith(fileGameVideo)).FirstOrDefault(); } }
        public string GameMusicFileName { get { return Directory.EnumerateFiles(this.fullPath).Where(file => file.StartsWith(fileGameMusic)).FirstOrDefault(); } }

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
            {
                try
                {
                    byte[] imageData;
                    if (imageUri.Scheme == "file")
                    {
                        imageData = File.ReadAllBytes(imageUri.LocalPath);
                    }
                    else
                    {
                        imageData = webClient.DownloadData(imageUri);
                    }
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

        /// <summary>
        /// Downloads a file and returns the full filename of the downloaded file
        /// </summary>
        /// <param name="fileUri">The Uri to download</param>
        /// <param name="fileName">Filename to save as without extension</param>
        /// <returns></returns>
        private string DownloadFile(Uri fileUri, string fileName)
        {
            string filePath = String.Format("{0}{1}{2}{3}", fileUri.Scheme, Uri.SchemeDelimiter, fileUri.Authority, fileUri.AbsolutePath);
            string extension = Path.GetExtension(filePath);
            string _fileName = fileName + "." + extension;
            using (var webClient = new WebClient())
            {
                foreach (string existingFileName in Directory.EnumerateFiles(this.fullPath))
                {
                    if (existingFileName.StartsWith(fileName))
                    {
                        File.Delete(Path.Combine(this.fullPath, existingFileName));
                    }
                } 
                if (fileUri.Scheme == "file")
                {
                    File.Copy(fileUri.LocalPath, Path.Combine(this.fullPath, _fileName));
                }
                else
                {
                    webClient.DownloadFile(fileUri, Path.Combine(this.fullPath, _fileName));
                }
                return _fileName;
            }
        }

        public void SetGameVideo(Uri videoUri)
        {
            string filePath = String.Format("{0}{1}{2}{3}", videoUri.Scheme, Uri.SchemeDelimiter, videoUri.Authority, videoUri.AbsolutePath);
            string extension = Path.GetExtension(filePath);
            if (extension.Contains("mp4") || extension.Contains("webm"))
            {
                this.DownloadFile(videoUri, fileGameVideo);
            }
            else
            {
                Console.WriteLine("[WARN] Attempted to download unknown video format");
            }
        }

        public void SetGameMusic(Uri musicUri)
        {
            string filePath = String.Format("{0}{1}{2}{3}", musicUri.Scheme, Uri.SchemeDelimiter, musicUri.Authority, musicUri.AbsolutePath);
            string extension = Path.GetExtension(filePath);
            if (extension.Contains("mp3") || extension.Contains("ogg") || extension.Contains("wav"))
            {
                this.DownloadFile(musicUri, fileGameMusic);
            }
            else
            {
                Console.WriteLine("[WARN] Attempted to download unknown music format");
            }
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
