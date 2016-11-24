using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Caching
{
    /// <summary>
    /// The image metadata key
    /// </summary>
    public static class ImageMetadataKeys
    {
        /// <summary>
        /// The image date
        /// </summary>
        public const string Date = "image_date";
        /// <summary>
        /// The image type
        /// </summary>
        public const string Type = "image_type";

        /// <summary>
        /// The scale of the image
        /// </summary>
        public const string Scale = "image_scale";

        /// <summary>
        /// The image cache id if available.
        /// </summary>
        public const string CacheId = "image_cache_id";
    }
}
