using Snowflake.Input.Controller;
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
    [Resource("stone", "controllers")]
    public class ControllersRoot
    {
        private IStoneProvider StoneProvider { get; }
        public ControllersRoot(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(EndpointVerb.Read)]
        public IEnumerable<IControllerLayout> ListControllers()
        {
            return this.StoneProvider.Controllers.Select(c => c.Value);
        }
    }
}
