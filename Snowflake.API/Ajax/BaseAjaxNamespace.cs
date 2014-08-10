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
    public abstract class BaseAjaxNamespace :  IBaseAjaxNamespace
    {
        public IDictionary<string, Func<JSRequest, JSResponse>> JavascriptMethods { get; private set; }

        protected BaseAjaxNamespace(Assembly pluginAssembly)
        {
            this.JavascriptMethods = new Dictionary<string, Func<JSRequest, JSResponse>>();
            this.RegisterMethods();
        }

        private void RegisterMethods()
        {
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if(!method.GetCustomAttributes(typeof(AjaxMethod), false).Any()) continue;
                if (method.ReturnType != typeof(JSResponse)) continue;
                if (method.GetParameters().First().ParameterType != typeof(JSRequest)) continue;
                var requestParam = Expression.Parameter(typeof (JSRequest));
                var instanceRef = Expression.Constant(this, this.GetType());
                var call = Expression.Call(
                    instanceRef, method, new Expression[] {requestParam}
                    );
                this.JavascriptMethods.Add(method.Name,
                    Expression.Lambda<Func<JSRequest, JSResponse>>(call, new ParameterExpression[] {requestParam})
                        .Compile());
            }
        }
    }
}
