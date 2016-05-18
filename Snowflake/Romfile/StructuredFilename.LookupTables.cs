using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile
{
    public partial class StructuredFilename
    {
	
        private static readonly IList<string> tosecLookupTable = new List<string>
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
            {"ZA"}
        };
        private static readonly IDictionary<string, string> goodToolsLookupTable = new Dictionary<string, string>()
        {
            {"A", "AU"},
            {"AS", "AS"},
            {"B", "BR"},
            {"C", "CA"},
            {"CH", "CN"},
            {"D", "NL"}, //D FOR DUTCH (NETHERLANDS)
            {"E", "EU"},
            {"F", "FR"},
            {"G", "DE"},
            {"GR", "GR"},
            {"HK", "HK"},
            {"I", "IT"},
            {"J", "JP"},
            {"K", "KR"},
            {"NK", "NL"}, //STILL NETHERLANDS
            {"NO", "NO"},
            {"R", "RU"},
            {"S", "ES"},
            {"SW", "SE"},
            {"U", "US"},
            {"UK", "GB"},
            {"W", "ZZ"},
        };

        private static readonly IDictionary<string, string> nointroLookupTable = new Dictionary<string, string>()
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
