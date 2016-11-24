using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Scraper;

namespace Snowflake.Records.File
{
    /// <summary>
    /// A list of standard keys for game metadata
    /// </summary>
    public static class FileMetadataKeys
    {
        /// <summary>
        /// The crc32 of the file
        /// </summary>
        public const string FileHashCrc32 = "file_hash_crc32";
        /// <summary>
        /// The md5 of the file
        /// </summary>
        public const string FileHashMd5 = "file_hash_md5";
        /// <summary>
        /// The sha1 of the file
        /// </summary>
        public const string FileHashSha1 = "file_hash_sha1";
        /// <summary>
        /// The region of the rom.
        /// </summary>
        public const string RomRegion = "rom_region";
        /// <summary>
        /// The stone platform id of the rom file
        /// </summary>
        public const string RomPlatform = "rom_platform";
        /// <summary>
        /// The internal name of the ROM.
        /// </summary>
        public const string RomInternalName = "rom_internal_name";
        /// <summary>
        /// The serial of the ROM
        /// </summary>
        public const string RomSerial = "rom_serial";
        /// <summary>
        /// The canonical title of a ROM from the shiragame database
        /// </summary>
        public const string RomCanonicalTitle = "rom_canonical_title";
        /// <summary>
        /// The ROM was not determined for sure by a <see cref="IScrapeEngine"/>
        /// </summary>
        public const string RomIsAmbiguousIdentification = "rom_is_ambiguous_identification";
        /// <summary>
        /// The filename of the runnable binary in a zip file.
        /// </summary>
        public const string RomZipRunnableFilename = "rom_zip_runnable_filename";
    }
}
