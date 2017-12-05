using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shiragame.Builder.Parser
{
    internal class XmlParser : DatParser
    {
        /// <summary>
        /// Parse Logiqx XML dat file
        /// </summary>
        /// <param name="datFile">Path to dat file</param>
        /// <param name="platformId">Platform of dat file</param>
        /// <returns>The information records contained in the file</returns>
        internal static IEnumerable<RomInfo> Parse(string datFile, string platformId)
        {
            return XmlParser.GetEntries(XDocument.Load(File.OpenRead(datFile)), platformId);
        }

        internal static IEnumerable<string> Filetypes(string datFile, string platformId)
        {
            return XmlParser.GetMissingFileTypes(XDocument.Load(File.OpenRead(datFile)), platformId);
        }

        private static IEnumerable<RomInfo> GetEntries(XDocument xmlDat, string platformId)
        {
            return from game in xmlDat.Root.Elements("game").AsParallel()
                   from rom in game.Elements("rom").AsParallel()
                   where rom.Attribute("size").Value != "0"
                   let crc = rom.Attribute("crc").Value.ToUpperInvariant()
                   let md5 = rom.Attribute("md5").Value.ToUpperInvariant()
                   let sha1 = rom.Attribute("sha1").Value.ToUpperInvariant()
                   let filename = rom.Attribute("name").Value
                   let region = RegionParser.ParseRegion(filename)
                   let mimetype = DatParser.GetMimeType(filename, platformId)
                   where mimetype != null
                   select new RomInfo(platformId, crc, md5, sha1, region, mimetype, filename);
        }

        internal  static IEnumerable<string> GetMissingFileTypes(XDocument xmlDat, string platformId)
        {
            return from game in xmlDat.Root.Elements("game").AsParallel()
                   from rom in game.Elements("rom").AsParallel()
                   where rom.Attribute("size").Value != "0"
                   let filename = rom.Attribute("name").Value
                   let ext = Path.GetExtension(filename).ToLowerInvariant()
                   where !stoneProvider.Platforms[platformId].FileTypes.ContainsKey(ext)
                   select "Missing mimetype for " + ext + " at platform " + platformId;
        }
    }
}
