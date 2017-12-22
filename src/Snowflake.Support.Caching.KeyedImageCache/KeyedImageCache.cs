using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Snowflake.Caching;
using Snowflake.Records.File;
using Snowflake.Records.Metadata;

namespace Snowflake.Support.Caching.KeyedImageCache
{
    public class KeyedImageCache : IKeyedImageCache
    {
        private readonly DirectoryInfo rootPath;

        public KeyedImageCache(DirectoryInfo appDataPath)
        {
            this.rootPath = appDataPath.CreateSubdirectory(".imgcache");
        }

        /// <inheritdoc/>
        public IList<IFileRecord> Add(Stream imageStream, Guid recordGuid, string imageType)
        {
            Guid cacheId = Guid.NewGuid();
            DirectoryInfo cachePath = this.rootPath.CreateSubdirectory(cacheId.ToString());
            using (var image = Image.Load(imageStream, out IImageFormat imageFormat))
            {
                return new List<IFileRecord>
                {
                    KeyedImageCache.SaveImage(image, 100, cacheId, cachePath, imageType, recordGuid),
                    KeyedImageCache.SaveImage(image, 50, cacheId, cachePath, imageType, recordGuid),
                    KeyedImageCache.SaveImage(image, 25, cacheId, cachePath, imageType, recordGuid),
                    KeyedImageCache.SaveImage(image, 10, cacheId, cachePath, imageType, recordGuid),
                };
            }
        }

        /// <inheritdoc/>
        public IList<IFileRecord> Add(Stream imageStream, Guid recordGuid, string imageType, DateTime dateTime)
        {
            Guid cacheId = Guid.NewGuid();
            DirectoryInfo cachePath = this.rootPath.CreateSubdirectory(cacheId.ToString());
            using (var image = Image.Load(imageStream, out IImageFormat imageFormat))
            {
                return new List<IFileRecord>
                {
                    KeyedImageCache.SaveImage(image, 100, cacheId, cachePath, imageType, recordGuid, dateTime),
                    KeyedImageCache.SaveImage(image, 50, cacheId, cachePath, imageType, recordGuid, dateTime),
                    KeyedImageCache.SaveImage(image, 25, cacheId, cachePath, imageType, recordGuid, dateTime),
                    KeyedImageCache.SaveImage(image, 10, cacheId, cachePath, imageType, recordGuid, dateTime),
                };
            }
        }

        private static IFileRecord SaveImage(Image<Rgba32> image, int percent, Guid cacheId, DirectoryInfo cachePath, string imageType, Guid recordGuid)
        {
            string path = Path.Combine(cachePath.FullName, $"@{percent}_{cacheId}.jpg");
            using (FileStream stream = File.Create(path))
            {
                if (percent == 100)
                {
                    image.SaveAsJpeg(stream);
                    stream.Close();
                }
                else
                {
                    int newHeight = Convert.ToInt32(Math.Ceiling(image.Height * percent * 0.01));
                    int newWidth = Convert.ToInt32(Math.Ceiling(image.Width * percent * 0.01));

                    image.Mutate(a => a.Resize(newWidth, newHeight));
                    image.SaveAsJpeg(stream);
                    stream.Close();
                }
            }

            var record = new FileRecord(path, "image/jpeg");
            var data = new List<IRecordMetadata>()
            {
                { new RecordMetadata(ImageMetadataKeys.CacheId, cacheId.ToString(), record.Guid) },
                { new RecordMetadata(ImageMetadataKeys.Scale, percent.ToString(), record.Guid) },
                { new RecordMetadata(ImageMetadataKeys.Type, imageType, record.Guid) },
            };

            foreach (var metadata in data)
            {
                record.Metadata.Add(metadata.Key, metadata);
            }

            return record;
        }

        private static IFileRecord SaveImage(Image<Rgba32> image, int percent, Guid cacheId, DirectoryInfo cachePath, string imageType, Guid recordGuid, DateTime dateTime)
        {
            var record = KeyedImageCache.SaveImage(image, percent, cacheId, cachePath, imageType, recordGuid);
            record.Metadata.Add(ImageMetadataKeys.Date,
                new RecordMetadata(ImageMetadataKeys.Date, dateTime.ToString("yyyy-MM-dd-HH-mm"), record.Guid));
            return record;
        }
    }
}
