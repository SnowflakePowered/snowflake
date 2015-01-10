using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraper
{
    /// <summary>
    /// Types of game images
    /// </summary>
    public enum GameImageType
    {
        /// <summary>
        /// A fanart of the game
        /// </summary>
        IMAGE_FANART,
        /// <summary>
        /// A screenshot of the game
        /// </summary>
        IMAGE_SCREENSHOT,
        /// <summary>
        /// The front of the game boxart
        /// </summary>
        IMAGE_BOXART_FRONT,
        /// <summary>
        /// The back of the game boxart
        /// </summary>
        IMAGE_BOXART_BACK,
        /// <summary>
        /// The entire game boxart
        /// </summary>
        IMAGE_BOXART_FULL
    }
}
