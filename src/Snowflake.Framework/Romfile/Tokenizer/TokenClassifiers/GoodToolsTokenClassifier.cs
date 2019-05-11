using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.Tokenizer
{
    public class GoodToolsTokenClassifier : ITokenClassifier
    {
        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ClassifyBracketsTokens(
            IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                if (goodToolsDumpFlags.Contains(tokenValue.Replace("?", string.Empty)))
                {
                    yield return new StructuredFilenameToken(tokenValue.Replace("?", string.Empty),
                        FieldType.DumpInfo,
                        NamingConvention.GoodTools);
                    continue;
                }

                if (tokenValue.StartsWith("hI") || tokenValue == "BF" || tokenValue.StartsWith("PRG"))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.DumpInfo,
                        NamingConvention.GoodTools);
                }

                if (tokenValue == "M" || tokenValue == "hFFE" || tokenValue == "C" || tokenValue == "S" ||
                    tokenValue == "hMxx")
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.System,
                        NamingConvention.GoodTools);
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ClassifyParensTokens(
            IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                if (goodToolsCountryLookupTable.Keys.Contains(tokenValue.ToUpperInvariant()))
                {
                    yield return new StructuredFilenameToken(goodToolsCountryLookupTable[tokenValue.ToUpperInvariant()],
                        FieldType.Country,
                        NamingConvention.GoodTools);
                    continue;
                }

                switch (tokenValue)
                {
                    case "1":
                        yield return new StructuredFilenameToken("JP",
                            FieldType.Country,
                            NamingConvention.GoodTools);
                        yield return new StructuredFilenameToken("KR",
                            FieldType.Country,
                            NamingConvention.GoodTools);
                        continue;
                    case "4":
                        yield return new StructuredFilenameToken("US",
                            FieldType.Country,
                            NamingConvention.GoodTools);
                        yield return new StructuredFilenameToken("BR",
                            FieldType.Country,
                            NamingConvention.GoodTools);
                        continue;
                    case "PD":
                    case "Unl":
                        yield return new StructuredFilenameToken(tokenValue,
                            FieldType.CopyrightStatus,
                            NamingConvention.GoodTools);
                        continue;
                    case "PC10":
                    case "VS":
                    case "M":
                    case "Adam":
                    case "BS":
                    case "ST":
                        yield return new StructuredFilenameToken(tokenValue,
                            FieldType.System,
                            NamingConvention.GoodTools);
                        continue;
                    case "5":
                        yield return new StructuredFilenameToken("NTSC",
                            FieldType.Video,
                            NamingConvention.GoodTools);
                        break;
                    case "8":
                        yield return new StructuredFilenameToken("PAL",
                            FieldType.Video,
                            NamingConvention.GoodTools);
                        break;
                }

                if (tokenValue.StartsWith("M") && tokenValue.Length == 2 &&
                    int.TryParse(tokenValue.Substring(1), out _))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Language,
                        NamingConvention.GoodTools);
                    continue;
                }

                if (tokenValue.EndsWith("k") && tokenValue.Length == 3 &&
                    int.TryParse(tokenValue[0..^2], out _))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.DumpInfo,
                        NamingConvention.GoodTools);
                    continue;
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ExtractTitleTokens(string title)
        {
            yield return new StructuredFilenameToken(title, FieldType.Title, NamingConvention.GoodTools);
        }

        private static readonly IList<string> goodToolsDumpFlags = new List<string>()
        {
            {"a"},
            {"b"},
            {"f"},
            {"o"},
            {"h"},
            {"!p"},
            {"p"},
            {"t"},
            {"T-"},
            {"T+"},
            {"!"},
        };

        internal static readonly IDictionary<string, string> goodToolsCountryLookupTable =
            new Dictionary<string, string>()
            {
                {"A", "AU"},
                {"AS", "AS"},
                {"B", "BR"},
                {"C", "CA"},
                {"CH", "CN"},
                {"D", "NL"}, // D FOR DUTCH (NETHERLANDS)
                {"E", "EU"},
                {"F", "FR"},
                {"G", "DE"},
                {"GR", "GR"},
                {"HK", "HK"},
                {"I", "IT"},
                {"J", "JP"},
                {"K", "KR"},
                {"NK", "NL"}, // STILL NETHERLANDS
                {"NO", "NO"},
                {"R", "RU"},
                {"S", "ES"},
                {"SW", "SE"},
                {"U", "US"},
                {"UK", "GB"},
                {"W", "ZZ"},
                {"FC", "CA"},
                {"UNK", "ZZ"},
            };
    }
}
