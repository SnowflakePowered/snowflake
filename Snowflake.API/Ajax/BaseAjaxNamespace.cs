using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Snowflake.Plugin;
using System.Linq;

namespace Snowflake.Ajax
{
    public abstract class BaseAjaxNamespace : BasePlugin, IBaseAjaxNamespace
    {
        public IDictionary<string, Func<JSRequest, JSResponse>> JavascriptMethods { get; private set; }

        protected BaseAjaxNamespace(Assembly pluginAssembly):base(pluginAssembly)
        {
            this.JavascriptMethods = new Dictionary<string, Func<JSRequest, JSResponse>>();
            this.RegisterMethods();
        }

        private void RegisterMethods()
        {
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if(!method.GetCustomAttributes(typeof(AjaxMethod), false).Any()) continue;
                var requestParam = Expression.Parameter(typeof (JSRequest));
                var call = Expression.Call(
                    null, method, new Expression[] {requestParam}
                    );
                this.JavascriptMethods.Add(method.Name,
                    Expression.Lambda<Func<JSRequest, JSResponse>>(call, new ParameterExpression[] {requestParam})
                        .Compile());
            }
        }
    }
}
