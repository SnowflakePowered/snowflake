using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    public class OpenVgdb : SqliteDatabase
    {

        private IStoneProvider stone = new StoneProvider();
        public OpenVgdb(string fileName) : base(fileName)
        {
        }

        public IEnumerable<DatInfo> GetForPlatform(string stonePlatform)
        {
            const string sql =
                "select regionID, romHashCRC, romHashMD5, romHashSHA1, romFileName from ROMs where systemID = @platformId";
            int platformId = this.PlatformMap.First(p => p.Value == stonePlatform).Key;
            return this.Query<dynamic>(sql, new {platformId})
                .Select(o => new DatInfo(stonePlatform,
                    o.romHashCRC,
                    o.romHashMD5,
                    o.romHashSHA1,
                    this.RegionMap[(int)o.regionID],
                    this.stone.Platforms[stonePlatform]
                        .FileTypes[Path.GetExtension(o.romFileName)],
                    o.romFileName));
        }

        public IEnumerable<DatInfo> GetEverything()
        {
            const string sql = "select regionID, systemID, romHashCRC, romHashMD5, romHashSHA1, romFileName from ROMs";
            return 
                from o in this.Query<dynamic>(sql, null)
                where this.PlatformMap.ContainsKey((int)o.systemID)
                let platform = this.stone.Platforms[this.PlatformMap[(int)o.systemID]]
                let ext = Path.GetExtension(o.romFileName)
                let crc = o.romHashCRC
                where crc != null //only accept hashable files
                select new DatInfo(platform.PlatformID,
                   o.romHashCRC,
                   o.romHashMD5,
                   o.romHashSHA1,
                   this.RegionMap[(int)o.regionID],
                   platform.FileTypes[ext],
                   o.romFileName);
        }
        private readonly IDictionary<int, string> PlatformMap = new Dictionary<int, string>
        {
            {1, "PANASONIC_3DO"},
            {2, "ARCADE_MAME"},
            {3, "ATARI_2600"},
            {4, "ATARI_5200"},
            {5, "ATARI_7800"},
            {6, "ATARI_LYNX"},
            {7, "ATARI_JAGUAR"},
            {8, "ATARI_JAGUAR_CD"},
            {9, "BANDAI_WS"},
            {10, "BANDAI_WSC"},
            {11, "COLECO_CV"},
            {12, "GCE_VECTREX"},
            {13, "MATTEL_INT"},
            {14, "NEC_TG16"},
            {15, "NEC_TGCD"},
            {16, "NEC_PCFX"},
            {17, "NEC_SGFX"},
            {18, "NINTENDO_FDS"},
            {19, "NINTENDO_GB"},
            {20, "NINTENDO_GBA"},
            {21, "NINTENDO_GBC"},
            {22, "NINTENDO_GCN"},
            {23, "NINTENDO_64"},
            {24, "NINTENDO_NDS"},
            {25, "NINTENDO_NES"},
            {26, "NINTENDO_SNES"},
            {27, "NINTENDO_VB"},
            {28, "NINTENDO_WII"},
            {29, "SEGA_32X"},
            {30, "SEGA_GG"},
            {31, "SEGA_SMS"},
            {32, "SEGA_CD"},
            {33, "SEGA_GEN"},
            {34, "SEGA_SAT"},
            {35, "SEGA_SG1000"},
            {36, "SNK_NGP"},
            {37, "SNK_NGPC"},
            {38, "SONY_PSX"},
            {39, "SONY_PSP"}
        };
        private readonly IDictionary<int, string> RegionMap = new Dictionary<int, string>
        {
            { 1, "AS" },
            { 2, "AU" },
            { 3, "BR" },
            { 4, "CA" },
            { 5, "CN" },
            { 6, "DK" },
            { 7, "EU" },
            { 8, "FI" },
            { 9, "FR" },
            { 10, "DE" },
            { 11, "HK"},
            { 12, "IT" },
            { 13, "JP"},
            { 14, "KR"},
            { 15, "NL"}, //Still Netherlands
            { 16, "RU"},
            { 17, "ES"},
            { 18, "SE"},
            { 19, "TW"},
            { 20, "ZZ"},
            { 21, "US"},
            { 22, "ZZ"},
            { 23, "AS-AU"},
            { 24, "BR-KR"},
            { 25, "JP-EU"},
            { 26, "JP-KR"},
            { 27, "JP-US"},
            { 28, "US-AU"},
            { 29, "US-EU"},
            { 30, "US-KR"},
            { 31, "EU-AU"},
            { 32, "GR"},
            { 33, "IE"},
            { 34, "NO"},
            { 35, "PT"},
            { 36, "ZZ"},
            { 37, "GB"},
            { 38, "US-BR"},
            { 39, "PL"},
        };
    }
}
