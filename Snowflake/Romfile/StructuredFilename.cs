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
    public class StructuredFilename : IStructuredFilename
    {
        public StructuredFilenameConvention NamingConvention { get; private set; }

        public string RegionCode { get; }

        public string Title { get; }

        public string Year { get; }

        public string OriginalFilename { get; }

        public StructuredFilename(string originalFilename)
        {
            this.OriginalFilename = Path.GetFileName(originalFilename);
            this.RegionCode = this.ParseRegion();
            this.Title = this.ParseTitle();
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

            return tagData.Cast<Match>().Select(tag => tag.Groups[2].Value.Trim()).FirstOrDefault(value => value.Length == 4 && (value.StartsWith("19") || value.StartsWith("20")));
        }

        private string ParseRegion()
        {
            var tagData = Regex.Matches(this.OriginalFilename,
               @"(\()([^)]+)(\))");
            foreach (string match in from Match tag in tagData select tag.Groups[2].Value.Trim().ToLowerInvariant())
            {
                if (StructuredFilename.goodToolsLookupTable.ContainsKey(match))
                {
                    this.NamingConvention = StructuredFilenameConvention.GoodTools; //Look up Goodtools code first
                    return StructuredFilename.goodToolsLookupTable[match];
                }
                if (StructuredFilename.nointroLookupTable.ContainsKey(match))
                {
                    this.NamingConvention = StructuredFilenameConvention.NoIntro; //Look up no intro country names
                    return StructuredFilename.nointroLookupTable[match];
                }

                if (match.Contains(",")) //If multiple no intro country names
                {
                    var countries = match.Split(',');
                    this.NamingConvention = StructuredFilenameConvention.NoIntro;
                    return String.Join("-", countries.Where(country => StructuredFilename.nointroLookupTable.ContainsKey(country.Trim())).Select(country => StructuredFilename.nointroLookupTable[country.Trim()]));
                }

                if (match.Length == 2) //Assume any 2 letter string in brackets is a region code
                {
                    try
                    {
                        var info = new RegionInfo(match); //validate code
                        this.NamingConvention = StructuredFilenameConvention.TheOldSchoolEmulationCenter;
                        return match;
                    }
                    catch (ArgumentException)
                    {
                        continue;
                    }
     
                }
                if (match.Contains("-")) //validate multi-region codes
                {
                    string[] countries = match.Split('-');
                    var countryCode = new StringBuilder();
                    foreach (string country in countries)
                    {
                        try
                        {
                            var info = new RegionInfo(country);
                            this.NamingConvention = StructuredFilenameConvention.TheOldSchoolEmulationCenter;
                            countryCode.Append($"-{country}");
                        }
                        catch (ArgumentException)
                        {
                            continue;
                        }
                    }
                    string returnCountry = countryCode.ToString().Substring(1);
                    return returnCountry.Length > 5 ? "ZZ" : returnCountry;
                }
            }
            this.NamingConvention = StructuredFilenameConvention.Unknown;
            return "ZZ";
        }

        private static readonly IDictionary<string, string> goodToolsLookupTable = new Dictionary<string, string>()
        {
            {"a", "AU"},
            {"as", "AS"},
            {"b", "BR"},
            {"c", "CA"},
            {"ch", "CN"},
            {"d", "NL"}, //D for Dutch (Netherlands)
            {"e", "EU"},
            {"f", "FR"},
            {"g", "DE"},
            {"gr", "GR"},
            {"hk", "HK"},
            {"i", "IT"},
            {"j", "JP"},
            {"k", "KR"},
            {"nk", "NL"}, //Still Netherlands
            {"no", "NO"},
            {"r", "RU"},
            {"s", "ES"},
            {"sw", "SE"},
            {"u", "US"},
            {"uk", "GB"},
            {"w", "ZZ"},
            {"unl", "ZZ"},
            {"pd", "ZZ"},
            {"unk", "ZZ"}
        };

        private static readonly IDictionary<string, string> nointroLookupTable = new Dictionary<string, string>()
        {
            {"australia", "AU"},
            {"brazil", "BR"},
            {"canada", "CA"},
            {"china", "CN"},
            {"netherlands", "NL"},
            {"europe", "EU"},
            {"france", "FR"},
            {"germany", "DE"},
            {"greece", "GR"},
            {"hong kong", "HK"},
            {"italy", "IT"},
            {"japan", "JP"},
            {"korea", "KR"},
            {"norway", "NO"},
            {"russia", "RU"},
            {"spain", "ES"},
            {"sweden", "SE"},
            {"usa", "US"},
            {"uk", "GB"},
            {"world", "ZZ"},
            {"asia", "AS"},
            {"unknown", "ZZ"}
        };
    }
}
