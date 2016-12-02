using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.Remoting
{
    public class EndpointParameters
    {
        private IDictionary<string, string> StringValues { get; }
        private IDictionary<(string, Type), Lazy<object>> ParsedValues { get; }


        public EndpointParameters(IDictionary<string, string> stringValues)
        {
            
        }

        public T Get<T>(string parameterName)
        {
            
        }

        public object Get(Type type, string parameterName)
        {

        }

    }
}
