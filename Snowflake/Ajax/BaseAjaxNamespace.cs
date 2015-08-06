using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Snowflake.Plugin;
using System.Linq;
using Snowflake.Service;

namespace Snowflake.Ajax
{
    /// <inheritdoc/>
    public abstract class BaseAjaxNamespace : BasePlugin, IBaseAjaxNamespace
    {
        public IDictionary<string, IJSMethod> JavascriptMethods { get; private set; }

        protected BaseAjaxNamespace(Assembly pluginAssembly, ICoreService coreInstance)
            : base(pluginAssembly, coreInstance)
        {
            this.JavascriptMethods = new Dictionary<string, IJSMethod>();
            this.RegisterMethods();
        }

        private void RegisterMethods()
        {
            foreach (MethodInfo method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!method.GetCustomAttributes(typeof(AjaxMethodAttribute), false).Any()) continue;
                if (!(method.ReturnType.Equals(typeof(IJSResponse)))) continue;
                if (!(method.GetParameters().First().ParameterType.Equals(typeof(IJSRequest)))) continue;
                ParameterExpression requestParam = Expression.Parameter(typeof(IJSRequest));
                ConstantExpression instanceRef = Expression.Constant(this, this.GetType());
                MethodCallExpression call = Expression.Call(
                    instanceRef, method, new Expression[] { requestParam }
                    );
                AjaxMethodAttribute methodAttribute = method.GetCustomAttribute<AjaxMethodAttribute>();
                string methodPrefix = $"{methodAttribute?.MethodPrefix}." ?? "";
                string methodName = methodAttribute.MethodName ?? method.Name;
                this.JavascriptMethods.Add(methodPrefix + methodName,
                    new JSMethod(method, Expression.Lambda<Func<IJSRequest, IJSResponse>>(call, new ParameterExpression[] { requestParam })
                        .Compile()));
            }
        }
    }
}
