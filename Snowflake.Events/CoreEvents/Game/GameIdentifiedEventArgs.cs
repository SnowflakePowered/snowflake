using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Service;
using Snowflake.Platform;

namespace Snowflake.Events.CoreEvents.GameEvent
{
    public class GameIdentifiedEventArgs : SnowflakeEventArgs
    {
        public string IdentifiedGameString { get; set; }
        public string GameFileName { get; private set; }
        public IPlatformInfo GamePlatform { get; private set; }
        public GameIdentifiedEventArgs(ICoreService eventCoreInstance, string identifiedGameString, string gameFileName, IPlatformInfo gamePlatform) : base(eventCoreInstance){

            this.IdentifiedGameString = identifiedGameString;
            this.GameFileName = gameFileName;
            this.GamePlatform = gamePlatform;
        }
    }
}
