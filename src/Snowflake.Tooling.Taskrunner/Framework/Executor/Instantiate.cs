using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Tooling.Taskrunner.Framework.Executor
{
    internal static class Instantiate
    {
        private static ConstructorInfo GetConstructor(BindingFlags flags, Type type, Type[] constructorParams)
        {
            var constructors = from constructor in type.GetConstructors(flags)
                let parameters = constructor.GetParameters().Select(p => p.ParameterType)
                where !parameters.Except(constructorParams).Any()
                where parameters.Count() == constructorParams.Count()
                select constructor;
            return constructors.First();
        }

        public static object CreateInstance(Type createType)
        {
            return Instantiate.CreateInstance(createType, Type.EmptyTypes);
        }

        public static object CreateInstance(Type createType, Type[] constructorParams)
        {
            Func<object> instanceCreator = Expression.Lambda<Func<object>>(
                    Expression.New(Instantiate.GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, createType,
                        constructorParams)))
                .Compile();
            return instanceCreator();
        }
    }
}
