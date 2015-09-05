using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile
{
    /// <summary>
    /// Represents all data that is able to be gleaned from the file without any other data source
    /// such as a database or online-scraper. This object is to be passed to whatever scraper plugin is used.
    /// </summary>
    interface IScrapableInfo
    {
        /// <summary>
        /// A searchable game title.
        /// This title always has beginning articles placed in from rather than at the back.
        /// <example>
        /// "The Legend of Zelda", rather than "Legend of Zelda, The"
        /// </example>
        /// </summary>
        string QueryableTitle { get; }
        /// <summary>
        /// Any applicable Game ID such as Wii Disc ID, Playsation Disc ID or 4-letter DS ROM ID
        /// </summary>
        string GameId { get; }
        /// <summary>
        /// The original filename of the rom
        /// </summary>
        string OriginalFilename { get; }
        /// <summary>
        /// The determined stone platform ID of the ROM. This is heuristically detected by looking at the ROM's file structure.
        /// Usually only the header is required, so to save on memory requirements never load more than 1024 bytes of the ROM in memory.
        /// </summary>
        string StonePlatformId { get; }
        /// <summary>
        ///The structured filename of the rom if it is readable.
        /// If not then this is null
        ///</summary>
        IStructuredFilename StructuredFilename { get; }
        /// <summary>
        /// Calculate the CRC32 of the rom. 
        /// </summary>
        /// <remarks>This is a very expensive operation taking around 10 seconds. If possible use GameId instead</remarks>
        /// <returns>The CRC32 hash of the rom</returns>
        string HashCrc32();
        /// <summary>
        /// Calculate th MD5 of the rom. 
        /// </summary>
        /// <remarks>This is a very expensive operation taking around 15 seconds. If possible use GameId instead</remarks>
        /// <returns>The MD5 hash of the rom</returns>
        string HashMd5();
    }
}
