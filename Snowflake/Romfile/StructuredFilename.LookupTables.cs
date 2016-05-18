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
