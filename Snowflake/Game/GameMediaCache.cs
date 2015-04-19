using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        public string GameVideoFileName { 
            get 
            {
                return Directory.EnumerateFiles(this.fullPath).Where(file => Path.GetFileNameWithoutExtension(file) == fileGameVideo).FirstOrDefault();
            } 
        }
        public string GameMusicFileName { 
            get 
            {
                return Directory.EnumerateFiles(this.fullPath).Where(file => Path.GetFileNameWithoutExtension(file) == fileGameMusic).FirstOrDefault(); 
            } 
        }

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
                            //Create high quality resizes
                            if (File.Exists(Path.Combine(this.fullPath, "@75_" + fileName))) File.Delete(Path.Combine(this.fullPath, "@50_" + fileName));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.75))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, "@75_" + fileName), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, "@50_" + fileName))) File.Delete(Path.Combine(this.fullPath, "@50_" + fileName));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.5))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, "@50_" + fileName), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, "@25_" + fileName))) File.Delete(Path.Combine(this.fullPath, "@25_" + fileName));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.25))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, "@25_" + fileName), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, "@10_" + fileName))) File.Delete(Path.Combine(this.fullPath, this.fullPath, "@10_" + fileName));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.1))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, "@10_" + fileName), ImageFormat.Png);
                            }
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

        /// <summary>
        /// Resize the image to the specified width and height.
        /// http://stackoverflow.com/a/24199315/1822679
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
        /// <summary>
        /// Resize an image by a percentage factor
        /// </summary>
        /// <param name="image">The image to resize</param>
        /// <param name="factor">The factor to resize by in decimal form</param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image image, double factor)
        {
            int newHeight = Convert.ToInt32(Math.Ceiling(image.Size.Height * factor));
            int newWidth = Convert.ToInt32(Math.Ceiling(image.Size.Width * factor));
            return GameMediaCache.ResizeImage(image, newWidth, newHeight);
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
            string _fileName = fileName + extension;
            using (var webClient = new WebClient())
            {
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
                File.Delete(Path.Combine(this.fullPath, fileGameVideo + ".mp4"));
                File.Delete(Path.Combine(this.fullPath, fileGameVideo + ".webm"));
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
                File.Delete(Path.Combine(this.fullPath, fileGameMusic + ".mp3"));
                File.Delete(Path.Combine(this.fullPath, fileGameMusic + ".ogg"));
                File.Delete(Path.Combine(this.fullPath, fileGameMusic + ".wav"));

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
