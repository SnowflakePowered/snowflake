using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using GraphQL.Builders;
using GraphQL.Relay.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Query
{
    public abstract partial class QueryBuilder
    {
        private static MethodInfo ConnectionMaker { get; }
        static QueryBuilder()
        {
            MethodInfo GetMethod<T>(Expression<Func<ResolveConnectionContext<T>, IEnumerable<T>, T>> expression)
            {
                MethodCallExpression methodCall = (MethodCallExpression)expression.Body;
                return methodCall.Method;
            }

            QueryBuilder.ConnectionMaker = GetMethod<object>((context, items) =>
                                               ConnectionUtils.ToConnection(items, context)).GetGenericMethodDefinition();
        }

        private IEnumerable<(MethodInfo fieldMethod, TAttribute attr, IEnumerable<ParameterAttribute> paramAttr)>
            EnumerateQueries<TAttribute>()
            where TAttribute : Attribute
        {
            var endpoints = from m in this.GetType().GetRuntimeMethods()
                             let queryAttr = m.GetCustomAttribute<TAttribute>()
                             where queryAttr != null
                             let endpointParamsAttrs = m.GetCustomAttributes<ParameterAttribute>()
                             select (methodInfo: m, queryAttr: queryAttr, paramAttrs: endpointParamsAttrs);
            foreach (var endpoint in endpoints)
            {
                this.VerifyParameters(endpoint.methodInfo, endpoint.paramAttrs);
            }

            return endpoints;
        }

        private IEnumerable<(MethodInfo fieldMethod, FieldAttribute fieldAttr, IEnumerable<ParameterAttribute> paramAttr)>
            EnumerateFieldQueries() => this.EnumerateQueries<FieldAttribute>();

        private IEnumerable<(MethodInfo fieldMethod, ConnectionAttribute connectionAttr,
                    IEnumerable<ParameterAttribute> paramAttr)> EnumerateConnectionQueries()
            => this.EnumerateQueries<ConnectionAttribute>();

        private IEnumerable<(MethodInfo fieldMethod, MutationAttribute fieldAttr,
                   IEnumerable<ParameterAttribute> paramAttr)> EnumerateMutationQueries()
           => this.EnumerateQueries<MutationAttribute>();

        private FieldQuery MakeFieldQuery(MethodInfo fieldMethod, FieldAttribute fieldAttr, IEnumerable<ParameterAttribute> paramAttr)
        {
            var invoker = this.CreateInvoker(fieldMethod);
            var resolver = this.CreateResolver(invoker, fieldMethod);
            var methodParams = fieldMethod.GetParameters();
            IList<QueryArgument> arguments = (from param in paramAttr
                                              select new QueryArgument(param.GraphQlType)
                                              {
                                                  Name = param.Key,
                                                  Description = param.Description,
                                                  DefaultValue = methodParams.FirstOrDefault(p => p.Name == param.Key)?.DefaultValue,
                                              }).ToList();
            return new FieldQuery()
            {
                Description = fieldAttr.Description,
                Name = fieldAttr.FieldName,
                GraphType = fieldAttr.GraphType,
                Arguments = arguments.Count != 0 ? new QueryArguments(arguments) : null,
                Resolver = resolver,
            };
        }

        private FieldQuery MakeMutationQuery(MethodInfo fieldMethod, MutationAttribute fieldAttr, IEnumerable<ParameterAttribute> paramAttr)
        {
            var invoker = this.CreateInvoker(fieldMethod);
            var resolver = this.CreateResolver(invoker, fieldMethod);
            var methodParams = fieldMethod.GetParameters();
            IList<QueryArgument> arguments = (from param in paramAttr
                                              select new QueryArgument(param.GraphQlType)
                                              {
                                                  Name = param.Key,
                                                  Description = param.Description,
                                                  DefaultValue = methodParams.FirstOrDefault(p => p.Name == param.Key)?.DefaultValue,
                                              }).ToList();
            return new FieldQuery()
            {
                Description = fieldAttr.Description,
                Name = fieldAttr.FieldName,
                GraphType = fieldAttr.GraphType,
                Arguments = arguments.Count != 0 ? new QueryArguments(arguments) : null,
                Resolver = resolver,
            };
        }

        private ConnectionQuery MakeConnectionQuery(MethodInfo fieldMethod, ConnectionAttribute
            fieldAttr, IEnumerable<ParameterAttribute> paramAttr)
        {
            var invoker = this.CreateInvoker(fieldMethod);
            var resolver = this.CreateResolver(invoker, fieldMethod);
            var methodParams = fieldMethod.GetParameters();
            IList<QueryArgument> arguments = (from param in paramAttr
                                                    select new QueryArgument(param.GraphQlType)
                                                    {
                                                        Name = param.Key,
                                                        Description = param.Description,
                                                        DefaultValue = methodParams.FirstOrDefault(p => p.Name == param.Key)?.DefaultValue,
                                                    }).ToList();
            if (!fieldMethod.ReturnType.GetInterfaces().Contains(typeof(IEnumerable)) ||
                !fieldMethod.ReturnType.IsConstructedGenericType)
            {
                throw new ArgumentOutOfRangeException("Connections must be of generic type IEnumerable!");
            }

            return new ConnectionQuery()
            {
                Description = fieldAttr.Description,
                Name = fieldAttr.FieldName,
                GraphType = fieldAttr.GraphType,
                Arguments = arguments.Count != 0 ? new QueryArguments(arguments) : null,
                Resolver = resolver,
                ItemsType = fieldMethod.ReturnType.GetGenericArguments()[0],
                ReturnType = fieldMethod.ReturnType,
            };
        }

        private Func<ResolveConnectionContext<object>, object>
            CreateConnectionResolver(ConnectionQuery query)
        {
            var connectionBuildCall = QueryBuilder.ConnectionMaker.MakeGenericMethod(query.ItemsType, typeof(object));
            ParameterExpression context = Expression.Parameter(typeof(ResolveConnectionContext<object>), "context");
            Expression<Func<ResolveFieldContext<object>, object>> resolvedValue = invokingContext =>
                query.Resolver(invokingContext);

            var executor = Expression.Lambda<Func<ResolveConnectionContext<object>, object>>(
                Expression.Call(
                    null,
                    connectionBuildCall,
                    Expression.Convert(Expression.Invoke(resolvedValue, context), query.ReturnType),
                    context), context);
            return executor.Compile();
        }

        private void RegisterQuery(ConnectionQuery query, ObjectGraphType<object> rootQuery)
        {
            var connectionResolver = CreateConnectionResolver(query);
            var connectionQuery = rootQuery.Connection<IGraphType>()
                .Name(query.Name)
                .Description(query.Description);
            connectionQuery.FieldType.Type = typeof(ConnectionType<>).MakeGenericType(query.GraphType);
            if (query.Arguments != null)
            {
                connectionQuery.FieldType.Arguments.AddRange(query.Arguments);
            }

            connectionQuery.Resolve(context => connectionResolver(context));
        }

        private void RegisterQuery(FieldQuery query, ObjectGraphType<object> rootQuery)
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

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }

            return null;
        }

        /// <summary>
        /// Collects parameters from a context from an invoking method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="invokingMethod"></param>
        /// <returns></returns>
        private static object[] CollectParameters(ResolveFieldContext<object> context, MethodInfo invokingMethod)
        {
            IList<object> paramValues = new List<object>();
            foreach (var parameter in invokingMethod.GetParameters())
            {
                try
                {
                    paramValues.Add(context.GetArgument(parameter.ParameterType, parameter.Name, parameter.RawDefaultValue));
                }
                catch (FormatException)
                {
                    paramValues.Add(QueryBuilder.GetDefaultValue(parameter.ParameterType));
                }
            }

            return paramValues.ToArray();
        }

        private Func<ResolveFieldContext<object>, object>
            CreateResolver(Func<object[], object> invoker, MethodInfo invokingMethod) =>
            this.CreateResolver<object>(invoker, invokingMethod);

        /// <summary>
        /// Creates a GraphQL resolver given a compiled invoker.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="invoker"></param>
        /// <param name="invokingMethod"></param>
        /// <returns></returns>
        private Func<ResolveFieldContext<object>, T>
            CreateResolver<T>(Func<object[], T> invoker, MethodInfo invokingMethod)
        {
            Func<ResolveFieldContext<object>, T> resolver = context =>
            {
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
        private Func<object[], T> CreateInvoker<T>(MethodInfo invokingMethod)
        {
            ParameterExpression argumentList = Expression.Parameter(typeof(object[]), "argumentList");
            var parameters = invokingMethod.GetParameters();
            var parameterRetrievals = new List<Expression>();

            for (int i = 0; i < parameters.Length; i++)
            {
                parameterRetrievals.Add(Expression.Convert(
                        Expression.ArrayIndex(argumentList,
                        Expression.Constant(i)),
                   parameters[i].ParameterType));
            }

            var executor = Expression.Lambda<Func<object[], T>>(
                Expression.Convert(Expression.Call(
                    Expression.Constant(this),
                    invokingMethod,
                    parameterRetrievals), typeof(object)),
                argumentList);
            return executor.Compile();
        }

        private Func<object[], object> CreateInvoker(MethodInfo invokingMethod) => this.CreateInvoker<object>(invokingMethod);
    }
}
