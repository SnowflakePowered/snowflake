using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;

namespace Snowflake.Scraper.Shiragame
{
    public class ShiragameProvider : IShiragameProvider
    {
        private readonly SqliteDatabase backingDatabase;
        public ShiragameProvider(string shiragamePath)
        {
            this.backingDatabase = new SqliteDatabase(shiragamePath);
            string shiragameVer = this.backingDatabase.Query<string>("SELECT version FROM shiragame").First();
            string stoneVer = this.backingDatabase.Query<string>("SELECT version FROM shiragame").First();
            this.StoneVersion = Version.Parse(stoneVer);
            this.DatabaseVersion = Version.Parse(shiragameVer);
        }

        public Version StoneVersion { get; }
        public Version DatabaseVersion { get; }

        public IRomInfo GetFromCrc32(string crc32)
        {
            return this.GetRomInfo("crc32", crc32);
        }

        public IRomInfo GetFromMd5(string md5)
        {
            return this.GetRomInfo("md5", md5);
        }

        public IRomInfo GetFromSha1(string sha1)
        {
            return this.GetRomInfo("sha1", sha1);
        }

        public ISerialInfo GetFromSerial(string platformId, string serial)
        {
            const string sql = @"SELECT * FROM serial WHERE serial LIKE @serial AND platformId = @platformId";
            return this.backingDatabase.QueryFirstOrDefault<SerialInfo>(sql, new { serial = $"%{serial}%", platformId});
        }

        public bool IsMameRom(string filename)
        {
            const string sql = @"SELECT filename FROM mame WHERE filename = @filename";
            return this.backingDatabase.Query<string>(sql, new {filename}).Any();
        }

        private IRomInfo GetRomInfo(string column, string value)
        {
            string sql = $@"SELECT * FROM rom WHERE {column} = @value";
            return this.backingDatabase.QueryFirstOrDefault<RomInfo>(sql, new {value});
        }

      
    }
}
