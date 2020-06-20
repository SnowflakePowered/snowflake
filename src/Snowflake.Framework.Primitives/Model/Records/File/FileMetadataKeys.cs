using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Model.Records.File
{
    /// <summary>
    /// A list of standard keys for game metadata
    /// </summary>
    public static class FileMetadataKeys
    {
        /// <summary>
        /// The crc32 of the file
        /// </summary>
        public static readonly string FileHashCrc32 = "file_hash_crc32";

        /// <summary>
        /// The md5 of the file
        /// </summary>
        public static readonly string FileHashMd5 = "file_hash_md5";

        /// <summary>
        /// The sha1 of the file
        /// </summary>
        public static readonly string FileHashSha1 = "file_hash_sha1";

        /// <summary>
        /// The region of the rom.
        /// </summary>
        public static readonly string RomRegion = "rom_region";

        /// <summary>
        /// The stone platform id of the rom file
        /// </summary>
        public static readonly string RomPlatform = "rom_platform";

        /// <summary>
        /// The internal name of the ROM.
        /// </summary>
        public static readonly string RomInternalName = "rom_internal_name";

        /// <summary>
        /// The serial of the ROM
        /// </summary>
        public static readonly string RomSerial = "rom_serial";

        /// <summary>
        /// The canonical title of a ROM from the shiragame database
        /// </summary>
        public static readonly string RomCanonicalTitle = "rom_canonical_title";
        
        /// <summary>
        /// The filename of the runnable binary in a zip file.
        /// </summary>
        public static readonly string RomZipRunnableFilename = "rom_zip_runnable_filename";

        /// <summary>
        /// The friendly name of a resource pack.
        /// </summary>
        public static readonly string ResourceName = "resource_name";

        /// <summary>
        /// The description of a resource pack.
        /// </summary>
        public static readonly string ResourceDescription = "resource_description";

        /// <summary>
        /// The type of a resource pack
        /// </summary>
        public static readonly string ResourceType = "resource_type";

        /// <summary>
        /// The emulator that uses a resource pack
        /// </summary>
        public static readonly string ResourceOrchestrator = "resource_orchestrator";
    }
}
