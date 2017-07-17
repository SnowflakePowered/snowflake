using Snowflake.Framework.Exceptions;
using Snowflake.Records.Game;
using Snowflake.Remoting.Marshalling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Mappers
{
    public class GameRecordMapping : ITypeMapping<IGameRecord>
    {
        private readonly IGameLibrary gameLibrary;
        public GameRecordMapping(IGameLibrary gameLibrary)
        {
            this.gameLibrary = gameLibrary;
        }

        public IGameRecord ConvertValue(string value)
        {
            if (!Guid.TryParse(value, out Guid gameGuid)) throw new ParseErrorException(value, 
                typeof(IGameRecord));
            return this.gameLibrary.Get(gameGuid);
        }
    }
}
