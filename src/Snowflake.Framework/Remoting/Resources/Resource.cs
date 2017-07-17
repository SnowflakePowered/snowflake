using Snowflake.Remoting.Marshalling;
using Snowflake.Remoting.Requests;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Snowflake.Remoting.Resources
{
    public abstract class Resource : IResource
    {
        public IResourcePath Path { get; }
        public IEnumerable<IMethodEndpoint> Endpoints { get; }
        protected Resource()
        {
            var path = this.GetType().GetCustomAttributes<ResourceAttribute>()
                .FirstOrDefault()?.Path;
            var pathParams =
                this.GetType().GetCustomAttributes<ParameterAttribute>()
                .Select(p => new Parameter(p.Type, p.Key));
            this.Path = new ResourcePath(pathParams, path);
            this.Endpoints = this.GetMethods().ToImmutableList();
        }

        
        public IMethodEndpoint MatchEndpointWithParams(EndpointVerb verb,
           IEnumerable<IEndpointArgument> requestArguments)
        {
            var reqArgKeys = requestArguments.Select(k => k.Key);

            return (from e in this.Endpoints
                    where e.Verb == verb
                    where e.EndpointParameters.Count() == reqArgKeys.Count()
                    where e.EndpointParameters.All(p => reqArgKeys.Contains(p.Key))
                    select e).FirstOrDefault();
        }

        public object Execute(IMethodEndpoint endpoint,
           IEnumerable<ITypedArgument> typedArgs)
        {
            var methodParams = endpoint.EndpointMethodInfo.GetParameters();
            var typedArgsLookup = typedArgs.ToDictionary(a => a.Key, a => a); // faster lookup
            var paramExpressions = new Expression[methodParams.Length];

            // populate the expressions.
             for (int i = 0; i < paramExpressions.Length; i++)
             {
                 var currentParam = methodParams[i];
                 if (typedArgsLookup.ContainsKey(currentParam.Name) 
                     && typedArgsLookup[currentParam.Name].Type == currentParam.ParameterType)
                 {
                    paramExpressions[i] = Expression.Constant(typedArgsLookup[currentParam.Name].Value,
                        currentParam.ParameterType);
                     continue;
                 }
                 paramExpressions[i] = Expression.Default(currentParam.ParameterType);
             }

            Func<object> result = Expression.Lambda<Func<object>>(
              Expression.Convert(
                  Expression.Call(
                      Expression.Constant(this),
                      endpoint.EndpointMethodInfo,
                      paramExpressions
                  ), typeof(object)
               )
            ).Compile();
            return result.Invoke();
        }

        private IEnumerable<IMethodEndpoint> GetMethods()
        {
            var endpoints =  (from m in this.GetType().GetRuntimeMethods()
                    let endpointAttr = m.GetCustomAttribute<EndpointAttribute>()
                    where endpointAttr != null
                    let endpointParamsAttrs = m.GetCustomAttributes<ParameterAttribute>()
                    let endpointParams = (from p in endpointParamsAttrs select new Parameter(p.Type, p.Key))
                    select new MethodEndpoint(m, endpointAttr.Verb, endpointParams));

            var resourceParamKeys = this.Path.ResourceParameters;

            // filter out endpoints with conflicting signatures as our resource parameters.
            // endpoints that override the resource parameters will be ignored.
            foreach (var endpoint in endpoints)
            {
                bool invalid = false;
                foreach(var param in endpoint.EndpointParameters)
                {
                    if (this.Path.ResourceParameters.Any(p => p.Key == param.Key 
                            && p.Type != param.Type))
                    {
                        invalid = true;
                    }
                }
                if (!invalid) yield return endpoint;
            }
        }
    }
}
