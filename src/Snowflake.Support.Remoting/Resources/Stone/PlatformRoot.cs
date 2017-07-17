using Snowflake.Platform;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Stone
{
    [Resource("stone", "platforms", ":platform")]
    [Parameter(typeof(IPlatformInfo), "platform")]
    public class PlatformRoot : Resource
    {
        [Endpoint(EndpointVerb.Read)]
        public IPlatformInfo GetPlatform(IPlatformInfo platform)
        {
            return platform;
        }

    }
}
