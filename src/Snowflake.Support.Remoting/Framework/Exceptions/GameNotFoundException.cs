using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(Guid uid) : base ($"Could not find game {uid}") {}

    }
}
