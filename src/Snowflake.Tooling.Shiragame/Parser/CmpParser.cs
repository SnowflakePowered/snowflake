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
using Snowflake.Services;

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
        internal static IEnumerable<RomInfo> Parse(string datFile, string platformId)
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
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                
                var lines = line.Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.None);
                if (lines[0].Equals("rom")) continue;
                gameEntry.Add(lines[0], lines[1].Trim('"'));
            }
        }
        private static IEnumerable<SerialInfo> GetSerials(IEnumerable<IDictionary<string, string>> cmpMatches, string platformId)
        {
            return from entry in cmpMatches.AsParallel()
                where entry.ContainsKey("serial")
                let name = entry["name"]
                let serials = platformId.StartsWith("NINTENDO") ? 
                        Regex.Replace(entry["serial"], "[A-Z]*?-[A-Z]-", "") : entry["serial"] //sanitize nintendo serials
                let region = RegionParser.ParseRegion(name)
                select new SerialInfo(platformId, name, region, serials);
        }

        private static IEnumerable<RomInfo> GetEntries(MatchCollection cmpMatches, string platformId)
        {
            const string regex = @"(?:\s|)(.*?)(?:\sname|\scrc|\scrc32|\smd5|\ssha1|\sbaddump|\snodump|\ssize|$)";
            return from Match romEntry in cmpMatches.AsParallel()
                   let match = romEntry.Value
                   let filename = Regex.Match(match, "name" + regex).Groups[1].Value.Trim('"')
                   let crc = Regex.Match(match, "crc" + regex).Groups[1].Value
                   let md5 = Regex.Match(match, "md5" + regex).Groups[1].Value
                   let sha1 = Regex.Match(match, "sha1" + regex).Groups[1].Value
                   let region = RegionParser.ParseRegion(filename)
                   let mimetype = DatParser.GetMimeType(filename, platformId)
                   where mimetype != null
                   select new RomInfo(platformId, crc, md5, sha1, region, mimetype, filename);
        }
    }
}
