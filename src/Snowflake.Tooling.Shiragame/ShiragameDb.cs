using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Persistence;
using Snowflake.Services;

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
            this.CreateTable("rom",
                "platformId TEXT",
                "crc32 TEXT",
                "md5 TEXT",
                "sha1 TEXT PRIMARY KEY",
                "mimetype TEXT",
                "filename TEXT",
                "region TEXT");
            this.CreateTable("serial",
                "platformId TEXT",
                "serial TEXT",
                "title TEXT",
                "region TEXT");
            this.CreateTable("mame",
                "filename TEXT PRIMARY KEY");
            this.CreateTable("shiragame",
                "shiragame TEXT PRIMARY KEY",
                "stoneversion TEXT",
                "generated TEXT",
                "version TEXT",
                "uuid TEXT");
            this.Execute(@"INSERT OR REPLACE INTO shiragame(shiragame, stoneversion, generated, version, uuid)" +
                         "VALUES (@shiragame, @stoneversion, @generated, @version, @uuid)", new
                         {
                             shiragame = "SHIRAGAME",
                             stoneversion = new StoneProvider().StoneVersion.ToString(),
                             generated = ShiragameDb.UnixTimeNow().ToString(),
                             version = typeof(ShiragameDb).GetTypeInfo().Assembly.GetName().Version.ToString(),
                             uuid = Guid.NewGuid().ToString(),
                         });
        }

        internal static long UnixTimeNow()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0).ToUniversalTime();
            return (long)timeSpan.TotalSeconds;
        }

        internal void Commit(IEnumerable<RomInfo> datInfos)
        {
            this.Execute(@"INSERT OR IGNORE INTO rom(platformId, crc32, md5, sha1, mimetype, filename, region)
                          VALUES (@PlatformId, @CRC32, @MD5, @SHA1, @MimeType, @FileName, @Region)", datInfos);
        }

        internal void Commit(IEnumerable<SerialInfo> serialInfos)
        {
            this.Execute(
                @"INSERT OR IGNORE INTO serial(platformId, serial, title, region)
                          VALUES (@PlatformId, @Serial, @Title, @Region)", serialInfos);
        }

        internal void Commit(IEnumerable<string> mameFilenames)
        {
            this.Execute(
                @"INSERT OR IGNORE INTO mame(filename) VALUES (@filename)",
                mameFilenames.Select(filename => new { filename })); // not sure if there's a better way.
        }
    }
}
