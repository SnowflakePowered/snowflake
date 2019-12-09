using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Caching
{
    /// <summary>
    /// Standard types of images.
    /// </summary>
    public static class ImageTypes
    {
        /// <summary>
        /// Front box art
        /// </summary>
        public static readonly string MediaBoxartFront = "media_boxart_front";

        /// <summary>
        /// Back box art
        /// </summary>
        public static readonly string MediaBoxartBack = "media_boxart_back";

        /// <summary>
        /// Manual page
        /// </summary>
        public static readonly string MediaManualPage = "media_manual_page";

        /// <summary>
        /// Game icon
        /// </summary>
        public static readonly string MediaIcon = "media_icon";

        /// <summary>
        /// Promotional material, such as promotional screenshots, advertisements, etc.
        /// </summary>
        public static readonly string MediaPromotional = "media_promotional";

        /// <summary>
        /// Game logoes
        /// </summary>
        public static readonly string MediaLogo = "media_logo";

        /// <summary>
        /// An arcade cabinet marquee
        /// </summary>
        public static readonly string MediaMarquee = "media_marquee";

        /// <summary>
        /// A game screenshot
        /// </summary>
        public static readonly string Screenshot = "screenshot";

        /// <summary>
        /// A generic image
        /// </summary>
        public static readonly string Generic = "generic";
    }
}
