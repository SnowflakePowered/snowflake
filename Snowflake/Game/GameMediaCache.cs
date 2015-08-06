using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

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

        public string RootPath { get; }
        public string CacheKey { get; }
        public string GameVideoFileName { 
            get 
            {
                return Directory.EnumerateFiles(this.fullPath).Where(file => Path.GetFileNameWithoutExtension(file) == GameMediaCache.fileGameVideo).FirstOrDefault();
            } 
        }
        public string GameMusicFileName { 
            get 
            {
                return Directory.EnumerateFiles(this.fullPath).Where(file => Path.GetFileNameWithoutExtension(file) == GameMediaCache.fileGameMusic).FirstOrDefault(); 
            } 
        }

        readonly string fullPath;

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
                            if (File.Exists(Path.Combine(this.fullPath, $"@75_{fileName}"))) File.Delete(Path.Combine(this.fullPath, $"@75_{fileName}"));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.75))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, $"@75_{fileName}"), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, $"@50_{fileName}"))) File.Delete(Path.Combine(this.fullPath, $"@50_{fileName}"));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.5))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, $"@50_{fileName}"), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, $"@25_{fileName}"))) File.Delete(Path.Combine(this.fullPath, $"@25_{fileName}"));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.25))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, $"@25_{fileName}"), ImageFormat.Png);
                            }
                            if (File.Exists(Path.Combine(this.fullPath, $"@10_{fileName}"))) File.Delete(Path.Combine(this.fullPath, this.fullPath, $"@10_{fileName}"));
                            using (Image resizedImage = GameMediaCache.ResizeImage(image, 0.1))
                            {
                                resizedImage.Save(Path.Combine(this.fullPath, $"@10_{fileName}"), ImageFormat.Png);
                            }
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine($"[WARN] Swallowed ArgumentException: Attemped to add invalid image to game cache {imageUri.AbsoluteUri}");
                        }
                        catch
                        {
                            Console.WriteLine($"[WARN] Swallowed UnknownException: Unable to save image {imageUri.AbsoluteUri} to game cache");
                        }
                    }
                }
                catch
                {
                    Console.WriteLine($"[WARN] Swallowed UnknownException: Unable to download {imageUri.AbsoluteUri} to game cache");
                }
            }
        }

        private Image GetImage(string fileName)
        {
            return Image.FromFile(Path.Combine(this.fullPath, fileName));
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
            this.SetImage(boxartFrontUri, GameMediaCache.fileBoxartFront);
        }

        public void SetBoxartBack(Uri boxartBackUri)
        {
            this.SetImage(boxartBackUri, GameMediaCache.fileBoxartBack);
        }

        public void SetBoxartFull(Uri boxartFullUri)
        {
            this.SetImage(boxartFullUri, GameMediaCache.fileBoxartFull);
        }

        public void SetGameFanart(Uri fanartUri)
        {
            this.SetImage(fanartUri, GameMediaCache.fileGameFanart);
        }

        /// <summary>
        /// Downloads a file and returns the full filename of the downloaded file
        /// </summary>
        /// <param name="fileUri">The Uri to download</param>
        /// <param name="fileName">Filename to save as without extension</param>
        /// <returns></returns>
        private string DownloadFile(Uri fileUri, string fileName)
        {
            string filePath = $"{fileUri.Scheme}{Uri.SchemeDelimiter}{fileUri.AbsoluteUri}{fileUri.AbsolutePath}";
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
            string filePath = $"{videoUri.Scheme}{Uri.SchemeDelimiter}{videoUri.Authority}{videoUri.AbsolutePath}";
            string extension = Path.GetExtension(filePath);
            if (extension.Contains("mp4") || extension.Contains("webm"))
            {
                File.Delete(Path.Combine(this.fullPath, $"{GameMediaCache.fileGameVideo}.mp4"));
                File.Delete(Path.Combine(this.fullPath, $"{GameMediaCache.fileGameVideo}.webm"));
                this.DownloadFile(videoUri, GameMediaCache.fileGameVideo);
            }
            else
            {
                Console.WriteLine("[WARN] Attempted to download unknown video format");
            }
        }

        public void SetGameMusic(Uri musicUri)
        {
            string filePath = $"{musicUri.Scheme}{Uri.SchemeDelimiter}{musicUri.Authority}{musicUri.AbsolutePath}";
            string extension = Path.GetExtension(filePath);
            if (extension.Contains("mp3") || extension.Contains("ogg") || extension.Contains("wav"))
            {
                File.Delete(Path.Combine(this.fullPath, $"{GameMediaCache.fileGameMusic}.mp3"));
                File.Delete(Path.Combine(this.fullPath, $"{GameMediaCache.fileGameMusic}.ogg"));
                File.Delete(Path.Combine(this.fullPath, $"{GameMediaCache.fileGameMusic}.wav"));

                this.DownloadFile(musicUri, GameMediaCache.fileGameMusic);
            }
            else
            {
                Console.WriteLine("[WARN] Attempted to download unknown music format");
            }
        }

        public Image GetBoxartFrontImage()
        {
            return this.GetImage(GameMediaCache.fileBoxartFront);
        }

        public Image GetBoxartBackImage()
        {
            return this.GetImage(GameMediaCache.fileBoxartBack);
        }

        public Image GetBoxartFullImage()
        {
            return this.GetImage(GameMediaCache.fileBoxartFull);
        }

        public Image GetGameFanartImage()
        {
            return this.GetImage(GameMediaCache.fileGameFanart);
        }



    }
}
