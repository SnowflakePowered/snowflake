using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework.Exceptions
{
    public class MetadataNotFoundException : RequestException
    {
        public MetadataNotFoundException(Guid uid, string metadata) : base ($"Could not find metadata {metadata} for record {uid}", 404) {}

    }
}
