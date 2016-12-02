using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public abstract class Endpoint
    {
        protected ICoreService CoreService { get; }

        protected Endpoint(ICoreService coreService)
        {
            this.CoreService = coreService;
        }
    }
}
