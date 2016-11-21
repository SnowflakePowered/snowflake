using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Snowflake.Extensibility;
using Snowflake.Services;

namespace Snowflake.Ajax
{
    /// <inheritdoc/>
    public abstract class AjaxNamespace : Plugin, IAjaxNamespace
    {
        public IDictionary<string, IJSMethod> JavascriptMethods { get; }

        protected AjaxNamespace(ICoreService coreInstance)
            : base(coreInstance.AppDataDirectory)
        {
            this.JavascriptMethods = new Dictionary<string, IJSMethod>();
            this.RegisterMethods();
        }

        private void RegisterMethods()
        {
            foreach (MethodInfo method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!method.GetCustomAttributes(typeof(AjaxMethodAttribute), false).Any()) continue;
                if (!(method.ReturnType == typeof(IJSResponse))) continue;
                if (!(method.GetParameters().First().ParameterType == typeof(IJSRequest))) continue;
                ParameterExpression requestParam = Expression.Parameter(typeof(IJSRequest));
                ConstantExpression instanceRef = Expression.Constant(this, this.GetType());
                MethodCallExpression call = Expression.Call(instanceRef, method, requestParam);
                AjaxMethodAttribute methodAttribute = method.GetCustomAttribute<AjaxMethodAttribute>();
                string methodPrefix = $"{methodAttribute?.MethodPrefix}." ?? "";
                string methodName = methodAttribute?.MethodName ?? method.Name;
                this.JavascriptMethods.Add(methodPrefix + methodName,
                    new JSMethod(method, Expression.Lambda<Func<IJSRequest, IJSResponse>>(call, requestParam)
                        .Compile()));
            }
        }
    }
}
