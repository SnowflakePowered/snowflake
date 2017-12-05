using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Snowflake.Romfile;
using Snowflake.Utility;

namespace Shiragame.Builder
{
    static internal partial class RegionParser
    {
        private static readonly TextInfo textInfo = new CultureInfo("en-US").TextInfo;

        internal static string ParseRegion(string name)
        {
            var tagData = Regex.Matches(name,
               @"(\()([^)]+)(\))");
            var validMatch = (from Match tagMatch in tagData
                              let match = tagMatch.Groups[2].Value
                              from regionCode in from regionCode in match.Split(',', '-') select regionCode.Trim()
                              where regionCode.Length != 2 || regionCode.ToLower().ToTitleCase() != regionCode

                              // allow FR & France to be parsed, but not Fr inside En,Fr,De, etc..
                              let isoRegion = RegionParser.ConvertToRegionCode(regionCode.ToUpperInvariant())
                              where isoRegion != null
                              select isoRegion).ToList();
            return !validMatch.Any() ? "ZZ" : string.Join("-", from regionCode in validMatch select regionCode);
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
