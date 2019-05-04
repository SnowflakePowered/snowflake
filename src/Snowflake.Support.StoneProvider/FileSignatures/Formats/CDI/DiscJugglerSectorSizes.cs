using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    internal enum DiscJugglerSectorSizes
    {
        Sector2048 = 0,
        Sector2336 = 1, 
        Sector2352 = 2,
        Sector2448 = 4
    }

    internal static class DiscJugglerSectorSizeExtensions
    {
        public static int GetSectorSize(this DiscJugglerSectorSizes @this)
        {
            switch(@this)
            {
                case DiscJugglerSectorSizes.Sector2048:
                    return 2048;
                case DiscJugglerSectorSizes.Sector2336:
                    return 2336;
                case DiscJugglerSectorSizes.Sector2352:
                    return 2352;
                case DiscJugglerSectorSizes.Sector2448:
                    return 2448;
                default:
                    return 0;
            }
        }
    }
}
