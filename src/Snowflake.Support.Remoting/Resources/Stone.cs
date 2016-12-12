using Snowflake.Input.Controller;
using Snowflake.Platform;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.Resources
{
    public class Stone
    {
        private IStoneProvider StoneProvider { get; }
        public Stone(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        public IEnumerable<IPlatformInfo> ListPlatforms()
        {
            return this.StoneProvider.Platforms.Select(p => p.Value);
        }

        public IPlatformInfo GetPlatform(string platformId)
        {
            return this.StoneProvider.Platforms[platformId];
        }

        public IEnumerable<IControllerLayout> ListControllers()
        {
            return this.StoneProvider.Controllers.Select(c => c.Value);
        }

        public IControllerLayout GetController(string controllerId)
        {
            return this.StoneProvider.Controllers[controllerId];
        }
    }
}
