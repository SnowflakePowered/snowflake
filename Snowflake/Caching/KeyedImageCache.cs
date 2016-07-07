using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Caching
{
    public class KeyedImageCache : IKeyedImageCache
    {
        private readonly string rootPath;

        public KeyedImageCache(string appDataPath)
        {
            if (!Directory.Exists(Path.Combine(appDataPath, ".imgcache")))
                Directory.CreateDirectory(Path.Combine(appDataPath, ".imgcache"));
            this.rootPath = Path.Combine(appDataPath, ".imgcache");
        }
        public IList<IFileRecord> Add(Image image, Guid recordGuid, string imageType)
        {
            Guid cacheId = Guid.NewGuid();
            string cachePath = Path.Combine(this.rootPath, cacheId.ToString());
            Directory.CreateDirectory(cachePath);
            return new List<IFileRecord>
            {
                KeyedImageCache.SaveImage(image, 100, cacheId, cachePath, imageType, recordGuid),
                KeyedImageCache.SaveImage(image, 50, cacheId, cachePath, imageType, recordGuid),
                KeyedImageCache.SaveImage(image, 25, cacheId, cachePath, imageType, recordGuid),
                KeyedImageCache.SaveImage(image, 10, cacheId, cachePath, imageType, recordGuid)

            };
        }

        public IList<IFileRecord> Add(Image image, Guid recordGuid, string imageType, DateTime dateTime)
        {
            Guid cacheId = Guid.NewGuid();
            string cachePath = Path.Combine(this.rootPath, cacheId.ToString());
            Directory.CreateDirectory(cachePath);
            return new List<IFileRecord>
            {
                KeyedImageCache.SaveImage(image, 100, cacheId, cachePath, imageType, recordGuid, dateTime),
                KeyedImageCache.SaveImage(image, 50, cacheId, cachePath, imageType, recordGuid, dateTime),
                KeyedImageCache.SaveImage(image, 25, cacheId, cachePath, imageType, recordGuid, dateTime),
                KeyedImageCache.SaveImage(image, 10, cacheId, cachePath, imageType, recordGuid, dateTime)

            };
        }

        private static IFileRecord SaveImage(Image image, int percent, Guid cacheId, string cachePath, string imageType, Guid recordGuid)
        {
            string path = Path.Combine(cachePath, $"@{percent}_{cacheId}.jpg");
            if (percent == 100)
            {
                image.Save(path, ImageFormat.Jpeg);
                
            }
            else
            {
                using (Image resizedImage = KeyedImageCache.ResizeImage(image, percent * 0.01))
                {
                    resizedImage.Save(path, ImageFormat.Jpeg);
                }
            }
            var record = new FileRecord(path, "image/jpeg", recordGuid);
            var data = new List<IRecordMetadata>()
            {
                {new RecordMetadata(ImageMetadataKeys.CacheId, cacheId.ToString(), record.Guid)},
                {new RecordMetadata(ImageMetadataKeys.Scale, percent.ToString(), record.Guid)},
                {new RecordMetadata(ImageMetadataKeys.Type, imageType, record.Guid)}
            };

            foreach (var metadata in data)
            {
                record.Metadata.Add(metadata.Key, metadata);
            }

            return record;
        }
        private static IFileRecord SaveImage(Image image, int percent, Guid cacheId, string cachePath, string imageType, Guid recordGuid, DateTime dateTime)
        {
            var record = KeyedImageCache.SaveImage(image, percent, cacheId, cachePath, imageType, recordGuid);
            record.Metadata.Add(ImageMetadataKeys.Date,
                new RecordMetadata(ImageMetadataKeys.Date, dateTime.ToString("yyyy-MM-dd-HH-mm"), record.Guid));
            return record;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// http://stackoverflow.com/a/24199315/1822679
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height)
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
        /// <returns>The resized image</returns>
        private static Bitmap ResizeImage(Image image, double factor)
        {
            int newHeight = Convert.ToInt32(Math.Ceiling(image.Size.Height * factor));
            int newWidth = Convert.ToInt32(Math.Ceiling(image.Size.Width * factor));
            return KeyedImageCache.ResizeImage(image, newWidth, newHeight);
        }
    }
}
