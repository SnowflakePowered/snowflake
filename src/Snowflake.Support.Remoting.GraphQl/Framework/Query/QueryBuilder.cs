using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using EnumsNET;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using System.Linq;
using System.Linq.Expressions;

namespace Snowflake.Support.Remoting.GraphQl.Framework.Query
{
    public abstract class QueryBuilder
    {
   
        private void GetFieldQueries(MethodInfo fieldMethod)
        {
            var endpoints = (from m in this.GetType().GetRuntimeMethods()
                             let fieldQueryAttr = m.GetCustomAttribute<FieldAttribute>()
                             where fieldQueryAttr != null
                             let endpointParamsAttrs = m.GetCustomAttributes<ParameterAttribute>()
                             let methodParameters = m.GetParameters()
                             select (m, fieldQueryAttr, endpointParamsAttrs));
            
            var fieldAttr = fieldMethod.Attributes.GetAttributes().Get<FieldAttribute>();
            var paramAttr = fieldMethod.Attributes.GetAttributes().Get<ParameterAttribute>();
        }

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
