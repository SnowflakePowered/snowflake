using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Model.Records.File
{
    /// <summary>
    /// The image metadata key
    /// </summary>
    public static class ImageMetadataKeys
    {
        /// <summary>
        /// The image date
        /// </summary>
        public static readonly string Date = "image_date";

        /// <summary>
        /// The image type
        /// </summary>
        public static readonly string Type = "image_type";

        /// <summary>
        /// The scale of the image
        /// </summary>
        public static readonly string Scale = "image_scale";

        /// <summary>
        /// The image cache id if available.
        /// </summary>
        public static readonly string CacheId = "image_cache_id";
    }
}
