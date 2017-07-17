using Snowflake.Input.Controller;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using Snowflake.Services;
using Snowflake.Support.Remoting.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Stone
{
    [Resource("stone", "controllers", ":controller")]
    [Parameter(typeof(IControllerLayout), "controller")]
    public class ControllerRoot : Resource
    {
        [Endpoint(EndpointVerb.Read)]
        public IControllerLayout GetController(IControllerLayout controller)
        {
            return controller;
        }
    }
}
