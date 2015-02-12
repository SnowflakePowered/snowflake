using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Platform;
using Snowflake.Game;
using Snowflake.Scraper;
namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameInfoScrapedEventArgs : GameEventArgs
    {
        public IScraper GameScraper { get; private set; }

        public GameInfoScrapedEventArgs(ICoreService eventCoreInstance, IGameInfo gameInfo, IScraper gameScraper)
            : base(eventCoreInstance, gameInfo)
        {
            this.GameScraper = gameScraper;

        }
    }
}
