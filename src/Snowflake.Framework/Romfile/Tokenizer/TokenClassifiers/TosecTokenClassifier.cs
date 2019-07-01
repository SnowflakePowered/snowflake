using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Romfile.Tokenizer
{
    internal class TosecTokenClassifier : ITokenClassifier
    {
        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ClassifyParensTokens(
            IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            bool hasDemo = false;
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                if (tokenValue.StartsWith("demo") && tokenPosition == 0)
                {
                    hasDemo = true;
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Demo,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tokenValue.StartsWith("19") || tokenValue.StartsWith("20") && tokenPosition == (hasDemo ? 1 : 0))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Date,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tokenValue.Contains("-"))
                {
                    foreach (string subToken in tokenValue.Split("-"))
                    {
                        if (tosecLanguageLookupTable.Contains(subToken))
                        {
                            yield return new StructuredFilenameToken(subToken,
                                FieldType.Language,
                                NamingConvention.TheOldSchoolEmulationCenter);
                            continue;
                        }

                        if (tosecCountryTable.Contains(subToken))
                        {
                            yield return new StructuredFilenameToken(subToken,
                                FieldType.Country,
                                NamingConvention.TheOldSchoolEmulationCenter);
                            continue;
                        }

                        if (subToken.StartsWith("19") ||
                            subToken.StartsWith("20") && tokenPosition == (hasDemo ? 1 : 0))
                        {
                            yield return new StructuredFilenameToken(subToken,
                                FieldType.Date,
                                NamingConvention.TheOldSchoolEmulationCenter);
                            continue;
                        }
                    }
                }

                if (tokenValue.StartsWith("M") && tokenValue.Length == 2 &&
                    int.TryParse(tokenValue.Substring(1), out _))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Language,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecLanguageLookupTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Language,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecCountryTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Country,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecVideoLookupTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Video,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecSystemLookupTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.System,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecDevLookupTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.DevelopmentStatus,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                if (tosecCopyrightLookupTable.Contains(tokenValue) && tokenPosition > (hasDemo ? 2 : 1))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.DevelopmentStatus,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                foreach (string startsWith in tosecMediaLookupTable)
                {
                    if (tokenValue.StartsWith(startsWith) && tokenPosition > (hasDemo ? 2 : 1))
                    {
                        if (tokenValue.Contains(" of "))
                        {
                            yield return new StructuredFilenameToken(tokenValue,
                                FieldType.MediaType,
                                NamingConvention.TheOldSchoolEmulationCenter);
                            break;
                        }

                        if (tokenValue.StartsWith("Side"))
                        {
                            yield return new StructuredFilenameToken(tokenValue,
                                FieldType.MediaType,
                                NamingConvention.TheOldSchoolEmulationCenter);
                            break;
                        }

                        continue;
                    }
                }

                if ((tokenPosition == (hasDemo ? 2 : 1)) &&
                    !NoIntroTokenClassifier.noIntroCountryLookupTable.Keys.Contains(tokenValue.ToUpperInvariant()) &&
                    !GoodToolsTokenClassifier.goodToolsCountryLookupTable.Keys.Contains(tokenValue.ToUpper()))
                {
                    yield return new StructuredFilenameToken(tokenValue,
                        FieldType.Publisher,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ClassifyBracketsTokens(
            IEnumerable<(string tokenValue, int tokenPosition)> tokens)
        {
            foreach ((string tokenValue, int tokenPosition) in tokens)
            {
                string dumpInfo = tokenValue.Split(' ')[0];
                if (tosecDumpInfoLookupTable.Contains(dumpInfo))
                {
                    yield return new StructuredFilenameToken(dumpInfo,
                        FieldType.DumpInfo,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }
            }

            yield break;
        }

        /// <inheritdoc/>
        public IEnumerable<StructuredFilenameToken> ExtractTitleTokens(string title)
        {
            var titleTokens = title.Split(" ");
            var titleBuilder = new StringBuilder();
            bool nextTokenIsRevision = false;
            int lastTokenIndex = titleTokens.Count() - 1;
            for (int i = 0; i < lastTokenIndex + 1; i++)
            {
                string token = titleTokens[i];
                if (token.StartsWith("v") && token.Contains(".") && i == lastTokenIndex)
                {
                    bool isVersion = true;
                    foreach (char v in token.ToCharArray().Take(4))
                    {
                        if (v == 'v')
                        {
                            continue;
                        }

                        if (v == '.')
                        {
                            continue;
                        }

                        isVersion = int.TryParse(v.ToString(), out _);
                    }

                    if (isVersion)
                    {
                        yield return new StructuredFilenameToken(token.Substring(1),
                            FieldType.Version,
                            NamingConvention.TheOldSchoolEmulationCenter);
                        continue;
                    }
                }

                if (token.StartsWith("v") && i == lastTokenIndex && long.TryParse(token.ToString().Substring(1), out _))
                {
                    yield return new StructuredFilenameToken(token.Substring(1),
                        FieldType.Version,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                // the only game this will fail on is Guilty Gear Xrd Rev 2.
                if (token == "Rev" && i == lastTokenIndex - 1 && !titleBuilder
                        .ToString().Contains("Guilty Gear Xrd")) // nice special case.
                {
                    nextTokenIsRevision = true;
                    continue;
                }

                if (nextTokenIsRevision)
                {
                    yield return new StructuredFilenameToken(token,
                        FieldType.Version,
                        NamingConvention.TheOldSchoolEmulationCenter);
                    continue;
                }

                titleBuilder.Append(token + " ");
            }

            yield return new StructuredFilenameToken(titleBuilder.ToString().Trim(' '),
                FieldType.Title,
                NamingConvention.TheOldSchoolEmulationCenter);
        }

        #region Lookup Tables

        private static readonly IList<string> tosecDumpInfoLookupTable = new List<string>()
        {
            {"cr"},
            {"f"},
            {"h"},
            {"m"},
            {"p"},
            {"t"},
            {"tr"},
            {"o"},
            {"u"},
            {"v"},
            {"b"},
            {"a"},
            {"!"},
        };

        private static readonly IList<string> tosecMediaLookupTable = new List<string>()
        {
            {"Disc"},
            {"Disk"},
            {"File"},
            {"Part"},
            {"Side"},
            {"Tape"},
        };

        private static readonly IList<string> tosecCopyrightLookupTable = new List<string>()
        {
            {"CW"},
            {"CW-R"},
            {"FW"},
            {"GW"},
            {"GW-R"},
            {"LW"},
            {"PD"},
            {"SW"},
            {"SW-R"},
        };

        private static readonly IList<string> tosecDevLookupTable = new List<string>()
        {
            {"alpha"},
            {"beta"},
            {"preview"},
            {"pre-release"},
            {"proto"},
        };

        private static readonly IList<string> tosecSystemLookupTable = new List<string>()
        {
            {"+2"},
            {"+2a"},
            {"+3"},
            {"130XE"},
            {"A1000"},
            {"A1200"},
            {"A1200-A4000"},
            {"A2000"},
            {"A2000-A3000"},
            {"A2024"},
            {"A2500-A3000UX"},
            {"A3000"},
            {"A4000"},
            {"A4000T"},
            {"A500"},
            {"A500+"},
            {"A500-A1000-A2000"},
            {"A500-A1000-A2000-CDTV"},
            {"A500-A1200"},
            {"A500-A1200-A2000-A4000"},
            {"A500-A2000"},
            {"A500-A600-A2000"},
            {"A570"},
            {"A600"},
            {"A600HD"},
            {"AGA"},
            {"AGA-CD32"},
            {"Aladdin Deck Enhancer"},
            {"CD32"},
            {"CDTV"},
            {"Computrainer"},
            {"Doctor PC Jr."},
            {"ECS"},
            {"ECS-AGA"},
            {"Executive"},
            {"Mega ST"},
            {"Mega-STE"},
            {"OCS"},
            {"OCS-AGA"},
            {"ORCH80"},
            {"Osbourne 1"},
            {"PIANO90"},
            {"PlayChoice-10"},
            {"Plus4"},
            {"Primo-A"},
            {"Primo-A64"},
            {"Primo-B"},
            {"Primo-B64"},
            {"Pro-Primo"},
            {"ST"},
            {"STE"},
            {"STE-Falcon"},
            {"TT"},
            {"TURBO-R GT"},
            {"TURBO-R ST"},
            {"VS DualSystem"},
            {"VS UniSystem"},
        };

        private static readonly IList<string> tosecVideoLookupTable = new List<string>()
        {
            {"CGA"},
            {"EGA"},
            {"HGC"},
            {"MCGA"},
            {"MDA"},
            {"NTSC"},
            {"NTSC-PAL"},
            {"PAL"},
            {"PAL-60"},
            {"PAL-NTSC"},
            {"SVGA"},
            {"VGA"},
            {"XGA"},
        };

        private static readonly IList<string> tosecLanguageLookupTable = new List<string>
        {
            {"ar"},
            {"bg"},
            {"bs"},
            {"cs"},
            {"cy"},
            {"da"},
            {"de"},
            {"el"},
            {"en"},
            {"eo"},
            {"es"},
            {"et"},
            {"fa"},
            {"fi"},
            {"fr"},
            {"ga"},
            {"gu"},
            {"he"},
            {"hi"},
            {"hr"},
            {"hu"},
            {"is"},
            {"it"},
            {"ja"},
            {"ko"},
            {"lt"},
            {"lv"},
            {"ms"},
            {"nl"},
            {"no"},
            {"pl"},
            {"pt"},
            {"ro"},
            {"ru"},
            {"sk"},
            {"sl"},
            {"sq"},
            {"sr"},
            {"sv"},
            {"th"},
            {"tr"},
            {"ur"},
            {"vi"},
            {"yi"},
            {"zh"},
        };

        private static readonly IList<string> tosecCountryTable = new List<string>
        {
            {"AE"},
            {"AL"},
            {"AS"},
            {"AT"},
            {"AU"},
            {"BA"},
            {"BE"},
            {"BG"},
            {"BR"},
            {"CA"},
            {"CH"},
            {"CL"},
            {"CN"},
            {"CS"},
            {"CY"},
            {"CZ"},
            {"DE"},
            {"DK"},
            {"EE"},
            {"EG"},
            {"ES"},
            {"EU"},
            {"FI"},
            {"FR"},
            {"GB"},
            {"GR"},
            {"HK"},
            {"HR"},
            {"HU"},
            {"ID"},
            {"IE"},
            {"IL"},
            {"IN"},
            {"IR"},
            {"IS"},
            {"IT"},
            {"JO"},
            {"JP"},
            {"KR"},
            {"LT"},
            {"LU"},
            {"LV"},
            {"MN"},
            {"MX"},
            {"MY"},
            {"NL"},
            {"NO"},
            {"NP"},
            {"NZ"},
            {"OM"},
            {"PE"},
            {"PH"},
            {"PL"},
            {"PT"},
            {"QA"},
            {"RO"},
            {"RU"},
            {"SE"},
            {"SG"},
            {"SI"},
            {"SK"},
            {"TH"},
            {"TR"},
            {"TW"},
            {"US"},
            {"VN"},
            {"YU"},
            {"ZA"},
        };

        #endregion
    }
}
