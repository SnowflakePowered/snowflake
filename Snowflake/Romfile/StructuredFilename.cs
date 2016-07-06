using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace Snowflake.Romfile
{
    public partial class StructuredFilename : IStructuredFilename
    {
        private static readonly TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        public StructuredFilenameConvention NamingConvention { get; private set; }

        public string RegionCode { get; }

        public string Title { get; }

        public string Year { get; }

        public string OriginalFilename { get; }

        public StructuredFilename(string originalFilename)
        {
            this.OriginalFilename = Path.GetFileName(originalFilename);
            this.RegionCode = this.ParseRegion();
            this.Title = Path.GetFileNameWithoutExtension(this.ParseTitle());
            this.Year = this.ParseYear();
        }

        private string ParseTitle()
        {
            string rawTitle = Regex.Match(this.OriginalFilename, @"(\([^]]*\))*(\[[^]]*\])*([\w\+\~\@\!\#\$\%\^\&\*\;\,\'\""\?\-\.\-\s]+)").Groups[3].Value.Trim();
            //Invert ending articles
            if (!rawTitle.EndsWith(", The", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", A", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", Die", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", De", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", La", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", Le", StringComparison.InvariantCultureIgnoreCase) &&
                !rawTitle.EndsWith(", Les", StringComparison.InvariantCultureIgnoreCase))
                return rawTitle;

            string[] splitString = rawTitle.Split(',');
            string endingArticle = splitString.Last().Trim();
            string withoutArticle = String.Join(",", splitString.Reverse().Skip(1).Reverse());
            return $"{endingArticle} {withoutArticle}";
        }

        private string ParseYear()
        {
            if (this.NamingConvention == StructuredFilenameConvention.NoIntro) return null;
            var tagData = Regex.Matches(this.OriginalFilename,
                @"(\()([^)]+)(\))");
            return (from Match tagMatch in tagData
                let match = tagMatch.Groups[2].Value.Trim()
                where match.Length == 4 && (match.StartsWith("19") || match.StartsWith("20"))
                select match).FirstOrDefault();
        }

        private string ParseRegion()
        {
            var tagData = Regex.Matches(this.OriginalFilename,
               @"(\()([^)]+)(\))");
            var validMatch = (from Match tagMatch in tagData
                             let match = tagMatch.Groups[2].Value
                             from regionCode in (from regionCode in match.Split(',', '-') select regionCode.Trim())
                             where regionCode.Length != 2 || StructuredFilename.textInfo.ToTitleCase(regionCode.ToLower()) != regionCode
                             let isoRegion = StructuredFilename.ConvertToRegionCode(regionCode.ToUpperInvariant())
                             where isoRegion.Item1 != null
                             select isoRegion).ToList();
            if (!validMatch.Any())
            {
                this.NamingConvention = StructuredFilenameConvention.Unknown;
                return "ZZ";
            }
            this.NamingConvention = validMatch.First().Item2;
            return String.Join("-", from regionCode in validMatch select regionCode.Item1);
        }

        private static Tuple<string, StructuredFilenameConvention> ConvertToRegionCode(string unknownRegion)
        {
            if (StructuredFilename.goodToolsLookupTable.ContainsKey(unknownRegion))
            {
                return Tuple.Create(StructuredFilename.goodToolsLookupTable[unknownRegion], StructuredFilenameConvention.GoodTools);
            }
            if (StructuredFilename.nointroLookupTable.ContainsKey(unknownRegion))
            {
                return Tuple.Create(StructuredFilename.nointroLookupTable[unknownRegion], StructuredFilenameConvention.NoIntro);
            }
            if (StructuredFilename.tosecLookupTable.Contains(unknownRegion))
            {
                return Tuple.Create(unknownRegion, StructuredFilenameConvention.TheOldSchoolEmulationCenter);
            }
            return Tuple.Create<string, StructuredFilenameConvention>(null, StructuredFilenameConvention.Unknown);
        }

    }
}
