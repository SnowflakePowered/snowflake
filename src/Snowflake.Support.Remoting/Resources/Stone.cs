using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Support.Remoting.Framework;

namespace Snowflake.Support.Remoting.Resources
{
    public class Stone
    {
        private IStoneProvider StoneProvider { get; }
        public Stone(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(RequestVerb.Read, "~:stone:platforms")]
        public IEnumerable<IPlatformInfo> ListPlatforms()
        {
            return this.StoneProvider.Platforms.Select(p => p.Value);
        }

        [Endpoint(RequestVerb.Read, "~:stone:platforms:{platformId}")]
        public IPlatformInfo GetPlatform(string platformId)
        {
            return this.StoneProvider.Platforms[platformId];
        }

        [EndpointAttribute(RequestVerb.Read, "~:stone:controllers")]
        public IEnumerable<IControllerLayout> ListControllers()
        {
            return this.StoneProvider.Controllers.Select(c => c.Value);
        }

        [Endpoint(RequestVerb.Read, "~:stone:controllers:{controllerId}")]
        public IControllerLayout GetController(string controllerId)
        {
            return this.StoneProvider.Controllers[controllerId];
        }
    }
}
