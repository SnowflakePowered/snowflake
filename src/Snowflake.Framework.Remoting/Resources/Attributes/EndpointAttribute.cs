using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.Resources.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class EndpointAttribute : Attribute
    {
        public EndpointVerb Verb { get; }
        public EndpointAttribute(EndpointVerb endpointVerb)
        {
            this.Verb = endpointVerb;
        }
    }
}
