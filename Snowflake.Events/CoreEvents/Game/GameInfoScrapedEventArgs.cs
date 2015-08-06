using Snowflake.Game;
using Snowflake.Scraper;
using Snowflake.Service;

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
