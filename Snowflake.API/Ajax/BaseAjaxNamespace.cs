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
    public abstract class BaseAjaxNamespace :  BasePlugin, IBaseAjaxNamespace
    {
        public IDictionary<string, Func<IJSRequest, IJSResponse>> JavascriptMethods { get; private set; }

        protected BaseAjaxNamespace(Assembly pluginAssembly, ICoreService coreInstance):base(pluginAssembly, coreInstance)
        {
            this.JavascriptMethods = new Dictionary<string, Func<IJSRequest, IJSResponse>>();
            this.RegisterMethods();
        }

        private void RegisterMethods()
        {
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!method.GetCustomAttributes(typeof(AjaxMethodAttribute), false).Any()) continue;
                if (!(method.ReturnType is IJSResponse)) continue;
                if (!(method.GetParameters().First().ParameterType is IJSRequest)) continue;
                var requestParam = Expression.Parameter(typeof (IJSRequest));
                var instanceRef = Expression.Constant(this, this.GetType());
                var call = Expression.Call(
                    instanceRef, method, new Expression[] {requestParam}
                    );
                var methodAttribute = method.GetCustomAttribute<AjaxMethodAttribute>();
                var methodPrefix = (methodAttribute.MethodPrefix != null) ? methodAttribute.MethodPrefix + "." : "";
                string methodName = (methodAttribute.MethodName != null) ? methodAttribute.MethodName : method.Name;
                this.JavascriptMethods.Add(methodPrefix + methodName,
                    Expression.Lambda<Func<IJSRequest, IJSResponse>>(call, new ParameterExpression[] {requestParam})
                        .Compile());
            }
        }
    }
}
