using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using System.Drawing;

namespace Snowflake.Caching
{
    /// <summary>
    /// Represents a set-and-forget keyed image cache on a record guid.
    /// </summary>
    public interface IKeyedImageCache
    {
        /// <summary>
        /// Adds an image to the image cache, generating resized image/png versions of the files, and
        /// returning a list of file records.
        /// </summary>
        /// <param name="image">The image to add</param>
        /// <param name="recordGuid">A record to link to</param>
        /// <param name="imageType">The type of image See <see cref="ImageTypes"/> for recognized image types</param>
        /// <returns>The generated file records with appropriate metadata linking it to the cache folder</returns>
        IList<IFileRecord> Add(Image image, Guid recordGuid, string imageType);

        /// <summary>
        /// Adds an image to the image cache, generating resized image/png versions of the files, and
        /// returning a list of file records.
        /// </summary>
        /// <param name="image">The image to add</param>
        /// <param name="recordGuid">A record to link to</param>
        /// <param name="imageType">The type of image See <see cref="ImageTypes"/> for recognized image types</param>
        /// <param name="dateTime">A date time to link it to.</param>
        /// <returns>The generated file records with appropriate metadata linking it to the cache folder</returns>
        IList<IFileRecord> Add(Image image, Guid recordGuid, string imageType, DateTime dateTime);
    }
}
