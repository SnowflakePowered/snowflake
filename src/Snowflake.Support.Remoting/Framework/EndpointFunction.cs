using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Snowflake.Support.Remoting.Framework
{
    public class EndpointFunction
    {
        private EndpointFunction(Expression<Func<EndpointParameters, EndpointResponse>> function)
        {

        }

        public static EndpointFunction Make<T1, TResult>(Expression<Func<T1, TResult>> method, string[] args)
        {
            var paramNames = args.Where(arg => arg.StartsWith("{") && arg.EndsWith("}"))
                .Select(arg => arg.Substring(1, arg.Length - 1))
                .ToList();
            var methodParams = method.Parameters.Select(p => (name:p.Name, type:p.Type)).ToList();
            return new EndpointFunction(x => new EndpointResponse(method.Compile().Invoke((T1)x.Get(methodParams[0].type, methodParams[0].name))));
        }
    }
}
