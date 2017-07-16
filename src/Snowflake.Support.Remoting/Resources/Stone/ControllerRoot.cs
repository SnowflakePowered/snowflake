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
    [Parameter(typeof(string), "controllerId")]
    public class ControllerRoot : Resource
    {
        private IStoneProvider StoneProvider { get; }
        public ControllerRoot(IStoneProvider provider)
        {
            this.StoneProvider = provider;
        }

        [Endpoint(EndpointVerb.Read)]
        public IControllerLayout GetController(string controllerId)
        {
            return this.StoneProvider.Controllers[controllerId];
        }
    }
}
