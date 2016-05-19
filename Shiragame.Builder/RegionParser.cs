using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Snowflake.Romfile;

namespace Shiragame.Builder
{
    internal partial class RegionParser
    {
        internal static string ParseRegion(string name)
        {
            var tagData = Regex.Matches(name,
               @"(\()([^)]+)(\))");
            var validMatch = (from Match tagMatch in tagData
                              let match = tagMatch.Groups[2].Value
                              from regionCode in (from regionCode in match.Split(',', '-') select regionCode.Trim())
                              let isoRegion = RegionParser.ConvertToRegionCode(regionCode.ToUpperInvariant())
                              where isoRegion != null
                              select isoRegion).ToList();
            return !validMatch.Any() ? "ZZ" : String.Join("-", from regionCode in validMatch select regionCode);
        }

        private static string ConvertToRegionCode(string unknownRegion)
        {
            if (RegionParser.goodToolsLookupTable.ContainsKey(unknownRegion))
            {
                return RegionParser.goodToolsLookupTable[unknownRegion];
            }
            if (RegionParser.nointroLookupTable.ContainsKey(unknownRegion))
            {
                return RegionParser.nointroLookupTable[unknownRegion];
            }
            if (RegionParser.tosecLookupTable.Contains(unknownRegion))
            {
                return unknownRegion;
            }
            return null;
        }
    }
}
