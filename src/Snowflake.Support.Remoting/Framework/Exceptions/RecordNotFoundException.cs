using Snowflake.Records.Game;
using Snowflake.Remoting.Requests;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class RecordNotFoundException : RequestException
    {
        public RecordNotFoundException(Guid uid) : base ($"Could not find record {uid}", 404) {}

    }
}
