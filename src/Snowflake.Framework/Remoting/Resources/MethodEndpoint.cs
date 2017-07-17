using Snowflake.Remoting.Marshalling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Snowflake.Remoting.Resources
{
    public class MethodEndpoint : IMethodEndpoint
    {
        public MethodInfo EndpointMethodInfo { get; }
        public EndpointVerb Verb { get; }
        public IEnumerable<IParameter> EndpointParameters { get; }

        internal MethodEndpoint(MethodInfo interfaceMethod, EndpointVerb verb, 
            IEnumerable<IParameter> endpointParams)
        {
            this.EndpointMethodInfo = interfaceMethod;
            this.Verb = verb;
            this.EndpointParameters = endpointParams;
        }

        public IEnumerable<ISerializedArgument> SerializeEndpointArguments(IEnumerable<IEndpointArgument> args)
        {
            return (from p in this.EndpointParameters
             from a in args
             where p.Key == a.Key
             select new SerializedArgument(p.Key, a.StringValue, p.Type));
        }
    }
}
