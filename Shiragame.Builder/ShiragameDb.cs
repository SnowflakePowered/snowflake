using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    internal class ShiragameDb : SqliteMemoryDatabase
    {
        internal ShiragameDb()
        {
            this.CreateDatabase();
        }
        private void CreateDatabase()
        {
            this.CreateTable("dat",
                "platformId TEXT",
                "crc32 TEXT PRIMARY KEY",
                "md5 TEXT",
                "sha1 TEXT",
                "mimetype TEXT",
                "filename TEXT",
                "region TEXT");
            this.CreateTable("serial",
                "platformId TEXT",
                "serial TEXT",
                "title TEXT",
                "region TEXT");
            this.CreateTable("shiragame",
                "shiragame TEXT PRIMARY KEY",
                "stoneversion TEXT",
                "generated TEXT",
                "uuid TEXT");
            this.Execute(@"INSERT OR REPLACE INTO shiragame(shiragame, stoneversion, generated, uuid)" +
                         "VALUES (@shiragame, @stoneversion, @generated, @uuid)", new
                         {
                             shiragame = "SHIRAGAME",
                             stoneversion = new StoneProvider().StoneVersion.ToString(),
                             generated = ShiragameDb.UnixTimeNow().ToString(),
                             uuid = Guid.NewGuid().ToString()
                         });
        }
        internal static long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime());
            return (long)timeSpan.TotalSeconds;
        }
        internal void Commit(IEnumerable<DatInfo> datInfos)
        {
            this.Execute(@"INSERT OR IGNORE INTO dat(platformId, crc32, md5, sha1, mimetype, filename, region)
                          VALUES (@PlatformId, @CRC32, @MD5, @SHA1, @MimeType, @FileName, @Region)", datInfos);
        }

        internal void Commit(IEnumerable<SerialInfo> serialInfos)
        {

            this.Execute(@"INSERT OR IGNORE INTO serial(platformId, serial, title, region)
                          VALUES (@PlatformId, @Serial, @Title, @Region)", serialInfos);
        }
    }
}
