using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    internal class EndpointParameters
    {
        public IDictionary<string, string> Get { get; }
        public JObject Post { get; }

        public EndpointParameters(IDictionary<string, string> getParams, dynamic postParams)
        {
            this.Get = getParams;
            this.Post = postParams;
        }
    }
}
