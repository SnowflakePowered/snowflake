using Snowflake.Platform;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Resources.Stone
{
    [Resource("stone", "platforms")]
    public class PlatformsRoot
    {
        private IStoneProvider StoneProvider { get; }
        public PlatformsRoot(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(EndpointVerb.Read)]
        public IEnumerable<IPlatformInfo> ListPlatforms()
        {
            return this.StoneProvider.Platforms.Select(p => p.Value);
        }
    }
}
