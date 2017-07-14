using Snowflake.Framework.Remoting.Marshalling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowflake.Framework.Remoting.Resources
{
    public class MethodEndpoint
    {
        public MethodInfo EndpointMethodInfo { get; }
        public EndpointVerb Verb { get; }
        public IEnumerable<Parameter> EndpointParameters { get; }

        internal MethodEndpoint(MethodInfo interfaceMethod, EndpointVerb verb, 
            IEnumerable<Parameter> endpointParams)
        {
            this.EndpointMethodInfo = interfaceMethod;
            this.Verb = verb;
            this.EndpointParameters = endpointParams;
        }

        public IEnumerable<SerializedArgument> MatchArguments(IEnumerable<EndpointArgument> args)
        {
            return (from p in this.EndpointParameters
             from a in args
             where p.Key == a.Key
             select new SerializedArgument(p.Key, a.StringValue, p.Type));
        }
    }
}
