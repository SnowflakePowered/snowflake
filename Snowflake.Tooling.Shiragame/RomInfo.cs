using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiragame
{
    /// <summary>
    /// Represents a file datum in a ClrMamePro or Logiqix XML Dat File
    /// </summary>
    public class RomInfo
    {
        /// <summary>
        /// The Stone platform ID
        /// </summary>
        public string PlatformId { get; }
        /// <summary>
        /// The canonical CRC32 from the dat file
        /// </summary>
        public string CRC32 { get; }
        /// <summary>
        /// The canonical MD5 from the dat file
        /// </summary>
        public string MD5 { get; }
        /// <summary>
        /// The canonical SHA1 from the dat file
        /// </summary>
        public string SHA1 { get; }
        /// <summary>
        /// The mimetype of the file
        /// </summary>
        public string MimeType { get; }
        /// <summary>
        /// The canonical filename from the dat file
        /// </summary>
        public string FileName { get; }
        /// <summary>
        /// The ISO 3166-1 alpha-2 region code for this rom
        /// </summary>
        public string Region { get; }

        internal RomInfo(string platformId, string crc32, string md5, string sha1, string region, string mimetype, string fileName)
        {
            this.PlatformId = platformId;
            this.CRC32 = crc32;
            this.MD5 = md5;
            this.SHA1 = sha1;
            this.MimeType = mimetype;
            this.FileName = fileName;
            this.Region = region;
        }
    }
}
