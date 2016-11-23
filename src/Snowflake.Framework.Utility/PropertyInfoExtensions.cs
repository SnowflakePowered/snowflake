using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Snowflake.Utility
{
    public static class PropertyInfoExtensions
    {
        public static bool HasAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.IsDefined(typeof(T));
        }
    }
}
