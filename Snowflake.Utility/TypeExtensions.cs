using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    public static class TypeExtensions
    {
        public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type)
        {
            if (!type.IsInterface)
                return type.GetProperties();

            return  type.GetInterfaces()
                    .Concat(new Type[] { type })
                    .SelectMany(i => i.GetProperties());
        }
        
        public static IEnumerable<T> GetPublicAttributes<T>(this Type type) where T: System.Attribute
        {
            if (!type.IsInterface)
                return type.GetCustomAttributes<T>();
            return type.GetInterfaces()
                   .Concat(new Type[] { type })
                   .SelectMany(i => i.GetCustomAttributes<T>());
        }
    }
}
