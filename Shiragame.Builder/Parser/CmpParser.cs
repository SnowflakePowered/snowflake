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

namespace Shiragame.Builder.Parser
{
    internal class CmpParser : DatParser
    {
        /// <summary>
        /// Parse ClrMamePro dat files
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<DatInfo> Parse(string datFile, string platformId)
        {
            var regex = Regex.Matches(File.ReadAllText(datFile), @"(rom \()(.+)(\))");
            return CmpParser.GetEntries(regex, platformId);
        }


        /// <summary>
        /// Parse ClrMamePro dat files
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<SerialInfo> ParseSerials(string datFile, string platformId)
        {
            var lines = File.ReadAllLines(datFile);
            return CmpParser.GetSerials(CmpParser.GetGameEntries(lines), platformId);
        }

        private static IEnumerable<IDictionary<string, string>> GetGameEntries(string[] datContents)
        {
            IDictionary<string, string> gameEntry = new Dictionary<string, string>();
            foreach (string line in datContents)
            {
                if (line.EndsWith("("))
                {
                    gameEntry = new Dictionary<string, string>();
                    continue;
                }
                if (line.Equals(")"))
                {
                    yield return gameEntry;
                    continue;
                }
                var lines = line.Trim().Split(new char[] {' '}, 2, StringSplitOptions.None);
                gameEntry.Add(lines[0], lines[1].Trim('"'));
            }
        }
        private static IEnumerable<SerialInfo> GetSerials(IEnumerable<IDictionary<string, string>> cmpMatches, string platformId)
        {
            return from entry in cmpMatches
                   where entry.ContainsKey("serial")
                   let name = entry["name"]
                   let serials = entry["serial"]
                   let region = RegionParser.ParseRegion(name)
                   select new SerialInfo(platformId, name, region, serials);
        }

        private static IEnumerable<DatInfo> GetEntries(MatchCollection cmpMatches, string platformId)
        {
            const string regex = @"(?:\s|)(.*?)(?:\sname|\scrc|\scrc32|\smd5|\ssha1|\sbaddump|\snodump|\ssize|$)";
            return from Match romEntry in cmpMatches
                   let match = romEntry.Value
                   let filename = Regex.Match(match, "name" + regex).Groups[1].Value.Trim('"')
                   let crc = Regex.Match(match, "crc" + regex).Groups[1].Value
                   let md5 = Regex.Match(match, "md5" + regex).Groups[1].Value
                   let sha1 = Regex.Match(match, "sha1" + regex).Groups[1].Value
                   let region = RegionParser.ParseRegion(filename)
                   let mimetype = stoneProvider.Platforms[platformId].FileTypes[Path.GetExtension(filename)]
                   select new DatInfo(platformId, crc, md5, sha1, region, mimetype, filename);
        }
    }
}
