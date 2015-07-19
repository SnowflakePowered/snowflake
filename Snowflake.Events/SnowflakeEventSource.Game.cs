using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Events.CoreEvents.GameEvent;

namespace Snowflake.Events
{
    public partial class SnowflakeEventSource
    {

        public event EventHandler<GameAddEventArgs> GameAdd;
        public event EventHandler<GameDeleteEventArgs> GameDelete;
        public event EventHandler<GamePreAddEventArgs> GamePreAdd;
        public event EventHandler<GamePreDeleteEventArgs> GamePreDelete;
        public event EventHandler<GameInfoScrapedEventArgs> GameInfoScraped;
        public event EventHandler<GameResultsScrapedEventArgs> GameResultsScraped;
        public event EventHandler<GameStartEventArgs> GameStart;
        public event EventHandler<GameQuitEventArgs> GameQuit;
        public event EventHandler<GameProcessQuitEventArgs> GameProcessQuit;
        public event EventHandler<GameProcessStartEventArgs> GameProcessStart;
       
        public void OnGameAdd(GameAddEventArgs e)
        {
            if (this.GameAdd != null)
            {
                this.GameAdd(this, e);
            }
        }
        public void OnGameDelete(GameDeleteEventArgs e)
        {
            if (this.GameDelete != null)
            {
                this.GameDelete(this, e);
            }
        }
        public void OnGamePreAdd(GamePreAddEventArgs e)
        {
            if (this.GamePreAdd != null)
            {
                this.GamePreAdd(this, e);
            }
        }
        public void OnGamePreDelete(GamePreDeleteEventArgs e)
        {
            if (this.GamePreDelete != null)
            {
                this.GamePreDelete(this, e);
            }
        }
        public void OnGameResultScraped(GameResultsScrapedEventArgs e)
        {
            if (this.GameResultsScraped != null)
            {
                this.GameResultsScraped(this, e);
            }
        }
        public void OnGameInfoScraped(GameInfoScrapedEventArgs e)
        {
            if (this.GameInfoScraped != null)
            {
                this.GameInfoScraped(this, e);
            }
        }
        public void OnGameStart(GameStartEventArgs e)
        {
            if (this.GameStart != null)
            {
                this.GameStart(this, e);
            }
        }
        public void OnGameQuit(GameQuitEventArgs e)
        {
            if (this.GameQuit != null)
            {
                this.GameQuit(this, e);
            }
        }
        public void OnGameProcessStart(GameProcessStartEventArgs e)
        {
            if (this.GameProcessStart != null)
            {
                this.GameProcessStart(this, e);
            }
        }
        public void OnGameProcessQuit(GameProcessQuitEventArgs e)
        {
            if (this.GameProcessQuit != null)
            {
                this.GameProcessQuit(this, e);
            }
        }
    }
}
