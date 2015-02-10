using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Platform;
using Snowflake.Game;
using Snowflake.Scraper;
namespace Snowflake.Events.CoreEvents
{
    public class GameScrapedEventArgs : GameEventArgs
    {
        public IScraper GameScraper { get; set; }

        public GameScrapedEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IScraper gameScraper)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameScraper = gameScraper;

        }
    }
}
