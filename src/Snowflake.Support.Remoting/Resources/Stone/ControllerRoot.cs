using Snowflake.Input.Controller;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Stone
{
    [Resource("stone", "controllers", ":controllerId")]
    public class ControllerRoot
    {
        private IStoneProvider StoneProvider { get; }
        public ControllerRoot(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(EndpointVerb.Read)]
        [Parameter(typeof(string), "controllerId")]
        public IControllerLayout GetController(string controllerId)
        {
            return this.StoneProvider.Controllers[controllerId];
        }
    }
}
