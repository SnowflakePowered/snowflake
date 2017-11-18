using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using System.Linq;
using System.Linq.Expressions;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Query
{
    public abstract class QueryBuilder
    { 

        internal IEnumerable<(MethodInfo fieldMethod, FieldAttribute fieldAttr, IEnumerable<ParameterAttribute> paramAttr)> EnumerateFieldQueries()
        {
            var endpoints = (from m in this.GetType().GetRuntimeMethods()
                             let fieldQueryAttr = m.GetCustomAttribute<FieldAttribute>()
                             where fieldQueryAttr != null
                             let endpointParamsAttrs = m.GetCustomAttributes<ParameterAttribute>()
                             select (m, fieldQueryAttr, endpointParamsAttrs));
            return endpoints;
        }

        internal FieldQuery MakeFieldQuery(MethodInfo fieldMethod, FieldAttribute fieldAttr, IEnumerable<ParameterAttribute> paramAttr)
        {
            var invoker = this.CreateInvoker(fieldMethod);
            var resolver = this.CreateResolver(invoker, fieldMethod);
            var methodParams = fieldMethod.GetParameters();
            Console.WriteLine(typeof(GraphType).IsAssignableFrom(typeof(StringGraphType)));
            /*IList<QueryArgument> arguments = (from param in paramAttr
                                                    select new QueryArgument(param.GraphQlType) {
                                                        Name = param.Key,
                                                        Description = param.Description,
                                                        DefaultValue = methodParams.FirstOrDefault(p => p.Name == param.Key)?.DefaultValue,
                                                        
                                                    }).ToList();*/
            IList<QueryArgument> arguments = new List<QueryArgument>();
            foreach(var param in paramAttr)
            {
                var queryArg = new QueryArgument(param.GraphQlType)
                {
                    Name = param.Key,
                    Description = param.Description,
                    DefaultValue = methodParams.FirstOrDefault(p => p.Name == param.Key)?.DefaultValue,
                };
                arguments.Add(queryArg);
            }
            return new FieldQuery()
            {
                Description = fieldAttr.Description,
                Name = fieldAttr.FieldName,
                GraphType = fieldAttr.GraphType,
                Arguments = arguments.Count != 0 ? new QueryArguments(arguments) : null,
                Resolver = resolver
            };

        }

        internal void RegisterFieldQuery(FieldQuery query, ObjectGraphType<object> rootQuery)
        {
            rootQuery.Field(query.GraphType, query.Name, query.Description, query.Arguments, query.Resolver);
        }
 
        /// <summary>
        /// Verify parameters for a query function.
        /// </summary>
        /// <param name="invokingMethod"></param>
        /// <param name="attributes"></param>
        private void VerifyParameters(MethodInfo invokingMethod, IEnumerable<ParameterAttribute> attributes)
        {
            foreach (var param in invokingMethod.GetParameters())
            {
                var paramDesc = attributes.FirstOrDefault(p => p.Key == param.Name);
                if (paramDesc == null)
                {
                    throw new ArgumentOutOfRangeException($"Parameter {param.Name} is not described for method {invokingMethod.Name}!");
                }
                if (paramDesc.ParameterType != param.ParameterType)
                {
                    throw new InvalidCastException($"Parameter {param.Name} has mismatched types!");
                }
            }
        }

        /// <summary>
        /// Collects parameters from a context from an invoking method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="invokingMethod"></param>
        /// <returns></returns>
        private static object[] CollectParameters(ResolveFieldContext<object> context, MethodInfo invokingMethod)
        {
            return (from parameter in invokingMethod.GetParameters()
                    select context.GetArgument(parameter.ParameterType, parameter.Name, parameter.RawDefaultValue)).ToArray();
        }

        private Func<ResolveFieldContext<object>, object>
            CreateResolver(Func<object[], object> invoker, MethodInfo invokingMethod)
        {
            Func<ResolveFieldContext<object>, object> resolver = context => {
                var args = QueryBuilder.CollectParameters(context, invokingMethod);
                return invoker(args);
            };
            return resolver;
        }

        /// <summary>
        /// Creates a compiled invoker for a method info that accepts a list of arguments in an object array.
        /// If called with the arity of the array does not match, will throw an exception. Usage is similar to 
        /// MethodInfo.Invoke, but invoking time is similar or the same as a direct call if cached.
        /// </summary>
        /// <param name="invokingMethod">The methodinfo to invoke on.</param>
        /// <returns>A function that invokes the given method when given a list of parameters.
        /// </returns>
        private Func<object[], object> CreateInvoker(MethodInfo invokingMethod)
        {
            ParameterExpression argumentList = Expression.Parameter(typeof(object[]), "argumentList");
            var parameters = invokingMethod.GetParameters();
            var parameterRetrievals = new List<Expression>();

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterRetrievals.Add(Expression.Convert(Expression.ArrayIndex(argumentList, Expression.Constant(i)), parameters[i].ParameterType));
            }

            var executor = Expression.Lambda<Func<object[], object>>(
                Expression.Call(
                    Expression.Constant(this),
                    invokingMethod,
                    parameterRetrievals
                ), argumentList);
            return executor.Compile();
        }
    }
}
