using Snowflake.Platform;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Stone
{
    [Resource("stone", "platforms", ":platformId")]
    [Parameter(typeof(string), "platformId")]
    public class PlatformRoot
    {
        private IStoneProvider StoneProvider { get; }
        public PlatformRoot(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(EndpointVerb.Read)]
        public IPlatformInfo GetPlatform(string platformId)
        {
            return this.StoneProvider.Platforms[platformId];
        }

    }
}
