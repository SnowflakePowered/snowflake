using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Snowflake.Romfile;
using Snowflake.Service;

namespace Shiragame.Builder
{
    internal static class DatParser
    {
        private static readonly IStoneProvider stoneProvider = new StoneProvider();

        /// <summary>
        /// Parse ClrMamePro dat files
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<DatInfo> ParseClrMamePro(string datFile, string platformId)
        {
            var regex = Regex.Matches(File.ReadAllText(datFile), @"(rom \()(.+)(\))");
            return DatParser.GetEntries(regex, platformId);
        }

        /// <summary>
        /// Parse Logiqx XML dat file
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<DatInfo> ParseLogiqx(string datFile, string platformId)
        {
            return DatParser.GetEntries(XDocument.Load(File.OpenRead(datFile)), platformId);
        }

        private static IEnumerable<DatInfo> GetEntries(MatchCollection cmpMatches, string platformId)
        {
            const string regex = @"(?:\s|)(.*?)(?:\sname|\scrc|\scrc32|\smd5|\ssha1|\sbaddump|\snodump|\ssize|$)";
            return from Match romEntry in cmpMatches.AsParallel()
                   let match = romEntry.Value
                   let filename = Regex.Match(match, "name" + regex).Groups[1].Value.Trim('"')
                   let crc = Regex.Match(match, "crc" + regex).Groups[1].Value
                   let md5 = Regex.Match(match, "md5" + regex).Groups[1].Value
                   let sha1 = Regex.Match(match, "sha1" + regex).Groups[1].Value
                   let region = new StructuredFilename(filename).RegionCode
                   let mimetype = DatParser.stoneProvider.Platforms[platformId].FileTypes[Path.GetExtension(filename)]
                   select new DatInfo(platformId, crc, md5, sha1, region, mimetype, filename);
        }

        private static IEnumerable<DatInfo> GetEntries(XDocument xmlDat, string platformId)
        {
            return from game in xmlDat.Root.Elements("game").AsParallel()
                from rom in game.Elements("rom")
                where rom.Attribute("size").Value != "0"
                let crc = rom.Attribute("crc").Value.ToUpperInvariant()
                let md5 = rom.Attribute("md5").Value.ToUpperInvariant()
                let sha1 = rom.Attribute("sha1").Value.ToUpperInvariant()
                let filename = rom.Attribute("name").Value 
                let region = new StructuredFilename(filename).RegionCode
                let mimetype = DatParser.stoneProvider.Platforms[platformId].FileTypes[Path.GetExtension(filename)]
                select new DatInfo(platformId, crc, md5, sha1, region, mimetype, filename);
        }

    }
}
