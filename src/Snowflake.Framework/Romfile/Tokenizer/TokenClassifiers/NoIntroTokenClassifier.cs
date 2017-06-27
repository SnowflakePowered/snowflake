using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.Tokenizer
{
    public class NoIntroTokenClassifier : ITokenClassifier
    {
        public IEnumerable<StructuredFilenameToken> ClassifyBracketsTokens(IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                if (tokenValue == "BIOS")
                {
                    yield return new StructuredFilenameToken(tokenValue,
                                FieldType.DumpInfo,
                                NamingConvention.NoIntro);
                    continue;
                }
                if(tokenValue == "b")
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.DumpInfo,
                        NamingConvention.NoIntro);
                }
            }
        }

        public IEnumerable<StructuredFilenameToken> ClassifyParensTokens(IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                if (tokenValue.Contains(","))
                {
                    foreach(string subToken in tokenValue.Split(",").Select(t => t.Trim(' ')))
                    {
                        if (noIntroCountryLookupTable.Keys.Contains(subToken.ToUpperInvariant()))
                        {
                            yield return new StructuredFilenameToken(noIntroCountryLookupTable[subToken.ToUpperInvariant()],
                                FieldType.Country,
                                NamingConvention.NoIntro);
                            continue;
                        }
                        if (noIntroLanguageLookupTable.Contains(subToken))
                        {
                            yield return new StructuredFilenameToken(subToken.ToLowerInvariant(),
                                FieldType.Language,
                                NamingConvention.NoIntro);
                        }
                    }
                }

                if (noIntroCountryLookupTable.Keys.Contains(tokenValue.ToUpperInvariant()))
                {
                    yield return new StructuredFilenameToken(noIntroCountryLookupTable[tokenValue.ToUpperInvariant()],
                        FieldType.Country,
                        NamingConvention.NoIntro);
                    continue;
                }
                if (noIntroLanguageLookupTable.Contains(tokenValue))
                {
                    yield return new StructuredFilenameToken(tokenValue.ToLowerInvariant(),
                        FieldType.Language,
                        NamingConvention.NoIntro);
                    continue;
                }
                if (tokenValue.StartsWith("v"))
                {
                    yield return new StructuredFilenameToken(tokenValue.Substring(1),
                        FieldType.Version,
                        NamingConvention.NoIntro);
                    continue;
                }
                if (tokenValue.StartsWith("Rev "))
                {
                    yield return new StructuredFilenameToken(tokenValue.Substring(4),
                        FieldType.Version,
                        NamingConvention.NoIntro);
                    continue;
                }
                if(tokenValue.StartsWith("Beta") || tokenValue.StartsWith("Proto") || tokenValue.StartsWith("Sample"))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                       FieldType.DevelopmentStatus,
                       NamingConvention.NoIntro);
                    continue;
                }
                if (tokenValue == "Unl")
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.CopyrightStatus,
                        NamingConvention.NoIntro);
                    continue;
                }
            }
            yield break;
        }

        public IEnumerable<StructuredFilenameToken> ExtractTitleTokens(string title)
        {
            yield return new StructuredFilenameToken(title, FieldType.Title, NamingConvention.NoIntro);
        }

        private static readonly IList<string> noIntroLanguageLookupTable = new List<string>()
        {
            {"En"},
            {"Ja"},
            {"Fr"},
            {"De"},
            {"Es"},
            {"It"},
            {"Nl"},
            {"Pt"},
            {"Sv"},
            {"No"},
            {"Da"},
            {"Fi"},
            {"Zh"},
            {"Ko"},
            {"Pl"}
        };
        internal static readonly IDictionary<string, string> noIntroCountryLookupTable = new Dictionary<string, string>()
        {
            {"AUSTRALIA", "AU"},
            {"BRAZIL", "BR"},
            {"CANADA", "CA"},
            {"CHINA", "CN"},
            {"NETHERLANDS", "NL"},
            {"EUROPE", "EU"},
            {"FRANCE", "FR"},
            {"GERMANY", "DE"},
            {"GREECE", "GR"},
            {"HONG KONG", "HK"},
            {"ITALY", "IT"},
            {"JAPAN", "JP"},
            {"KOREA", "KR"},
            {"NORWAY", "NO"},
            {"RUSSIA", "RU"},
            {"SPAIN", "ES"},
            {"SWEDEN", "SE"},
            {"USA", "US"},
            {"UK", "GB"},
            {"WORLD", "ZZ"},
            {"ASIA", "AS"},
            {"UNKNOWN", "ZZ"}
        };
    }
}
