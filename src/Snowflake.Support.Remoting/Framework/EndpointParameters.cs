using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointParameters
    {
        public IDictionary<string, string> GetParameters { get; }
        public dynamic PostJsonParameters { get; }

        public EndpointParameters(IDictionary<string, string> getParams, dynamic postParams)
        {
            this.GetParameters = getParams;
            this.PostJsonParameters = postParams;
        }
    }
}
